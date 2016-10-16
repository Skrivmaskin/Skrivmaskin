using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TextOn.Nouns
{
    /// <summary>
    /// Manages the set of <see cref="Noun"/> objects and ensures that circular references and duplicate values cannot be added.
    /// </summary>
    /// <remarks>
    /// Always deal with this object rather than with the <see cref="Noun"/> objects directly when designing <see cref="Noun"/> and
    /// <see cref="NounSuggestion"/> relationships, to ensure that no clashes or circular dependencies are introduced.
    /// </remarks>
    public sealed class NounProfile
    {
        private readonly Dictionary<string, Noun> nouns = new Dictionary<string, Noun>();
        private readonly Dictionary<string, HashSet<string>> globalDependencies = new Dictionary<string, HashSet<string>> ();
        private readonly List<string> nounsInOrder = new List<string> ();

        /// <summary>
        /// Gets the nouns.
        /// </summary>
        /// <value>The nouns.</value>
        public IEnumerable<Noun> GetAllNouns ()
        {
            return nounsInOrder.Select ((n) => nouns [n]);
        }

        /// <summary>
        /// Called by the <see cref="TextOn.Design.TemplateWriter"/> to set up.
        /// </summary>
        /// <param name="value">Value.</param>
        internal void SetAllNouns (IEnumerable<Noun> value)
        {
            // do this quickly, then rebuild
            foreach (var noun in value) {
                nouns.Add (noun.Name, noun);
                nounsInOrder.Add (noun.Name);
                globalDependencies.Add (noun.Name, new HashSet<string> ());
            }
            RebuildGlobalDependencies ();
        }

        /// <summary>
        /// Determines if a name for a new <see cref="Noun"/> is valid for addition
        /// </summary>
        /// <returns><c>true</c>, if a valid name for a new <see cref="Noun"/>  was given, <c>false</c> otherwise.</returns>
        /// <param name="name">Name.</param>
        public bool IsValidNewNounName (string name)
        {
            return !nouns.ContainsKey (name);
        }

        /// <summary>
        /// Adds a new Noun.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        /// <param name="acceptsUserValue">If set to <c>true</c> accepts user value.</param>
        public void AddNewNoun (string name, string description, bool acceptsUserValue)
        {
            var noun = new Noun (name, description, acceptsUserValue);
            nouns.Add (name, noun);
            nounsInOrder.Add (name);

            // No need to rebuild, this Noun is currently independent.
            globalDependencies.Add (name, new HashSet<string> ());
        }

        /// <summary>
        /// Remove a Noun from the profile.
        /// </summary>
        /// <remarks>
        /// This is pretty extreme - make sure your users don't do this without their eyes open. This cleans up a lot.
        /// </remarks>
        /// <param name="name">Name.</param>
        public void DeleteNoun (string name)
        {
            nouns.Remove (name); // this removes the suggestions too
            nounsInOrder.Remove (name);
            globalDependencies.Remove (name);

            // for every variable with a dependency on me, try to find any suggestions that reference me and clean up
            foreach (var item in globalDependencies) {
                if (item.Value.Contains (name)) {
                    foreach (var suggestion in nouns [item.Key].Suggestions) {
                        int i = 0;
                        while (i < suggestion.Dependencies.Count) {
                            if (suggestion.Dependencies [i].Name == name) {
                                suggestion.Dependencies.RemoveAt (i);
                                break;
                            }
                            ++i;
                        }
                    }
                }
            }

            // finally, rebuild, since some transitive dependencies, through me might have gone away
            RebuildGlobalDependencies ();
        }

        [JsonIgnore]
        /// <summary>
        /// Number of Nouns.
        /// </summary>
        /// <value>The count.</value>
        public int Count { get { return nouns.Count; } }

        /// <summary>
        /// Gets the allowed new dependencies, that won't create clashes or circular references.
        /// </summary>
        /// <returns>The allowed new dependencies.</returns>
        /// <param name="name">Name.</param>
        public IEnumerable<string> GetAllowedNewDependencies (string name)
        {
            var currentDependencies = globalDependencies [name];
            return GetAllNouns ().Where ((n) => n.Name != name && !currentDependencies.Contains (n.Name) && !globalDependencies [n.Name].Contains (name)).Select ((n) => n.Name);
        }

        /// <summary>
        /// Gets the existing dependencies from the given Noun.
        /// </summary>
        /// <returns>The existing dependencies.</returns>
        /// <param name="name">Name.</param>
        public IEnumerable<string> GetExistingDependencies (string name)
        {
            // why filter the list? it's kinder to the user to see these in dependency order.
            var thisDependencies = globalDependencies [name];
            return nounsInOrder.Where ((n) => thisDependencies.Contains (n));
        }

        private void AddGlobalDependency (string fromName, string onName) {
            globalDependencies [fromName].Add (onName);
            var foundFromName = false;
            var i = 0;
            while (i < nounsInOrder.Count) {
                var nounName = nounsInOrder [i];
                if (nounName == fromName) {
                    foundFromName = true;
                    nounsInOrder.RemoveAt (i);
                } else if (nounName == onName) {
                    if (foundFromName) {
                        nounsInOrder.Insert (i + 1, fromName);
                        break;
                    } else
                        break;
                } else {
                    ++i;
                }
            }
        }

        /// <summary>
        /// Rebuilds the global dependencies, including transitives
        /// </summary>
        private void RebuildGlobalDependencies()
        {
            foreach (var kvp in globalDependencies) {
                kvp.Value.Clear ();
            }

            // Find all the direct dependencies.
            foreach (var kvp in nouns) {
                foreach (var suggestion in kvp.Value.Suggestions) {
                    foreach (var dependency in suggestion.Dependencies) {
                        if (!globalDependencies [kvp.Key].Contains (dependency.Name))
                            AddGlobalDependency (kvp.Key, dependency.Name);
                    }
                }
            }

            // Find all the transitive dependencies one at a time, until there are none left.
            Tuple<string, string> toAdd = null;
            do {
                if (toAdd != null) {
                    AddGlobalDependency (toAdd.Item1, toAdd.Item2);
                    toAdd = null;
                }

                foreach (var fromKvp in globalDependencies) {
                    var fromNoun = fromKvp.Key;
                    var fromDependencies = fromKvp.Value;
                    foreach (var directNoun in fromDependencies) {
                        foreach (var transitiveNoun in globalDependencies [directNoun]) {
                            if (!fromDependencies.Contains (transitiveNoun)) {
                                toAdd = new Tuple<string, string> (fromNoun, transitiveNoun);
                                break;
                            }
                        }
                    }
                    if (toAdd != null) break;
                }
            } while (toAdd != null);
        }

        /// <summary>
        /// Adds the suggestion.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        /// <param name="dependencies">Dependencies.</param>
        public void AddSuggestion (string name, string value, IEnumerable<NounSuggestionDependency> dependencies)
        {
            var suggestion = new NounSuggestion (value);
            var noun = nouns [name];
            var globalDependenciesForThisNoun = globalDependencies [name];
            suggestion.Dependencies.AddRange (dependencies);
            noun.Suggestions.Add (suggestion);
            if (suggestion.Dependencies.Count == 0) return; // can't affect dependencies
            RebuildGlobalDependencies ();
            SuggestionsChangedForNoun?.Invoke (name);
        }

        /// <summary>
        /// Deletes the suggestion.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        public void DeleteSuggestion (string name, string value)
        {
            var noun = nouns [name];
            var suggestionToDelete = noun.Suggestions.Find ((s) => s.Value == value);
            if (!noun.Suggestions.Remove (suggestionToDelete))
                throw new ApplicationException ("Unable to remove suggestion [" + name + ", " + value + "]");
            if (suggestionToDelete.Dependencies.Count > 0)
                RebuildGlobalDependencies ();
            SuggestionsChangedForNoun?.Invoke (name);
        }

        /// <summary>
        /// Makes a new set values session.
        /// </summary>
        /// <remarks>
        /// This session will only be aware of the nouns that are known at creation time. It is expected that
        /// users will only ever have one at a time, and will fully deactivate and throw it away before creating a
        /// new one to pick up edits.
        /// </remarks>
        /// <returns>The set values session.</returns>
        public NounSetValuesSession MakeSetValuesSession ()
        {
            return new NounSetValuesSession (this);
        }

        public Noun GetNounByIndex (int index)
        {
            return nouns [nounsInOrder [index]];
        }

        public Noun GetNounByName (string name)
        {
            return nouns [name];
        }

        /// <summary>
        /// Sets the dependencies for a suggestion.
        /// </summary>
        /// <param name="nounName">Noun name.</param>
        /// <param name="suggestionValue">Suggestion value.</param>
        /// <param name="newDependencies">New dependencies.</param>
        public void SetDependenciesForSuggestion (string nounName, string suggestionValue, IEnumerable<NounSuggestionDependency> newDependencies)
        {
            var noun = nouns [nounName];
            var suggestionToChange = noun.Suggestions.Find ((s) => s.Value == suggestionValue);
            suggestionToChange.Dependencies = new List<NounSuggestionDependency> (newDependencies);
            RebuildGlobalDependencies ();
            SuggestionsChangedForNoun?.Invoke (nounName);
        }

        public event Action<string> SuggestionsChangedForNoun;
    }
}
