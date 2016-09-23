using System;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Skrivmaskin.Core.Compilation.SkrivmaskinCompiler"/> class.
        /// </summary>
        /// <param name="lexerSyntax">Lexer syntax.</param>
        public SkrivmaskinCompiler (ILexerSyntax lexerSyntax)
        {
            parser = new SkrivmaskinParser (lexerSyntax);
        }

        /// <summary>
        /// Compile the specified design time project.
        /// </summary>
        /// <param name="project">Project.</param>
        public ICompiledNode Compile (Project project)
        {
            var transientCompiledNodes = new Dictionary<TextNode, ICompiledNode> ();
            throw new NotImplementedException ();
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
                break;
            case NodeType.Comment:
                break;
            case NodeType.Choice:
                break;
            case NodeType.Sequential:
                break;
            case NodeType.ParagraphBreak:
                break;
            default:
                break;
            }
            //      if (transientCompiledNodes.TryGetValue (textNode, out result)) {
            //         return result;
            //      } else if (compiledNodes.TryGetValue (textNode, out result)) {
            //           transientCompiledNodes.Add (textNode, result);
            //            return result;
            //          } else {
            //
            //            }
            return null;
        }
    }
}
