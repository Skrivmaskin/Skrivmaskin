using System;
using System.Diagnostics;
using System.Linq;
using Irony.Parsing;
using Skrivmaskin.Core.Compiler;
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
        readonly ILexerSyntax lexerSyntax;
        readonly Parser parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Skrivmaskin.Core.Parsing.SkrivmaskinParser"/> class.
        /// </summary>
        /// <param name="lexerSyntax">Lexer syntax.</param>
        public SkrivmaskinParser (ILexerSyntax lexerSyntax)
        {
            this.lexerSyntax = lexerSyntax;
            parser = new Parser (new SkrivmaskinGrammar (lexerSyntax));
        }

        public ParseTree ParseToTree (string s)
        {
            return parser.Parse (s);
        }

        /// <summary>
        /// Parse a design time node into a compiled node.
        /// </summary>
        public ICompiledNode Compile (TextNode designNode)
        {
            var parseTree = ParseToTree (designNode.Text);
            if (parseTree.HasErrors ()) {
                //TODO Pass back some info about the errors
                return new ErrorCompiledNode (designNode, 1, designNode.Text.Length);
            }
            return ConvertSentence (designNode, parseTree.Root);
        }

        private ICompiledNode ConvertSentence (INode designNode, ParseTreeNode node)
        {
            SkrivmaskinTokens token;
            if (Enum.TryParse (node.Term.Name, out token)) {
                switch (token) {
                case SkrivmaskinTokens.Sentence:
                    if (node.ChildNodes.Count == 0) {
                        return new TextCompiledNode ("", designNode, 1, 1);
                    } else if (node.ChildNodes.Count == 1) {
                        return ConvertAnything (designNode, node.ChildNodes [0]);
                    } else {
                        return new SequentialCompiledNode (node.ChildNodes.Select ((ptn) => ConvertAnything (designNode, ptn)).ToList (), designNode, 1, node.Span.Length);
                    }
                default:
                    throw new ApplicationException (string.Format ("Unexpected token when expected a sentence: {0}", token));
                }
            }
            throw new ApplicationException (string.Format ("Unexpected token: {0}", node.Term.Name));
        }

        private ICompiledNode ConvertAnything (INode designNode, ParseTreeNode node)
        {
            SkrivmaskinTokens token;
            if (!Enum.TryParse (node.Term.Name, out token))
                throw new ApplicationException (string.Format ("Unexpected token: {0}", node.Term.Name));
            if (token != SkrivmaskinTokens.Anything)
                throw new ApplicationException (string.Format ("Unexpected token when expected an anything: {0}", token));
            if (node.ChildNodes.Count != 1) throw new ApplicationException (string.Format ("Unexpected number of child nodes {0}", node.ChildNodes.Count));
            if (Enum.TryParse (node.ChildNodes [0].Term.Name, out token)) {
                switch (token) {
                case SkrivmaskinTokens.Variable:
                    return ConvertVariable (designNode, node.ChildNodes [0]);
                case SkrivmaskinTokens.Phrase:
                    return ConvertPhrase (designNode, node.ChildNodes [0]);
                case SkrivmaskinTokens.MultiChoice:
                    return ConvertMultiChoice (designNode, node.ChildNodes [0]);
                default:
                    throw new ApplicationException (string.Format ("Unexpected token: {0}", token));
                }
            }
            throw new ApplicationException (string.Format ("Unexpected token: {0}", token));
        }

        private ICompiledNode ConvertVariable (INode designNode, ParseTreeNode node)
        {
            SkrivmaskinTokens token;
            if (node.ChildNodes.Count != 1) throw new ApplicationException (string.Format ("Unexpected number of children {0}", node.ChildNodes.Count));
            node = node.ChildNodes [0];
            if (!Enum.TryParse (node.Term.Name, out token))
                throw new ApplicationException (string.Format ("Unexpected token: {0}", node.Term.Name));
            switch (token) {
            case SkrivmaskinTokens.SimpleVariable:
                if (node.ChildNodes.Count != 3) throw new ApplicationException (string.Format ("Unexpected number of children {0}", node.ChildNodes.Count));
                return new VariableCompiledNode (node.ChildNodes [1].Token.Text, designNode, node.Span.Location.Position + 1, node.Span.EndPosition);
            case SkrivmaskinTokens.CompoundVariable:
                if (node.ChildNodes.Count != 5) throw new ApplicationException (string.Format ("Unexpected number of children {0}", node.ChildNodes.Count));
                return new VariableCompiledNode (node.ChildNodes [1].Token.Text + lexerSyntax.VariableFormDelimiter.ToString () + node.ChildNodes [3].Token.Text, designNode, node.Span.Location.Position + 1, node.Span.EndPosition);
            default:
                throw new ApplicationException (string.Format ("Unexpected token when expected a variable type: {0}", token));
            }
        }

        private ICompiledNode ConvertPhrase (INode designNode, ParseTreeNode node)
        {
            SkrivmaskinTokens token;
            if (!Enum.TryParse (node.Term.Name, out token))
                throw new ApplicationException (string.Format ("Unexpected token: {0}", node.Term.Name));
            if (token != SkrivmaskinTokens.Phrase)
                throw new ApplicationException (string.Format ("Unexpected token when expected a phrase: {0}", token));
            return new TextCompiledNode (string.Join ("", node.ChildNodes.Select ((cn) => ConvertText (cn))), designNode, node.Span.Location.Position + 1, node.Span.EndPosition);

        }

        private string ConvertText (ParseTreeNode node)
        {
            SkrivmaskinTokens token;
            if (!Enum.TryParse (node.Term.Name, out token))
                throw new ApplicationException (string.Format ("Unexpected token: {0}", node.Term.Name));
            switch (token) {
            case SkrivmaskinTokens.Text:
                return node.Token.Text;
            case SkrivmaskinTokens.Escape:
                return node.Token.Text [1].ToString ();
            case SkrivmaskinTokens.CompoundText:
                return ConvertText (node.ChildNodes [0]);
            default:
                throw new ApplicationException (string.Format ("Unexpected token: {0}", token));
            }
        }

        private ICompiledNode ConvertMultiChoice (INode designNode, ParseTreeNode node)
        {
            if (node.ChildNodes.Count != 3) throw new ApplicationException (string.Format ("Unexpected number of children {0}", node.ChildNodes.Count));
            node = node.ChildNodes [1];
            SkrivmaskinTokens token;
            if (!Enum.TryParse (node.Term.Name, out token))
                throw new ApplicationException (string.Format ("Unexpected token: {0}", node.Term.Name));
            switch (token) {
            case SkrivmaskinTokens.SimpleChoice:
                return ConvertSimpleChoice (designNode, node);
            case SkrivmaskinTokens.Choice:
                return new ChoiceCompiledNode (node.ChildNodes.Select ((cn) => ConvertSimpleChoice (designNode, cn)).ToList(), designNode, node.Span.Location.Position + 1, node.Span.EndPosition);
            default:
                throw new ApplicationException (string.Format ("Unexpected token: {0}", token));
            }
        }

        private ICompiledNode ConvertSimpleChoice (INode designNode, ParseTreeNode node)
        {
            if (node.ChildNodes.Count == 0) return new TextCompiledNode ("", designNode, node.Span.Location.Position + 1, node.Span.EndPosition);
            if (node.ChildNodes.Count == 1) return ConvertAnything (designNode, node.ChildNodes [0]);
            return new SequentialCompiledNode (node.ChildNodes.Select ((cn) => ConvertAnything (designNode, cn)).ToList (), designNode, node.Span.Location.Position + 1, node.Span.EndPosition);
        }
    }
}

