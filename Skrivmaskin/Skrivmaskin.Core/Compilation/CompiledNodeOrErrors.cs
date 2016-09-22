using System;
using Skrivmaskin.Core.Compiled;

namespace Skrivmaskin.Core.Compilation
{
    internal class CompiledNodeOrErrors
    {
        readonly ICompiledNode compiledNode;
        readonly CompilationErrors errors;
    }
}
