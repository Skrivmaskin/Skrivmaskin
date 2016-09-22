using System;
using System.Collections.Generic;
using Skrivmaskin.Core.Compiled;
using Skrivmaskin.Core.Design;
using Skrivmaskin.Core.Interfaces;
using Skrivmaskin.Core.Parsing;

namespace Skrivmaskin.Core.Compilation
{
    /// <summary>
    /// Skrivmaskin compiler. This manages compilation of the tree.
    /// </summary>
    public sealed class SkrivmaskinCompiler
    {
        readonly SkrivmaskinParser parser;
        readonly Dictionary<TextNode, ICompiledNode> compiledNodes = new Dictionary<TextNode, ICompiledNode> ();

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
        public ICompiledNode Compile (Project project, out CompilationErrors compilationErrors)
        {
            throw new NotImplementedException ();
        }

        private ICompiledNode CompileNode (INode node, out CompilationErrors compilationErrors, out VariableRequirements variableRequirements)
        {
            throw new NotImplementedException ();
        }
    }
}
