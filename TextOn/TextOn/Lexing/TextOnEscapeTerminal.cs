using System;
using System.Linq;
using System.Text.RegularExpressions;
using Irony.Parsing;
using TextOn.Interfaces;

namespace TextOn.Lexing
{
    internal sealed class TextOnEscapeTerminal : RegexBasedTerminal
    {
        private static string MakeRegex (ILexerSyntax lexerSyntax)
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
            var escapePlusSpecial = String.Join ("|", specialCharacters.Select ((x) => "(" + SpecialCharacters.GetEscapeForRegexInRoot (lexerSyntax.EscapeCharacter) + SpecialCharacters.GetEscapeForRegexInRoot (x) + ")"));
            return escapePlusSpecial;
        }

        public TextOnEscapeTerminal (string name, ILexerSyntax lexerSyntax)
            : base (name, MakeRegex (lexerSyntax))
        {

        }
    }
}
