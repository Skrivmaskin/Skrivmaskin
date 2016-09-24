using System;
namespace Skrivmaskin.Compiler
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
        Error,
        Blank,
        SentenceBreak,
        ParagraphBreak
    }
}
