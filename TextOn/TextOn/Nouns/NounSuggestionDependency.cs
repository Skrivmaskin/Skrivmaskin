namespace TextOn.Nouns
{
    /// <summary>
    /// Dependency from a <see cref="NounSuggestion"/> on the value of another 
    /// </summary>
	public sealed class NounSuggestionDependency
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Nouns.NounSuggestionDependency"/> class.
        /// </summary>
        public NounSuggestionDependency () : this ("", "")
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Nouns.NounSuggestionDependency"/> class.
        /// </summary>
        /// <param name="name">Name.</param>
        /// <param name="value">Value.</param>
        public NounSuggestionDependency (string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value { get; set; }
	}
}