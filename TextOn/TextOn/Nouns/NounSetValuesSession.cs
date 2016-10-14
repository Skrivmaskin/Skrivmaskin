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
        private readonly NounProfile nounProfile;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Nouns.NounSetValuesSession"/> class.
        /// </summary>
        /// <remarks>
        /// Edits are not allowed while there is a session open - this is guaranteed by the profile.
        /// </remarks>
        /// <param name="nouns">The noun profile from the design template.</param>
        internal NounSetValuesSession (NounProfile nouns)
        {
            nounProfile = nouns;
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

        private void CopySuggestionsForNoun (string nounName)
        {
            this.currentSuggestions.Remove (nounName);
            var noun = nouns [nounName];
            var li = new List<NounSuggestion> ();
            // Careful - taking actual references here - but we're only going to filter them and reset, no edits allowed.
            li.AddRange (noun.Suggestions);
            this.currentSuggestions.Add (nounName, li);
        }

        private void CopySuggestions ()
        {
            // take a copy of the suggestions at this point - no filters to apply
            foreach (var nounName in nounNames) {
                CopySuggestionsForNoun (nounName);
            }
        }

        private void RecalculateSuggestions (string changedNounName)
        {
            var dependentNounNames =
                nounNames
                    .Where ((dnn) => nounProfile.GetExistingDependencies (dnn).Contains (changedNounName));

            foreach (var dependentNounName in dependentNounNames) {
                CopySuggestionsForNoun (dependentNounName);

                foreach (var dep in nounProfile.GetExistingDependencies (dependentNounName)) {
                    var depValue = values [dep];
                    if (!String.IsNullOrWhiteSpace (depValue)) {
                        currentSuggestions [dependentNounName] =
                            currentSuggestions [dependentNounName]
                                .Where ((suggestion) => HasNoInvalidSuggestions (dep, depValue, suggestion.Dependencies));
                    }
                }
                //TODO notify users for a non-AcceptUserValue dependent variable that their value stinks
                FireSuggestionsUpdated (dependentNounName);
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

        bool settingValue = false;
        Queue<Tuple<string, string>> remainingToSet = new Queue<Tuple<string, string>>();

        /// <summary>
        /// Set the value for a Noun.
        /// </summary>
        /// <remarks>
        /// This queues up inputs because users may recurse in to set new values after suggestions change on dependencies. The user
        /// should not recurse back in for this or a higher precedence Noun.
        /// </remarks>
        /// <param name="_nounName">Noun name.</param>
        /// <param name="_value">Value.</param>
        public void SetValue (string _nounName, string _value)
        {
            remainingToSet.Enqueue (new Tuple<string, string> (_nounName, _value));
            if (settingValue) return;

            while (remainingToSet.Count > 0) {
                var data = remainingToSet.Dequeue ();
                var nounName = data.Item1;
                var value = data.Item2;
                settingValue = true;
                values [nounName] = value;
                var newAllValuesAreSet = allValuesAreSet || (!HasAnyUnsetValues ());
                if (allValuesAreSet != newAllValuesAreSet) {
                    allValuesAreSet = newAllValuesAreSet;
                    AllValuesAreSetUpdated?.Invoke (allValuesAreSet);
                }
                RecalculateSuggestions (nounName);
                settingValue = false;
            }
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
