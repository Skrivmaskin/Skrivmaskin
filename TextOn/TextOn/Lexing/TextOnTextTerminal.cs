using System;
using System.Linq;
using System.Text.RegularExpressions;
using Irony.Parsing;
using TextOn.Interfaces;

namespace TextOn.Lexing
{
    public sealed class TextOnTextTerminal : RegexBasedTerminal
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
                    lexerSyntax.VariableStartDelimiter,
                    lexerSyntax.EscapeCharacter
            };
            var specialCharactersInSquare = specialCharacters.Distinct ().Select ((c) => SpecialCharacters.GetEscapeForRegexInSquareBracket (c));
            var notSpecialCharacterRegex = String.Join ("", specialCharactersInSquare.Select ((x) => "^" + x));
            if (!whitespaceAllowed)
                notSpecialCharacterRegex = notSpecialCharacterRegex + @"\s";
            return "[" + notSpecialCharacterRegex + "]+";
        }
        
        public TextOnTextTerminal (string name, ILexerSyntax lexerSyntax, bool whitespaceAllowed)
            : base(name, MakeRegex(lexerSyntax, whitespaceAllowed))
        {
            
        }
    }
}
