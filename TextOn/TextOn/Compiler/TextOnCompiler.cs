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
        Dictionary<TextNode, ICompiledNode> compiledNodes = new Dictionary<TextNode, ICompiledNode> ();
        readonly ILexerSyntax lexerSyntax;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Compilation.TextOnCompiler"/> class.
        /// </summary>
        /// <param name="lexerSyntax">Lexer syntax.</param>
        public TextOnCompiler (ILexerSyntax lexerSyntax)
        {
            parser = new TextOnParser (lexerSyntax);
            this.lexerSyntax = lexerSyntax;
        }

        public ICompiledNode GetCompiledNode (TextNode textNode)
        {
            return compiledNodes [textNode];
        }

        /// <summary>
        /// Compile the specified design time project.
        /// </summary>
        /// <param name="project">Project.</param>
        public CompiledTemplate Compile (TextOnTemplate project)
        {
            var transientCompiledNodes = new Dictionary<TextNode, ICompiledNode> ();
            var compiledNode = CompileNode (transientCompiledNodes, project.Definition);
            compiledNodes = transientCompiledNodes;
            var variables = new List<ICompiledVariable> ();
            foreach (var definition in project.VariableDefinitions) {
                foreach (var form in definition.Forms) {
                    var compiledVariable = new CompiledVariable (lexerSyntax, definition, form);
                    variables.Add (compiledVariable);
                }
            }
            return new CompiledTemplate (variables, compiledNode);
        }

        /// <summary>
        /// Compile some text into a compiled node for the gui.
        /// </summary>
        /// <returns>The text.</returns>
        /// <param name="text">Text.</param>
        public ICompiledNode CompileText (string text)
        {
            var transientCompiledNodes = new Dictionary<TextNode, ICompiledNode> ();
            return CompileNode (transientCompiledNodes, new TextNode (text, true));
        }

        private ICompiledNode CompileNode (Dictionary<TextNode, ICompiledNode> transientCompiledNodes, INode node)
        {
            if (!node.IsActive) return new BlankCompiledNode ();

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
            case NodeType.Choice:
                var choiceNode = node as ChoiceNode;
                return new ChoiceCompiledNode (choiceNode.Choices.Where ((c) => c.IsActive).Select ((c) => CompileNode (transientCompiledNodes, c)).Where ((c) => c.Type != CompiledNodeType.Blank).ToList (), choiceNode);
            case NodeType.Sequential:
                var sequentialNode = node as SequentialNode;
                if (sequentialNode.Sequential.Count > 0) {
                    var li = new List<ICompiledNode> ();
                    ICompiledNode compiledNode;
                    int i = 0;
                    for (; i < sequentialNode.Sequential.Count - 1; i++) {
                        compiledNode = CompileNode (transientCompiledNodes, sequentialNode.Sequential [i]);
                        if (compiledNode.Type != CompiledNodeType.Blank) li.Add (compiledNode);
                        if (sequentialNode.Sequential [i].Type != NodeType.ParagraphBreak &&
                            sequentialNode.Sequential [i].IsActive &&
                            sequentialNode.Sequential [i + 1].Type != NodeType.ParagraphBreak &&
                            sequentialNode.Sequential [i + 1].IsActive)
                            li.Add (new SentenceBreakCompiledNode ());
                    }
                    compiledNode = CompileNode (transientCompiledNodes, sequentialNode.Sequential [i]);
                    if (compiledNode.Type != CompiledNodeType.Blank) li.Add (compiledNode);
                    if (li.Count == 0) return new BlankCompiledNode ();
                    return new SequentialCompiledNode (li, sequentialNode);
                }
                return new BlankCompiledNode ();
            case NodeType.ParagraphBreak:
                return new ParagraphBreakCompiledNode ();
            default:
                throw new ApplicationException ("Unexpected design time node during compilation " + (node.GetType ()));
            }
        }
    }
}
