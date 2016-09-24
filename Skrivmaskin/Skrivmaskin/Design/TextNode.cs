using System;
using Newtonsoft.Json;

namespace Skrivmaskin.Design
{
    /// <summary>
    /// Text node. This is stored in the Skrivmaskin language.
    /// </summary>
    public sealed class TextNode : INode, IEquatable<TextNode>
    {
        public TextNode () : this ("")
        {

        }

        public TextNode (string text)
        {
            Text = text;
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

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