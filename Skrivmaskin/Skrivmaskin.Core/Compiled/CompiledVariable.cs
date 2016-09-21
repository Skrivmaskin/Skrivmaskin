using System;
using Skrivmaskin.Core.Interfaces;

namespace Skrivmaskin.Core.Compiled
{
    /// <summary>
    /// Representation of a compiled variable declaration.
    /// </summary>
    public sealed class CompiledVariable
    {
        readonly ILexerSyntax lexerSyntax;

        public CompiledVariable (ILexerSyntax lexerSyntax)
        {
            this.lexerSyntax = lexerSyntax;
        }

        /// <summary>
        /// The root name of this variable.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// The user's form of this variable. This represents a particular variant of this variable, and can be used to handle language specific
        /// variations, or stylistic variations (use of pronouns) or other complexities that only the user can get right.
        /// </summary>
        /// <remarks>
        /// Every variable will have one entry with the empty string here. This is the root form of this variable.
        /// </remarks>
        /// <value>The name of the form.</value>
        public string FormName { get; set; }

        /// <summary>
        /// The user's description of this variable. This is stored in order to provide it back to the user when the user defines the value
        /// at run time.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// The user's sample suggestion for this variable value.
        /// </summary>
        /// <value>The suggestion.</value>
        public string Suggestion { get; set; }
        public string FullName { get { return ((FormName == "") ? Name : (Name + lexerSyntax.VariableFormDelimiter + FormName)); } }
    }
}
