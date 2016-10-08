using System;
namespace TextOn.Compiler
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
        Success,
        Blank,
        SentenceBreak,
        ParagraphBreak
    }
}
