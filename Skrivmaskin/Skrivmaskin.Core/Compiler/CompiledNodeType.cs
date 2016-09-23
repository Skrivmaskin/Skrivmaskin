using System;
namespace Skrivmaskin.Core.Compiler
{
    /// <summary>
    /// Compiled node type.
    /// </summary>
    public enum CompiledNodeType
    {
        Choice,
        Variable,
        Sequential,
        Text,
        Error
    }
}
