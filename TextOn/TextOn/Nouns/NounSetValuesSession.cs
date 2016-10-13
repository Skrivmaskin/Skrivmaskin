using System;
using System.Linq;
using System.Collections.Generic;

namespace TextOn.Nouns
{
    /// <summary>
    /// The user's current session for setting Noun values.
    /// </summary>
    public sealed class NounSetValuesSession
    {
        private readonly IReadOnlyDictionary<string, Noun> nouns;
        private readonly string [] nounNames;
        private readonly Dictionary<string, string> values = new Dictionary<string, string> ();
        private bool allValuesAreSet = false;
        private Dictionary<string, IEnumerable<NounSuggestion>> currentSuggestions = new Dictionary<string, IEnumerable<NounSuggestion>> ();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Nouns.NounSetValuesSession"/> class.
        /// </summary>
        /// <remarks>
        /// Edits are not allowed while there is a session open - this is guaranteed by the profile.
        /// </remarks>
        /// <param name="nouns">The noun profile from the design template.</param>
        internal NounSetValuesSession (NounProfile nouns)
        {
            var allNouns = nouns.GetAllNouns ();
            this.nouns = allNouns.ToDictionary ((noun) => noun.Name);
            this.nounNames = allNouns.Select ((n) => n.Name).ToArray();

            // We treat anything null or whitespace as an invalid value.
            foreach (var nounName in nounNames) {
                values.Add (nounName, "");
            }
            allValuesAreSet = (nounNames.Length == 0);
            CopySuggestions ();
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <returns>The name.</returns>
        /// <param name="index">Index.</param>
        public string GetName (int index)
        {
            return nounNames [index];
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <returns>The description.</returns>
        /// <param name="name">Name.</param>
        public string GetDescription (string name)
        {
            return nouns [name].Description;
        }

        /// <summary>
        /// Gets the accepts user value.
        /// </summary>
        /// <returns><c>true</c>, if accepts user value was gotten, <c>false</c> otherwise.</returns>
        /// <param name="name">Name.</param>
        public bool GetAcceptsUserValue (string name)
        {
            return nouns [name].AcceptsUserValue;
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count {
            get {
                return nounNames.Length;
            }
        }

        /// <summary>
        /// Gets the current suggestions for noun.
        /// </summary>
        /// <returns>The current suggestions for noun.</returns>
        /// <param name="name">Name.</param>
        public string [] GetCurrentSuggestionsForNoun (string name)
        {
            return currentSuggestions [name].Select ((s) => s.Value).ToArray();
        }

        private void CopySuggestions ()
        {
            // take a copy of the suggestions at this point - no filters to apply
            foreach (var kvp in this.nouns) {
                var noun = kvp.Value;
                var li = new List<NounSuggestion> ();
                // Careful - taking actual references here - but we're only going to filter them and reset, no edits allowed.
                li.AddRange (noun.Suggestions);
                this.currentSuggestions.Add (noun.Name, li);
            }
        }

        private void RecalculateSuggestions ()
        {
            // Take a copy of the original suggestions
            currentSuggestions.Clear ();
            CopySuggestions ();

            // given all the current inputs for each noun, we filter down the possible
            // suggestions of all other nouns that are dependent on this one
            // Note: we do not do the reverse ordering, it is only reasonable to do so for a chain
            // of Nouns that do not accept user values (there might be two Hyde Parks for all the
            // machine knows)
            //TODO notify users for a non-AcceptUserValue dependent variable that their value stinks
            for (var i = 0; i < nounNames.Length; ++i)
            {
                var nounName = nounNames [i];
                var nounValue = values [nounName];
                if (!String.IsNullOrWhiteSpace (nounValue))
                {
                    for (int j = i + 1; j < nounNames.Length; j++) {
                        var maybeDependentNounName = nounNames [j];
                        var maybeDependentSuggestions = currentSuggestions [maybeDependentNounName];
                        var newMaybeDependentSuggestions =
                            maybeDependentSuggestions
                                .Where ((suggestion) => HasNoInvalidSuggestions (nounName, nounValue, suggestion.Dependencies));
                        currentSuggestions [maybeDependentNounName] = newMaybeDependentSuggestions;
                    }
                }
            }
            foreach (var nounName in nounNames) {
                FireSuggestionsUpdated (nounName);
            }
        }

        private void FireSuggestionsUpdated (string name)
        {
            SuggestionsUpdated?.Invoke (name);
        }

        private bool HasNoInvalidSuggestions (string nounName, string nounValue, IEnumerable<NounSuggestionDependency> dependencies)
        {
            foreach (var dependency in dependencies) {
                if (dependency.Name == nounName)
                    return nounValue == dependency.Value;
            }
            return true;
        }

        /// <summary>
        /// Gets the noun values selected by the user.
        /// </summary>
        /// <value>The noun values.</value>
        public IReadOnlyDictionary<string, string> NounValues {
            get {
                return values;
            }
        }

        /// <summary>
        /// Gets a value indicating whether every Noun has a value.
        /// </summary>
        /// <value><c>true</c> if all values are set; otherwise, <c>false</c>.</value>
        public bool AllValuesAreSet {
            get {
                return allValuesAreSet;
            }
        }

        /// <summary>
        /// Set the value for a Noun.
        /// </summary>
        /// <param name="nounName">Noun name.</param>
        /// <param name="value">Value.</param>
        public void SetValue (string nounName, string value)
        {
            values [nounName] = value;
            var newAllValuesAreSet = allValuesAreSet || (!HasAnyUnsetValues ());
            if (allValuesAreSet != newAllValuesAreSet) {
                allValuesAreSet = newAllValuesAreSet;
                AllValuesAreSetUpdated?.Invoke (allValuesAreSet);
            }
            RecalculateSuggestions ();
        }

        private bool HasAnyUnsetValues ()
        {
            foreach (var item in values) {
                if (String.IsNullOrWhiteSpace (item.Value)) return true;
            }
            return false;
        }

        public void Deactivate ()
        {
            Deactivating?.Invoke ();
        }

        /// <summary>
        /// Notifies users of the session that the set of suggestions has changed for this noun.
        /// </summary>
        public event Action<string> SuggestionsUpdated;

        /// <summary>
        /// Notifies users of a change in validity of <see cref="NounValues"/>. 
        /// </summary>
        public event Action<bool> AllValuesAreSetUpdated;

        /// <summary>
        /// Occurs when deactivating. Users are expected to stop listening to this session and throw away references.
        /// </summary>
        public event Action Deactivating;
    }
}
