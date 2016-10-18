using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;
using TextOn.Compiler;
using TextOn.Design;
using TextOn.Interfaces;
using TextOn.Lexing;

namespace TextOn.Parsing
{
    /// <summary>
    /// Responsible for parsing a single line of design time text into compiled node format.
    /// </summary>
    internal sealed class TextOnParser
    {
        readonly ILexerSyntax lexerSyntax;
        readonly Parser parser;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Parsing.TextOnParser"/> class.
        /// </summary>
        /// <param name="lexerSyntax">Lexer syntax.</param>
        public TextOnParser (ILexerSyntax lexerSyntax)
        {
            this.lexerSyntax = lexerSyntax;
            parser = new Parser (new TextOnGrammar (lexerSyntax));
        }

        public ParseTree ParseToTree (string s)
        {
            return parser.Parse (s);
        }

        /// <summary>
        /// Parse a design time node into a compiled node.
        /// </summary>
        /// <remarks>
        /// The design node must be a text node by this point.
        /// </remarks>
        public CompiledNode Compile (DesignNode designNode)
        {
            var parseTree = ParseToTree (designNode.Text);
            var elements = MakeElements (designNode.Text.Length, parseTree.Tokens);
            if (parseTree.HasErrors ()) {
                return new CompiledNode (CompiledNodeType.Error, designNode, true, new string[0], "", new CompiledNode[0], elements);
            }
            var sentence = ConvertSentence (designNode, parseTree.Root);
            return new CompiledNode (CompiledNodeType.Success, designNode, false, sentence.RequiredNouns, "", new CompiledNode [1] { sentence }, elements);
        }

        internal IEnumerable<TextOnParseElement> MakeElements (int totalLength, List<Token> tokens)
        {
            var lastCharIndex = totalLength - 1;
            var tokenizedLastCharIndex = -1;
            var elements = new List<TextOnParseElement> ();
            var inVariable = false;
            int choiceDepth = 0;
            for (int i = 0; i < tokens.Count; i++) {
                var token = tokens [i];
                var isKey = token.KeyTerm != null;
                if (token.Category == TokenCategory.Outline) {
                    
                } else if (token.IsError ()) {
                    elements.Add (new TextOnParseElement (TextOnParseTokens.Error, choiceDepth, new TextOnParseRange (token.Location.Position, tokenizedLastCharIndex = token.Location.Position + token.Length - 1)));
                } else if (isKey) {
                    if (inVariable) {
                        if (lexerSyntax.VariableEndDelimiter.ToString () == token.Text) {
                            inVariable = false;
                            elements.Add (new TextOnParseElement (TextOnParseTokens.VarEnd, choiceDepth, new TextOnParseRange (token.Location.Position, tokenizedLastCharIndex = token.Location.Position)));
                        } else {
                            elements.Add (new TextOnParseElement (TextOnParseTokens.InvalidCharacter, choiceDepth, new TextOnParseRange (token.Location.Position, tokenizedLastCharIndex = token.Location.Position)));
                        }
                    } else {
                        if (lexerSyntax.VariableStartDelimiter.ToString () == token.Text) {
                            inVariable = true;
                            elements.Add (new TextOnParseElement (TextOnParseTokens.VarStart, choiceDepth, new TextOnParseRange (token.Location.Position, tokenizedLastCharIndex = token.Location.Position)));
                        } else if (lexerSyntax.ChoiceStartDelimiter.ToString () == token.Text) {
                            elements.Add (new TextOnParseElement (TextOnParseTokens.ChoiceStart, choiceDepth++, new TextOnParseRange (token.Location.Position, tokenizedLastCharIndex = token.Location.Position)));
                        } else if (lexerSyntax.ChoiceAlternativeDelimiter.ToString () == token.Text) {
                            elements.Add (new TextOnParseElement (TextOnParseTokens.ChoiceDivide, choiceDepth, new TextOnParseRange (token.Location.Position, tokenizedLastCharIndex = token.Location.Position)));
                        } else if (lexerSyntax.ChoiceEndDelimiter.ToString () == token.Text) {
                            elements.Add (new TextOnParseElement (TextOnParseTokens.ChoiceEnd, --choiceDepth, new TextOnParseRange (token.Location.Position, tokenizedLastCharIndex = token.Location.Position)));
                        } else {
                            elements.Add (new TextOnParseElement (TextOnParseTokens.InvalidCharacter, choiceDepth, new TextOnParseRange (token.Location.Position, tokenizedLastCharIndex = token.Location.Position)));
                        }
                    }
                } else {
                    TextOnParseNodes terminalType;
                    if (Enum.TryParse<TextOnParseNodes> (token.Terminal.OutputTerminal.Name, out terminalType)) {
                        switch (terminalType) {
                        case TextOnParseNodes.Text:
                        case TextOnParseNodes.Escape:
                            if (inVariable)
                            {
                                elements.Add (new TextOnParseElement (TextOnParseTokens.InvalidText, choiceDepth, new TextOnParseRange (token.Location.Position, tokenizedLastCharIndex = token.Location.Position + token.Text.Length - 1)));
                            }
                            else
                            {
                                elements.Add (new TextOnParseElement (TextOnParseTokens.Text, choiceDepth, new TextOnParseRange (token.Location.Position, tokenizedLastCharIndex = token.Location.Position + token.Text.Length - 1)));
                            }
                            break;
                        case TextOnParseNodes.VarName:
                            elements.Add (new TextOnParseElement (TextOnParseTokens.VarName, choiceDepth, new TextOnParseRange (token.Location.Position, tokenizedLastCharIndex = token.Location.Position + token.Text.Length - 1)));
                            break;
                        default:
                            //TODO info about this failure - this is meant to be an assertion, but is effectively asserting that
                            // Irony is well behaved...
                            throw new ApplicationException ("Didn't match to a known terminal type");
                        }
                    } else {
                        //TODO info about this failure - this is meant to be an assertion, but is effectively asserting that
                        // Irony is well behaved...
                        throw new ApplicationException ("Didn't match to a known terminal type");
                    }
                }
            }
            if (tokenizedLastCharIndex < lastCharIndex) {
                elements.Add (new TextOnParseElement (TextOnParseTokens.InvalidText, choiceDepth, new TextOnParseRange (tokenizedLastCharIndex + 1, lastCharIndex)));
            }
            return elements;
        }

