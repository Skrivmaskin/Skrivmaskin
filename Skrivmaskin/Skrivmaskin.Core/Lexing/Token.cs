using System;
using System.Collections.Generic;
namespace Skrivmaskin.Core.Lexing
{
    /// <summary>
    /// Token.
    /// </summary>
    public sealed class Token
    {
        public TokenType TokenType { get; private set; }
        public string Data { get; private set; }
        public Token [] SubTokens { get; private set; }
        public Token (TokenType tokenType, string data, Token[] subTokens)
        {
            TokenType = tokenType;
            Data = data;
            SubTokens = subTokens;
        }
    }
}
