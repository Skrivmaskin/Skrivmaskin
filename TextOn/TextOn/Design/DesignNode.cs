using System;
namespace TextOn.Design
{
    /// <summary>
    /// Immutable representation of a node in the design tree.
    /// </summary>
    public sealed class DesignNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Design.DesignNode"/> class.
        /// </summary>
        /// <param name="nodeType">Node type.</param>
        /// <param name="isActive">If set to <c>true</c> is active.</param>
        /// <param name="text">Text.</param>
        /// <param name="childNodes">Child nodes.</param>
        public DesignNode (NodeType nodeType, bool isActive, string text, DesignNode[] childNodes)
        {
            Type = nodeType;
            IsActive = isActive;
            Text = text;
            ChildNodes = childNodes;
        }

        /// <summary>
        /// Gets the type of this Design node.
        /// </summary>
        /// <value>The type.</value>
        public NodeType Type { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:TextOn.Design.DesignNode"/> is active.
        /// </summary>
        /// <value><c>true</c> if is active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; private set; }

        /// <summary>
        /// Gets the text. This can be either the output text for a text node or the name of the node for a Choice/Sequential.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; private set; }

        /// <summary>
        /// Gets the child nodes.
        /// </summary>
        /// <value>The child nodes.</value>
        public DesignNode [] ChildNodes { get; private set; }
    }
}