        private CompiledNode ConvertSentence (DesignNode designNode, ParseTreeNode node)
        {
            TextOnParseNodes token;
            if (Enum.TryParse (node.Term.Name, out token)) {
                switch (token) {
                case TextOnParseNodes.Sentence:
                    if (node.ChildNodes.Count == 0) {
                        return new CompiledNode (CompiledNodeType.Text, designNode, false, new string [0], "", new CompiledNode [0], new TextOnParseElement [0]);
                    } else if (node.ChildNodes.Count == 1) {
                        return ConvertAnything (designNode, node.ChildNodes [0]);
                    } else {
                        var childNodes = node.ChildNodes.Select ((cn) => ConvertAnything (designNode, cn)).ToArray ();
                        var hasErrors = false;
                        HashSet<string> requiredNouns = new HashSet<string> ();
                        foreach (var item in childNodes) {
                            hasErrors = hasErrors || item.HasErrors;
                            foreach (var requiredNoun in item.RequiredNouns) {
                                requiredNouns.Add (requiredNoun);
                            }
                        }
                        return new CompiledNode (CompiledNodeType.Sequential, designNode, hasErrors, requiredNouns, "", childNodes, new TextOnParseElement [0]);
                    }
                default:
                    throw new ApplicationException (string.Format ("Unexpected token when expected a sentence: {0}", token));
                }
            }
            throw new ApplicationException (string.Format ("Unexpected token: {0}", node.Term.Name));
        }

        private CompiledNode ConvertAnything (DesignNode designNode, ParseTreeNode node)
        {
            TextOnParseNodes token;
            if (!Enum.TryParse (node.Term.Name, out token))
                throw new ApplicationException (string.Format ("Unexpected token: {0}", node.Term.Name));
            if (token != TextOnParseNodes.Anything)
                throw new ApplicationException (string.Format ("Unexpected token when expected an anything: {0}", token));
            if (node.ChildNodes.Count != 1) throw new ApplicationException (string.Format ("Unexpected number of child nodes {0}", node.ChildNodes.Count));
            if (Enum.TryParse (node.ChildNodes [0].Term.Name, out token)) {
                switch (token) {
                case TextOnParseNodes.Noun:
                    return ConvertVariable (designNode, node.ChildNodes [0]);
                case TextOnParseNodes.Phrase:
                    return ConvertPhrase (designNode, node.ChildNodes [0]);
                case TextOnParseNodes.MultiChoice:
                    return ConvertMultiChoice (designNode, node.ChildNodes [0]);
                default:
                    throw new ApplicationException (string.Format ("Unexpected token: {0}", token));
                }
            }
            throw new ApplicationException (string.Format ("Unexpected token: {0}", token));
        }

