using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TextOn.Nouns
{
    /// <summary>
    /// Noun. Represents a user variable.
    /// </summary>
    public sealed class Noun
    {
        [JsonConstructor]
        internal Noun (string name, string description, bool acceptsUserValue)
        {
            Name = name;
            Description = description;
            AcceptsUserValue = acceptsUserValue;
        }

        /// <summary>
        /// Gets or sets the name of this Noun.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the description to display for this Noun at generation time.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:TextOn.Nouns.Noun"/> accepts the user inputting
        /// an arbitrary value that is not in the suggestion list.
        /// </summary>
        /// <value><c>true</c> if accepts user value; otherwise, <c>false</c>.</value>
        public bool AcceptsUserValue { get; set; }

        /// <summary>
        /// Gets or sets the suggestions for the value of this Noun.
        /// </summary>
        /// <value>The suggestions.</value>
        public List<NounSuggestion> Suggestions { get; private set; } = new List<NounSuggestion>();
    }
}
