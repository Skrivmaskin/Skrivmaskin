using System;
using System.Linq;
using System.Collections.Generic;
using TextOn.Design;
using TextOn.Interfaces;
using TextOn.Parsing;

namespace TextOn.Compiler
{
    /// <summary>
    /// TextOn compiler. This manages compilation of the tree.
    /// </summary>
    public sealed class TextOnCompiler
    {
        readonly TextOnParser parser;
        Dictionary<TextNode, CompiledNode> compiledNodes = new Dictionary<TextNode, CompiledNode> ();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Compilation.TextOnCompiler"/> class.
        /// </summary>
        /// <param name="lexerSyntax">Lexer syntax.</param>
        public TextOnCompiler (ILexerSyntax lexerSyntax)
        {
            parser = new TextOnParser (lexerSyntax);
        }

        public CompiledNode GetCompiledNode (TextNode textNode)
        {
            return compiledNodes [textNode];
        }

        /// <summary>
        /// Compile the specified design time project.
        /// </summary>
        /// <param name="project">Project.</param>
        public CompiledTemplate Compile (TextOnTemplate project)
        {
            var transientCompiledNodes = new Dictionary<TextNode, CompiledNode> ();
            var compiledNode = CompileNode (transientCompiledNodes, project.Definition);
            compiledNodes = transientCompiledNodes;
            return new CompiledTemplate (project.Nouns, compiledNode);
        }

        /// <summary>
        /// Compile some text into a compiled node for the gui.
        /// </summary>
        /// <returns>The text.</returns>
        /// <param name="text">Text.</param>
        public CompiledNode CompileText (string text)
        {
            var transientCompiledNodes = new Dictionary<TextNode, CompiledNode> ();
            return CompileNode (transientCompiledNodes, new TextNode (text, true));
        }

        private CompiledNode CompileNode (Dictionary<TextNode, CompiledNode> transientCompiledNodes, INode node)
        {
            if (!node.IsActive) return new CompiledNode (CompiledNodeType.Blank, node, false, new string [0], "", new CompiledNode [0], new TextOnParseElement [0]);

            // Text nodes are a special case here. The compiled node will come out as an error compiled node if there are errors.
            CompiledNode result;
            var hasErrors = false;
            HashSet<string> requiredNouns = new HashSet<string> ();
            switch (node.Type) {
            case NodeType.Text:
                var textNode = node as TextNode;
                if (transientCompiledNodes.TryGetValue (textNode, out result))
                    return result;
                if (compiledNodes.TryGetValue (textNode, out result)) {
                    transientCompiledNodes.Add (textNode, result);
                    return result;
                }
                result = parser.Compile (textNode);
                transientCompiledNodes.Add (textNode, result);
                return result;
            case NodeType.Choice:
                var choiceNode = node as ChoiceNode;
                var choices = choiceNode.Choices.Where ((c) => c.IsActive).Select ((c) => CompileNode (transientCompiledNodes, c)).Where ((c) => c.Type != CompiledNodeType.Blank).ToArray ();
                foreach (var item in choices) {
                    hasErrors = hasErrors || item.HasErrors;
                    foreach (var requiredNoun in item.RequiredNouns) {
                        requiredNouns.Add (requiredNoun);
                    }
                }
                return new CompiledNode (CompiledNodeType.Choice, node, hasErrors, requiredNouns, "", choices, new TextOnParseElement [0]);
            case NodeType.Sequential:
                var sequentialNode = node as SequentialNode;
                if (sequentialNode.Sequential.Count > 0) {
                    var li = new List<CompiledNode> ();
                    CompiledNode compiledNode;
                    int i = 0;
                    for (; i < sequentialNode.Sequential.Count - 1; i++) {
                        compiledNode = CompileNode (transientCompiledNodes, sequentialNode.Sequential [i]);
                        if (compiledNode.Type != CompiledNodeType.Blank) li.Add (compiledNode);
                        if (sequentialNode.Sequential [i].Type != NodeType.ParagraphBreak &&
                            sequentialNode.Sequential [i].IsActive &&
                            sequentialNode.Sequential [i + 1].Type != NodeType.ParagraphBreak &&
                            sequentialNode.Sequential [i + 1].IsActive)
                            li.Add (new CompiledNode (CompiledNodeType.SentenceBreak, null, false, new string [0], "", new CompiledNode [0], new TextOnParseElement [0]));
                    }
                    compiledNode = CompileNode (transientCompiledNodes, sequentialNode.Sequential [i]);
                    if (compiledNode.Type != CompiledNodeType.Blank) li.Add (compiledNode);
                    if (li.Count == 0) return new CompiledNode (CompiledNodeType.Blank, node, false, new string [0], "", new CompiledNode [0], new TextOnParseElement [0]);
                    foreach (var item in li) {
                        hasErrors = hasErrors || item.HasErrors;
                        foreach (var requiredNoun in item.RequiredNouns) {
                            requiredNouns.Add (requiredNoun);
                        }
                    }
                    return new CompiledNode (CompiledNodeType.Sequential, node, hasErrors, requiredNouns, "", li.ToArray (), new TextOnParseElement [0]);
                }
                return new CompiledNode (CompiledNodeType.Blank, node, false, new string [0], "", new CompiledNode [0], new TextOnParseElement [0]);
            case NodeType.ParagraphBreak:
                return new CompiledNode (CompiledNodeType.ParagraphBreak, node, false, new string [0], "", new CompiledNode [0], new TextOnParseElement [0]);
            default:
                throw new ApplicationException ("Unexpected design time node during compilation " + (node.GetType ()));
            }
        }
    }
}
