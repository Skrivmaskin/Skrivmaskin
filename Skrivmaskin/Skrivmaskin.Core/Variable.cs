using System;
namespace Skrivmaskin.Core
{
    /// <summary>
    /// Represents a variable name.
    /// </summary>
    /// <remarks>
    /// A document may have 0 or more variables associated with it. These can then be reused in other language elements.
    /// </remarks>
    public sealed class Variable
    {
        /// <summary>
        /// User name for this variable.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Optional user description for this variable.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Suggested value for this variable.
        /// </summary>
        /// <value>The suggestion.</value>
        public string Suggestion { get; set; }

        /// <summary>
        /// The grammatical forms in which this variable may be used.
        /// </summary>
        /// <value>The forms.</value>
        public VariableForm [] Forms { get; set; }
    }
}
