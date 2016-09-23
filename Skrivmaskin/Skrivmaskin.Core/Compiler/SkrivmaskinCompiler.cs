using System;
using System.Linq;
using System.Collections.Generic;
using Skrivmaskin.Core.Design;
using Skrivmaskin.Core.Interfaces;
using Skrivmaskin.Core.Parsing;

namespace Skrivmaskin.Core.Compiler
{
    /// <summary>
    /// Skrivmaskin compiler. This manages compilation of the tree.
    /// </summary>
    public sealed class SkrivmaskinCompiler
    {
        readonly SkrivmaskinParser parser;
        Dictionary<TextNode, ICompiledNode> compiledNodes = new Dictionary<TextNode, ICompiledNode> ();
        readonly ICompilerConfig compilerConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Skrivmaskin.Core.Compilation.SkrivmaskinCompiler"/> class.
        /// </summary>
        /// <param name="lexerSyntax">Lexer syntax.</param>
        public SkrivmaskinCompiler (ILexerSyntax lexerSyntax, ICompilerConfig compilerConfig)
        {
            parser = new SkrivmaskinParser (lexerSyntax);
            this.compilerConfig = compilerConfig;
        }

        /// <summary>
        /// Compile the specified design time project.
        /// </summary>
        /// <param name="project">Project.</param>
        public ICompiledNode Compile (Project project)
        {
            var transientCompiledNodes = new Dictionary<TextNode, ICompiledNode> ();
            var compiledNode = CompileNode (transientCompiledNodes, project.Definition);
            compiledNodes = transientCompiledNodes;
            return compiledNode;
        }

        private ICompiledNode CompileNode (Dictionary<TextNode, ICompiledNode> transientCompiledNodes, INode node)
        {
            // Text nodes are a special case here. The compiled node will come out as an error compiled node if there are errors.
            ICompiledNode result;
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
            case NodeType.Comment:
                var commentNode = node as CommentNode;
                return new TextCompiledNode ("", commentNode, 1, commentNode.Value.Length);
            case NodeType.Choice:
                var choiceNode = node as ChoiceNode;
                return new ChoiceCompiledNode (choiceNode.Choices.Select ((c) => CompileNode (transientCompiledNodes, c)).ToList (), choiceNode, 1, 1);
            case NodeType.Sequential:
                var sequentialNode = node as SequentialNode;
                if (sequentialNode.Sequential.Count > 0) {
                    var li = new List<ICompiledNode> ();
                    for (int i = 0; i < sequentialNode.Sequential.Count - 1; i++) {
                        li.Add (CompileNode (transientCompiledNodes, sequentialNode.Sequential [i]));
                        li.Add (new TextCompiledNode (compilerConfig.Spacing, node, 1, 1));
                    }
                    li.Add (CompileNode (transientCompiledNodes, sequentialNode.Sequential [sequentialNode.Sequential.Count - 1]));
                    return new SequentialCompiledNode (li, sequentialNode, 1, 1);
                }
                return new TextCompiledNode ("", sequentialNode, 1, 1);
            case NodeType.ParagraphBreak:
                return new TextCompiledNode (compilerConfig.ParagraphBreak, node, 1, 1);
            default:
                throw new ApplicationException ("Unexpected design time node during compilation " + (node.GetType ()));
            }
        }
    }
}
