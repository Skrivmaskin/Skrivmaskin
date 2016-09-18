using System;
using System.Collections.Generic;
namespace Skrivmaskin.Core.Lexing
{

    /*
    /// <summary>
    /// Note conscious design choice not to use tools like lex or Irony, since that seems like overkill for a simple "language" like this.
    /// I hope I don't regret that...
    /// </summary>
    public sealed class Lexer
    {
        readonly char [] characters;
        int index = -1;
        bool isDone = false;
        Token currentToken = null;
        readonly LexerState state;
        
        private Lexer (string s, LexerState state)
        {
            characters = s.ToCharArray ();
            this.state = state;
        }

        private static Token [] TokenizeInner (string s, LexerState state)
        {
            var parser = new Lexer (s, state);
            var li = new List<Token> ();
            while (!parser.isDone) {
                parser.Advance ();
                li.Add (parser.currentToken);
            }
            return li.ToArray ();
        }

        public static Token [] Tokenize (string s)
        {
            return TokenizeInner (s, LexerState.Global);
        }

        private string EatWhile (Func<char, bool> condition)
        {
            var s = "";
            while ((!IsAtEnd) && (condition (Lookahead ()))) {
                s += NextChar ();
            }
            if (IsAtEnd) throw new LexerException ("Hit EOF");
            return s;
        }

        private char Lookahead ()
        {
            return characters [index + 1];
        }

        private bool IsAtEnd { get { return ((index + 1) >= characters.Length); } }

        private char NextChar ()
        {
            throw new NotImplementedException ();
        }

        public void Advance ()
        {
            if (IsAtEnd) {
                isDone = true;
                return;
            }
            char nextChar = NextChar ();
            if (state != LexerState.MultipleChoice && Syntax.IsVariableStartDelimiter (nextChar, Lookahead)) {
                // This token is going to be a variable. Eat the name to get to the end delimiter.
                nextChar = NextChar (); // Eat that second delimiter character.
                var variableName = EatWhile (Syntax.IsValidVariableNameCharacter);
                // Eat the end of the variable delimiter, or throw a LexerException.
                nextChar = NextChar ();
                if (!Syntax.IsVariableEndDelimiter (nextChar, NextChar))
                    throw new LexerException (string.Format ("Unclosed variable {0} at index {1}", variableName, index - variableName.Length));
                currentToken = new Token (TokenType.VariableName, variableName, new Token [0]);
            } else if (state == LexerState.Global && Syntax.IsMultipleChoiceStartDelimiter (nextChar, Lookahead)) {
                // This token is going to be a multiple choice.
                // Eat the innards and tokenize that.
                nextChar = NextChar (); // Eat that second delimiter character.
                // Note: this means recursive multiple choices are not ok. I am fine with that.
                // Note: variables inside multiple choices are totally cool. Apparently.
                var inner = EatWhile ((c) => !Syntax.IsMultipleChoiceEndDelimiter (c, Lookahead));
                var subTokens = TokenizeInner (inner, LexerState.MultipleChoice);
                var token = new Token (TokenType.MultipleChoice, inner, subTokens);

            } else if (Syntax.IsCharOrdinalStart (nextChar)) {

                char marker = NextChar ();
                if (marker == '\\') {
                    string code = EatWhile (Syntax.IsNumber);
                    if (code.Length != 3) {
                        throw new CompilationException (
                        "Expected: \\nnn where n are decimal digits",
                        _currentLine);
                    }
                    int value = int.Parse (code);
                    if (value >= 256) {
                        throw new CompilationException (
                        "Character ordinal is out of range [0,255]",
                        _currentLine);
                    }
                    _currentToken = new Token (
                        TokenType.IntConst, value.ToString ());
                } else {
                    _currentToken = new Token (
                        TokenType.IntConst, ((int)marker).ToString ());
                }
                NextChar ();//Swallow the end of the character ordinal
            } else if (Syntax.IsStringConstantStart (nextChar)) {
                //This token is going to be a string constant.
                string strConst = EatWhile (
                    c => !Syntax.IsStringConstantStart (c));
                NextChar ();//Swallow the end of the string constant
                _currentToken = new Token (
                    TokenType.StrConst, strConst);
            } else if (Syntax.IsStartOfKeywordOrIdent (nextChar)) {

                string keywordOrIdent = nextChar.ToString ();
                keywordOrIdent += EatWhile (
                                  Syntax.IsPartOfKeywordOrIdent);
                if (Syntax.IsKeyword (keywordOrIdent)) {
                    _currentToken = new Token (
                        TokenType.Keyword, keywordOrIdent);
                } else {
                    _currentToken = new Token (
                        TokenType.Ident, keywordOrIdent);
                }
            } else {
                throw new CompilationException (
                    "Unexpected character: " + nextChar, _currentLine);
            }

        }
    }*/
}
