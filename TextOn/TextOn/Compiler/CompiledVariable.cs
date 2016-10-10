using System;
using TextOn.Design;
using TextOn.Interfaces;
using TextOn.Version0;

namespace TextOn.Compiler
{
    /// <summary>
    /// Representation of a compiled variable declaration.
    /// </summary>
    internal sealed class CompiledVariable : ICompiledVariable
    {
        readonly ILexerSyntax lexerSyntax;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Compiled.CompiledVariable"/> class.
        /// </summary>
        /// <param name="lexerSyntax">Lexer syntax.</param>
        public CompiledVariable (ILexerSyntax lexerSyntax, Variable definition, VariableForm form)
        {
            this.lexerSyntax = lexerSyntax;
            Name = definition.Name;
            FormName = form.Name;
            Description = definition.Description;
            Suggestion = form.Suggestion;
        }

        /// <summary>
        /// The root name of this variable.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; private set; }

        /// <summary>
        /// The user's form of this variable. This represents a particular variant of this variable, and can be used to handle language specific
        /// variations, or stylistic variations (use of pronouns) or other complexities that only the user can get right.
        /// </summary>
        /// <remarks>
        /// Every variable will have one entry with the empty string here. This is the root form of this variable.
        /// </remarks>
        /// <value>The name of the form.</value>
        public string FormName { get; private set; }

        /// <summary>
        /// The user's description of this variable. This is stored in order to provide it back to the user when the user defines the valu        /// at run time.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; private set; }

        /// <summary>
        /// The user's sample suggestion for this variable value.
        /// </summary>
        /// <value>The suggestion.</value>
        public string Suggestion { get; private set; }

        /// <summary>
        /// The full name of this variable, including form (if necessary).
        /// </summary>
        /// <value>The full name.</value>
        public string FullName { get { return ((FormName == "") ? Name : (Name + lexerSyntax.VariableFormDelimiter + FormName)); } }

        /// <summary>
        /// Determines whether the specified <see cref="ICompiledVariable"/> is equal to the
        /// current <see cref="T:TextOn.Compiled.CompiledVariable"/>.
        /// </summary>
        /// <param name="other">The <see cref="ICompiledVariable"/> to compare with the current <see cref="T:TextOn.Compiled.CompiledVariable"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="ICompiledVariable"/> is equal to the
        /// current <see cref="T:CompiledVariable"/>; otherwise, <c>false</c>.</returns>
        public bool Equals (ICompiledVariable other)
        {
            return FullName == other.FullName;
        }
    }
}
