using System;
using System.Collections.Generic;
namespace TextOn.Lexing
{
    /// <summary>
    /// All the special characters that are available to use in the single line lexing process.
    /// </summary>
    /// <remarks>
    /// This static class also provides utilities for converting characters into .NET regex format in various contexts.
    /// </remarks>
    internal static class SpecialCharacters
    {
        /// <summary>
        /// All of the special characters that are available for use.
        /// </summary>
        public static readonly IEnumerable<char> All = MakeAll ();
        private static IEnumerable<char> MakeAll ()
        {
            return new char [] { '$', ']', '[', '{', '}', '(', ')', '%', '\\', '/', '|', ':' };
        }

        private static readonly IReadOnlyDictionary<char, string> EscapesForRegexInSquareBracket = MakeEscapesForRegexInSquareBracket ();
        private static IReadOnlyDictionary<char, string> MakeEscapesForRegexInSquareBracket ()
        {
            var d = new Dictionary<char, string> ();
            d.Add ('$', @"\$");
            d.Add (']', @"\]");
            d.Add ('[', @"\[");
            d.Add ('{', @"{");
            d.Add ('}', @"}");
            d.Add ('(', @"(");
            d.Add (')', @")");
            d.Add ('%', @"%");
            d.Add ('\\', @"\\");
            d.Add ('/', @"/");
            d.Add ('|', @"|");
            d.Add (':', @":");
            return d;
        }

        /// <summary>
        /// Gets the escape for regex when the character is used in a square bracketed expression.
        /// </summary>
        /// <remarks>
        /// The requested character must be included in <see cref="All"/>. 
        /// </remarks>
        /// <exception cref="KeyNotFoundException">
        /// <returns>The escape for regex in square bracket.</returns>
        /// <param name="c">The character.</param>
        public static string GetEscapeForRegexInSquareBracket (char c)
        {
            return EscapesForRegexInSquareBracket [c];
        }

        private static readonly IReadOnlyDictionary<char, string> EscapesForRegexInRoot = MakeEscapesForRegexInRoot ();
        private static IReadOnlyDictionary<char, string> MakeEscapesForRegexInRoot ()
        {
            var d = new Dictionary<char, string> ();
            d.Add ('$', @"\$");
            d.Add (']', @"\]");
            d.Add ('[', @"\[");
            d.Add ('{', @"\{");
            d.Add ('}', @"\}");
            d.Add ('(', @"\(");
            d.Add (')', @"\)");
            d.Add ('%', @"%");
            d.Add ('\\', @"\\");
            d.Add ('/', @"/");
            d.Add ('|', @"\|");
            d.Add (':', @":");
            return d;
        }

        /// <summary>
        /// Gets the escape for regex when the character is used in the root.
        /// </summary>
        /// <remarks>
        /// The requested character must be included in <see cref="All"/>. 
        /// </remarks>
        /// <exception cref="KeyNotFoundException">
        /// <returns>The escape for regex in root.</returns>
        /// <param name="c">The character.</param>
        public static string GetEscapeForRegexInRoot (char c)
        {
            return EscapesForRegexInRoot [c];
        }
    }
}
