using System;
namespace Skrivmaskin.Core.Lexing
{
    /// <summary>
    /// Describes rules for recognising tokens.
    /// </summary>
    internal static class Syntax
    {
        public static bool IsVariableStartDelimiter (char c, Func<char> nc)
        {
            return ((c == '[') && (nc () == '['));
        }

        public static bool IsVariableEndDelimiter (char c, Func<char> nc)
        {
            return ((c == ']') && (nc () == ']'));
        }

        public static bool IsMultipleChoiceStartDelimiter (char c, Func<char> nc)
        {
            return ((c == '(') && (nc () == '('));
        }

        public static bool IsMultipleChoiceEndDelimiter (char c, Func<char> nc)
        {
            return ((c == ')') && (nc () == ')'));
        }

        public static bool IsMultipleChoiceAlternativeDelimiter (char c, Func<char> nc)
        {
            return ((c == '/') && (nc () == '/'));
        }

        public static bool IsValidVariableNameCharacter (char c)
        {
            return Char.IsLetterOrDigit (c); // hope this is cool with diacritics
        }
    }
}
