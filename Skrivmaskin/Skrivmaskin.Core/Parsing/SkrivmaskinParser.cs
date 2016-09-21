using System;
using Irony.Parsing;
using Skrivmaskin.Core.Compiled;
using Skrivmaskin.Core.Design;
using Skrivmaskin.Core.Interfaces;
using Skrivmaskin.Core.Lexing;

namespace Skrivmaskin.Core.Parsing
{
    /// <summary>
    /// Responsible for parsing a single line of design time text into compiled node format.
    /// </summary>
    public sealed class SkrivmaskinParser
    {
        readonly Parser parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Skrivmaskin.Core.Parsing.SkrivmaskinParser"/> class.
        /// </summary>
        /// <param name="lexerSyntax">Lexer syntax.</param>
        public SkrivmaskinParser (ILexerSyntax lexerSyntax)
        {
            parser = new Parser (new SkrivmaskinGrammar (lexerSyntax));
        }

        public ParseTree ParseToTree (string s)
        {
            return parser.Parse (s);
        }

        /// <summary>
        /// Parse a design time node into a compiled node.
        /// </summary>
        public bool Compile (TextNode designNode, out ICompiledNode compiledNode, out int firstErrorChar, out string firstErrorMessage)
        {
            var parseTree = ParseToTree (designNode.Text);
            if (parseTree.HasErrors ()) {
                compiledNode = null;
                throw new NotImplementedException ();
            }
            firstErrorChar = 0;
            firstErrorMessage = "";
            throw new NotImplementedException ();
        }
    }
}
