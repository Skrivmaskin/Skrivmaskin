using System;
namespace Skrivmaskin.Core.Compiled
{
    /// <summary>
    /// Compiled node type.
    /// </summary>
    public enum CompiledNodeType
    {
        Choice,
        Variable,
        ParagraphBreak,
        SentenceBreak,
        Sequential,
        Text,
        Error
    }
}
