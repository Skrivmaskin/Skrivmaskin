using System;
using System.Linq;
using System.Text.RegularExpressions;
using Irony.Parsing;
using Skrivmaskin.Interfaces;

namespace Skrivmaskin.Lexing
{
    public sealed class SkrivmaskinTextTerminal : RegexBasedTerminal
    {
        private static string MakeRegex (ILexerSyntax lexerSyntax, bool whitespaceAllowed)
        {
            var specialCharacters =
                new char []
                {
                    lexerSyntax.ChoiceAlternativeDelimiter,
                    lexerSyntax.ChoiceStartDelimiter,
                    lexerSyntax.ChoiceEndDelimiter,
                    lexerSyntax.VariableEndDelimiter,
                    lexerSyntax.VariableFormDelimiter,
                    lexerSyntax.VariableStartDelimiter,
                    lexerSyntax.EscapeCharacter
            };
            var specialCharactersInSquare = specialCharacters.Distinct ().Select ((c) => SpecialCharacters.GetEscapeForRegexInSquareBracket (c));
            var notSpecialCharacterRegex = String.Join ("", specialCharactersInSquare.Select ((x) => "^" + x));
            if (!whitespaceAllowed)
                notSpecialCharacterRegex = notSpecialCharacterRegex + @"\s";
            return "[" + notSpecialCharacterRegex + "]+";
        }
        
        public SkrivmaskinTextTerminal (string name, ILexerSyntax lexerSyntax, bool whitespaceAllowed)
            : base(name, MakeRegex(lexerSyntax, whitespaceAllowed))
        {
            
        }
    }
}
