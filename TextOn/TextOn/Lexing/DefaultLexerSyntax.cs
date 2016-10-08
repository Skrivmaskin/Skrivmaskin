using System;
using TextOn.Interfaces;

namespace TextOn.Lexing
{
    /// <summary>
    /// The default lexer syntax.
    /// </summary>
    public sealed class DefaultLexerSyntax : ILexerSyntax
    {
        /// <summary>
        /// Gets the choice alternative delimiter.
        /// </summary>
        /// <value>The choice alternative delimiter.</value>
        public char ChoiceAlternativeDelimiter {
            get {
                return '|';
            }
        }

        /// <summary>
        /// Gets the choice end delimiter.
        /// </summary>
        /// <value>The choice end delimiter.</value>
        public char ChoiceEndDelimiter {
            get {
                return '}';
            }
        }

        /// <summary>
        /// Gets the choice start delimiter.
        /// </summary>
        /// <value>The choice start delimiter.</value>
        public char ChoiceStartDelimiter {
            get {
                return '{';
            }
        }

        /// <summary>
        /// Gets the variable end delimiter.
        /// </summary>
        /// <value>The variable end delimiter.</value>
        public char VariableEndDelimiter {
            get {
                return ']';
            }
        }

        /// <summary>
        /// Gets the variable form delimiter.
        /// </summary>
        /// <value>The variable form delimiter.</value>
        public char VariableFormDelimiter {
            get {
                return '|';
            }
        }

        /// <summary>
        /// Gets the variable start delimiter.
        /// </summary>
        /// <value>The variable start delimiter.</value>
        public char VariableStartDelimiter {
            get {
                return '[';
            }
        }

        /// <summary>
        /// Escape character.
        /// </summary>
        /// <value>The escape character.</value>
        public char EscapeCharacter { get { return '\\';}}
    }
}
