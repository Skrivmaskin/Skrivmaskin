using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Skrivmaskin.Design
{
    /// <summary>
    /// Paragraph break node.
    /// </summary>
    public sealed class ParagraphBreakNode : INode
    {
        internal ParagraphBreakNode () : this (true)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Skrivmaskin.Design.ParagraphBreakNode"/> class.
        /// </summary>
        /// <param name="isActive">If set to <c>true</c> is active.</param>
        public ParagraphBreakNode (bool isActive)
        {
            IsActive = isActive;
        }

        public bool Equals (INode other)
        {
            return (other is ParagraphBreakNode);
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Skrivmaskin.Design.ParagraphBreakNode"/> is active.
        /// </summary>
        /// <value><c>true</c> if is active; otherwise, <c>false</c>.</value>
        [DefaultValue (true)]
        public bool IsActive { get; set; }

        [JsonIgnore]
        public NodeType Type { get { return NodeType.ParagraphBreak; } }

        public override string ToString ()
        {
            return string.Format ("[ParagraphBreakNode: IsActive={0}]", IsActive);
        }
    }
}
