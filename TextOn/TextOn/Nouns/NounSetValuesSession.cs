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
        private Dictionary<string, List<NounSuggestion>> currentSuggestions = new Dictionary<string, List<NounSuggestion>> ();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Nouns.NounSetValuesSession"/> class.
        /// </summary>
        /// <remarks>
        /// Edits are not allowed while there is a session open - this is guaranteed by the profile.
        /// </remarks>
        /// <param name="nouns">The noun profile from the design template.</param>
        public NounSetValuesSession (NounProfile nouns)
        {
            var allNouns = nouns.GetAllNouns ();
            this.nouns = allNouns.ToDictionary ((noun) => noun.Name);
            this.nounNames = allNouns.Select ((n) => n.Name).ToArray();

            // We treat anything null or whitespace as an invalid value.
            foreach (var nounName in nounNames) {
                values.Add (nounName, "");
            }
            allValuesAreSet = (nounNames.Length == 0);

            // take a copy of the suggestions at this point - no filters to apply
            foreach (var kvp in this.nouns) {
                var noun = kvp.Value;
                var suggestions = noun.Suggestions;
                var li = new List<NounSuggestion> ();
                this.currentSuggestions.Add (noun.Name, li);
                foreach (var suggestion in suggestions) {
                    var suggestionCopy = new NounSuggestion (suggestion.Value);
                    foreach (var dependency in suggestion.Dependencies) {
                        suggestionCopy.Dependencies.Add (new NounSuggestionDependency (dependency.Name, dependency.Value));
                    }
                    li.Add (suggestionCopy);
                }
            }
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

        private void RecalculateSuggestions ()
        {
            // given all the current inputs for each noun, we filter down the possible
            // suggestions of all other nouns that are either directly dependent on this noun or that this noun
            // depends on directly

        }

        private void FireSuggestionsUpdated (string name)
        {
            SuggestionsUpdated?.Invoke (name);
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
            if (String.IsNullOrWhiteSpace (value)) values.Remove (nounName);
            else {
                allValuesAreSet = allValuesAreSet || (!HasAnyUnsetValues ());
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

        /// <summary>
        /// Notifies users of the session that the set of suggestions has changed for this noun.
        /// </summary>
        public event Action<string> SuggestionsUpdated;

        /// <summary>
        /// Occurs when deactivating.
        /// </summary>
        public event Action Deactivating;
    }
}
