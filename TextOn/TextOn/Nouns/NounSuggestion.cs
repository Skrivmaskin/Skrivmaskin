using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TextOn.Nouns
{
    /// <summary>
    /// Suggestion for a given <see cref="Noun"/>, including dependencies on other names.
    /// </summary>
    public sealed class NounSuggestion
    {
        [JsonConstructor]
        internal NounSuggestion (string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets or sets the value for this suggestion.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the dependencies on other <see cref="Noun"/> values.
        /// </summary>
        /// <value>The dependencies.</value>
        public List<NounSuggestionDependency> Dependencies { get; set; } = new List<NounSuggestionDependency> ();
    }
}
