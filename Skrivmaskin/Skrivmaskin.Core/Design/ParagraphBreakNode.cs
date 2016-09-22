using System;
namespace Skrivmaskin.Core.Design
{
    /// <summary>
    /// Paragraph break node.
    /// </summary>
    public sealed class ParagraphBreakNode : INode
    {
        public bool Equals (INode other)
        {
            return (other is ParagraphBreakNode);
        }
    }
}
