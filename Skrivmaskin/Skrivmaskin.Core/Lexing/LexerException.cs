using System;
namespace Skrivmaskin.Core.Lexing
{
    /// <summary>
    /// Raised if the input cannot be fully lexed into valid tokens.
    /// </summary>
    public sealed class LexerException : Exception
    {
        readonly string details;
        public LexerException (string details)
        {
            this.details = details;
        }
        public string Details { get { return details; } }
    }
}
