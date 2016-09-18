using System;
namespace Skrivmaskin.Core
{
    /// <summary>
    /// Represents a variable substitution.
    /// </summary>
    public sealed class PhraseVariableSubstitution : IBlockOfText
    {
        /// <summary>
        /// The specific form of the variable needed for this substitution.
        /// </summary>
        /// <value>The variable.</value>
        public VariableForm Variable { get; set; }
    }
}
