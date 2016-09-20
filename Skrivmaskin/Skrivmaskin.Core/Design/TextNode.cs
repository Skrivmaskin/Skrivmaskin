using System;
namespace Skrivmaskin.Core.Design
{
    /// <summary>
    /// Text node. This is stored in the Skrivmaskin language.
    /// </summary>
    public sealed class TextNode : INode
    {
        public TextNode () : this ("")
        {

        }

        public TextNode (string text)
        {
            Value = text;
        }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Value { get; set; }
    }
}