        private CompiledNode ConvertVariable (DesignNode designNode, ParseTreeNode node)
        {
            if (node.ChildNodes.Count != 3) throw new ApplicationException (string.Format ("Unexpected number of children {0}", node.ChildNodes.Count));
            var variableName = node.ChildNodes [1].Token.Text;
            return new CompiledNode (CompiledNodeType.Variable, designNode, false, new string [1] { variableName }, variableName, new CompiledNode [0], new TextOnParseElement [0]);
        }

        private CompiledNode ConvertPhrase (DesignNode designNode, ParseTreeNode node)
        {
            TextOnParseNodes token;
            if (!Enum.TryParse (node.Term.Name, out token))
                throw new ApplicationException (string.Format ("Unexpected token: {0}", node.Term.Name));
            if (token != TextOnParseNodes.Phrase)
                throw new ApplicationException (string.Format ("Unexpected token when expected a phrase: {0}", token));
            var text = string.Join ("", node.ChildNodes.Select ((cn) => ConvertText (cn)));
            return new CompiledNode (CompiledNodeType.Text, designNode, false, new string [0], text, new CompiledNode [0], new TextOnParseElement [0]);

        }

        private string ConvertText (ParseTreeNode node)
        {
            TextOnParseNodes token;
            if (!Enum.TryParse (node.Term.Name, out token))
                throw new ApplicationException (string.Format ("Unexpected token: {0}", node.Term.Name));
            switch (token) {
            case TextOnParseNodes.Text:
                return node.Token.Text;
            case TextOnParseNodes.Escape:
                return node.Token.Text [1].ToString ();
            case TextOnParseNodes.CompoundText:
                return ConvertText (node.ChildNodes [0]);
            default:
                throw new ApplicationException (string.Format ("Unexpected token: {0}", token));
            }
        }

        private CompiledNode ConvertMultiChoice (DesignNode designNode, ParseTreeNode node)
        {
            if (node.ChildNodes.Count != 3) throw new ApplicationException (string.Format ("Unexpected number of children {0}", node.ChildNodes.Count));
            node = node.ChildNodes [1];
            TextOnParseNodes token;
            if (!Enum.TryParse (node.Term.Name, out token))
                throw new ApplicationException (string.Format ("Unexpected token: {0}", node.Term.Name));
            switch (token) {
            case TextOnParseNodes.SimpleChoice:
                return ConvertSimpleChoice (designNode, node);
            case TextOnParseNodes.Choice:
                var childNodes = node.ChildNodes.Select ((cn) => ConvertSimpleChoice (designNode, cn)).ToArray();
                var hasErrors = false;
                HashSet<string> requiredNouns = new HashSet<string> ();
                foreach (var item in childNodes) {
                    hasErrors = hasErrors || item.HasErrors;
                    foreach (var requiredNoun in item.RequiredNouns) {
                        requiredNouns.Add (requiredNoun);
                    }
                }
                return new CompiledNode (CompiledNodeType.Choice, designNode, hasErrors, requiredNouns, "", childNodes, new TextOnParseElement [0]);
            default:
                throw new ApplicationException (string.Format ("Unexpected token: {0}", token));
            }
        }

        private CompiledNode ConvertSimpleChoice (DesignNode designNode, ParseTreeNode node)
        {
            if (node.ChildNodes.Count == 0) return new CompiledNode (CompiledNodeType.Text, designNode, false, new string [0], "", new CompiledNode [0], new TextOnParseElement [0]);
            if (node.ChildNodes.Count == 1) return ConvertAnything (designNode, node.ChildNodes [0]);
            var childNodes = node.ChildNodes.Select ((cn) => ConvertAnything (designNode, cn)).ToArray ();
            var hasErrors = false;
            HashSet<string> requiredNouns = new HashSet<string> ();
            foreach (var item in childNodes) {
                hasErrors = hasErrors || item.HasErrors;
                foreach (var requiredNoun in item.RequiredNouns) {
                    requiredNouns.Add (requiredNoun);
                }
            }
            return new CompiledNode (CompiledNodeType.Sequential, designNode, hasErrors, requiredNouns, "", childNodes, new TextOnParseElement [0]);
        }
    }
}

