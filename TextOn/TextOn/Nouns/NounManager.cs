using System;
using System.Linq;
using System.Collections.Generic;

namespace TextOn.Nouns
{
    /// <summary>
    /// Manages the set of <see cref="Noun"/> objects and ensures that circular references and duplicate values cannot be added.
    /// </summary>
    /// <remarks>
    /// Always deal with this object rather than with the <see cref="Noun"/> objects directly when designing <see cref="Noun"/> and
    /// <see cref="NounSuggestion"/> relationships, to ensure that no clashes or circular dependencies are introduced.
    /// </remarks>
    public sealed class NounManager
    {
        private readonly Dictionary<string, Noun> nouns = new Dictionary<string, Noun>();
        private readonly List<NounDependency> dependencies = new List<NounDependency> ();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Nouns.NounManager"/> class.
        /// </summary>
        public NounManager ()
        {
        }

        /// <summary>
        /// Gets all defined nouns.
        /// </summary>
        /// <returns>The all nouns.</returns>
        public IEnumerable<Noun> GetAllNouns ()
        {
            return nouns.Select ((kvp) => kvp.Value);
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
        /// Adds a new noun. It is assumed that this noun has been validated using this <see cref="T:TextOn.Nouns.NounManager"/>.
        /// </summary>
        /// <param name="noun">Noun.</param>
        public void AddNewNoun (Noun noun)
        {
            nouns.Add (noun.Name, noun);
        }

        public IEnumerable<Noun> GetAllowedNewDependencies (Noun noun)
        {
            throw new NotImplementedException ();
        }

        public IEnumerable<Noun> GetExistingDependencies (Noun noun)
        {
            throw new NotImplementedException ();
        }


    }
}
