using System;
using Newtonsoft.Json;

namespace Skrivmaskin.Design
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

        [JsonIgnore]
        public NodeType Type { get { return NodeType.ParagraphBreak; } }
    }
}
