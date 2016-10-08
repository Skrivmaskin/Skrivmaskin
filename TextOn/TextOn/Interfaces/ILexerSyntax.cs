using System;
namespace TextOn.Interfaces
{
    /// <summary>
    /// Provides the syntax used by the lexer to convert one line of input text into a design time replacement tree.
    /// </summary>
    /// <remarks>
    /// Provided as an interface so that users may customize their tokens, however beware that customizing this will make templates unshareable
    /// obviously.
    /// </remarks>
    public interface ILexerSyntax
    {
        /// <summary>
        /// Delimits the start of a variable.
        /// </summary>
        char VariableStartDelimiter { get; }

        /// <summary>
        /// Delimits the end of a variable.
        /// </summary>
        char VariableEndDelimiter { get; }

        /// <summary>
        /// Delimits the variable name from its form.
        /// </summary>
        char VariableFormDelimiter { get; }

        /// <summary>
        /// Delimits the beginning of a multiple choice.
        /// </summary>
        char ChoiceStartDelimiter { get; }

        /// <summary>
        /// Delimits the end of a multiple choice.
        /// </summary>
        char ChoiceEndDelimiter { get; }

        /// <summary>
        /// Delimits alternatives within a multiple choice.
        /// </summary>
        char ChoiceAlternativeDelimiter { get; }

        /// <summary>
        /// Escape character.
        /// </summary>
        /// <value>The escape character.</value>
        char EscapeCharacter { get; }
    }
}
