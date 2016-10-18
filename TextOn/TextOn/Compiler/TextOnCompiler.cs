using System;
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
        private static readonly TextOnParseElement [] emptyElements = new TextOnParseElement [0];
        private static readonly CompiledNode [] emptyCompiledNodes = new CompiledNode [0];
        private static readonly DesignNode [] emptyDesignSubtree = new DesignNode [0];
        private static readonly string [] emptyRequiredNouns = new string [0];
        private const string emptyString = "";

        readonly TextOnParser parser;
        Dictionary<DesignNode, CompiledNode> compiledNodes = new Dictionary<DesignNode, CompiledNode> (); // should use ReferenceEquals

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Compilation.TextOnCompiler"/> class.
        /// </summary>
        /// <param name="lexerSyntax">Lexer syntax.</param>
        public TextOnCompiler (ILexerSyntax lexerSyntax)
        {
            parser = new TextOnParser (lexerSyntax);
        }

        public CompiledNode GetCompiledNode (DesignNode textNode)
        {
            return compiledNodes [textNode];
        }

        /// <summary>
        /// Compile the specified design time project.
        /// </summary>
        /// <param name="project">Project.</param>
        public CompiledTemplate Compile (TextOnTemplate project)
        {
            var transientCompiledNodes = new Dictionary<DesignNode, CompiledNode> ();
            var compiledNode = CompileNode (transientCompiledNodes, project.DesignTree);
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
            var transientCompiledNodes = new Dictionary<DesignNode, CompiledNode> ();
            return CompileNode (transientCompiledNodes, new DesignNode (NodeType.Text, true, text, emptyDesignSubtree));
        }

        private CompiledNode CompileNode (Dictionary<DesignNode, CompiledNode> transientCompiledNodes, DesignNode node)
        {
            if (!node.IsActive) return new CompiledNode (CompiledNodeType.Blank, node, false, emptyRequiredNouns, emptyString, emptyCompiledNodes, emptyElements);

            // Text nodes are a special case here. The compiled node will come out as an error compiled node if there are errors.
            CompiledNode result;
            var hasErrors = false;
            HashSet<string> requiredNouns = new HashSet<string> ();
            switch (node.Type) {
            case NodeType.Text:
                if (transientCompiledNodes.TryGetValue (node, out result))
                    return result;
                if (compiledNodes.TryGetValue (node, out result)) {
                    transientCompiledNodes.Add (node, result);
                    return result;
                }
                result = parser.Compile (node);
                transientCompiledNodes.Add (node, result);
                return result;
            case NodeType.Choice:
                var choices = new List<CompiledNode> ();
                foreach (var choice in node.ChildNodes) {
                    if (choice.IsActive) {
                        var cn = CompileNode (transientCompiledNodes, choice);
                        if (cn.Type != CompiledNodeType.Blank) {
                            hasErrors = hasErrors || cn.HasErrors;
                            foreach (var rn in cn.RequiredNouns) {
                                requiredNouns.Add (rn);
                            }
                            choices.Add (cn);
                        }
                    }
                }
                return new CompiledNode (CompiledNodeType.Choice, node, hasErrors, requiredNouns, emptyString, choices.ToArray (), emptyElements);
            case NodeType.Sequential:
                if (node.ChildNodes.Length > 0) {
                    var li = new List<CompiledNode> ();
                    CompiledNode compiledNode;
                    int i = 0;
                    for (; i < node.ChildNodes.Length - 1; i++) {
                        compiledNode = CompileNode (transientCompiledNodes, node.ChildNodes [i]);
                        if (compiledNode.Type != CompiledNodeType.Blank) {
                            hasErrors = hasErrors || compiledNode.HasErrors;
                            foreach (var rn in compiledNode.RequiredNouns) {
                                requiredNouns.Add (rn);
                            }
                            li.Add (compiledNode);
                        }
                        if (node.ChildNodes [i].Type != NodeType.ParagraphBreak &&
                            node.ChildNodes [i].IsActive &&
                            node.ChildNodes [i + 1].Type != NodeType.ParagraphBreak &&
                            node.ChildNodes [i + 1].IsActive)
                            li.Add (new CompiledNode (CompiledNodeType.SentenceBreak, node, false, emptyRequiredNouns, emptyString, emptyCompiledNodes, emptyElements));
                    }
                    compiledNode = CompileNode (transientCompiledNodes, node.ChildNodes [i]);
                    hasErrors = hasErrors || compiledNode.HasErrors;
                    foreach (var rn in compiledNode.RequiredNouns) {
                        requiredNouns.Add (rn);
                    }
                    if (compiledNode.Type != CompiledNodeType.Blank) li.Add (compiledNode);
                    if (li.Count == 0) return new CompiledNode (CompiledNodeType.Blank, node, false, emptyRequiredNouns, emptyString, emptyCompiledNodes, emptyElements);
                    return new CompiledNode (CompiledNodeType.Sequential, node, hasErrors, requiredNouns, emptyString, li.ToArray (), emptyElements);
                }
                return new CompiledNode (CompiledNodeType.Blank, node, false, emptyRequiredNouns, emptyString, emptyCompiledNodes, emptyElements);
            case NodeType.ParagraphBreak:
                return new CompiledNode (CompiledNodeType.ParagraphBreak, node, false, emptyRequiredNouns, emptyString, emptyCompiledNodes, emptyElements);
            default:
                throw new ApplicationException ("Unexpected design time node during compilation " + (node.GetType ()));
            }
        }
    }
}
