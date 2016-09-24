using System;
using System.Linq;
using System.Text.RegularExpressions;
using Irony.Parsing;
using Skrivmaskin.Interfaces;

namespace Skrivmaskin.Lexing
{
    public sealed class SkrivmaskinEscapeTerminal : RegexBasedTerminal
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
                    lexerSyntax.VariableFormDelimiter,
                    lexerSyntax.VariableStartDelimiter,
                    lexerSyntax.EscapeCharacter
            };
            var escapePlusSpecial = String.Join ("|", specialCharacters.Select ((x) => "(" + SpecialCharacters.GetEscapeForRegexInRoot (lexerSyntax.EscapeCharacter) + SpecialCharacters.GetEscapeForRegexInRoot (x) + ")"));
            return escapePlusSpecial;
        }

        public SkrivmaskinEscapeTerminal (string name, ILexerSyntax lexerSyntax)
            : base (name, MakeRegex (lexerSyntax))
        {

        }
    }
}
