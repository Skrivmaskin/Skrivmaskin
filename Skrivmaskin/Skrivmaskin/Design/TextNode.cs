using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Skrivmaskin.Design
{
    /// <summary>
    /// Text node. This is stored in the Skrivmaskin language.
    /// </summary>
    public sealed class TextNode : INode, IEquatable<TextNode>
    {
        internal TextNode () : this ("", true)
        {

        }

        public TextNode (string text, bool isActive)
        {
            Text = text;
            IsActive = isActive;
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Skrivmaskin.Design.TextNode"/> is active.
        /// </summary>
        /// <value><c>true</c> if is active; otherwise, <c>false</c>.</value>
        [DefaultValue (true)]
        public bool IsActive { get; set; }

        [JsonIgnore]
        public NodeType Type {
            get {
                return NodeType.Text;
            }
        }

        public bool Equals (TextNode other)
        {
            return (this.Text == other.Text);
        }

        public bool Equals (INode other)
        {
            var o = other as TextNode;
            if (o == null) return false;
            return this.Equals (o);
        }
    }
}
