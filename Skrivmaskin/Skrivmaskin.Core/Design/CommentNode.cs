using System;
namespace Skrivmaskin.Core.Design
{
    /// <summary>
    /// Free form comment that the user can set at any point.
    /// </summary>
    public sealed class CommentNode : INode
    {
        public CommentNode () : this ("", "")
        {

        }

        public CommentNode (string commentName, string comment)
        {
            CommentName = commentName;
            Value = comment;
        }

        /// <summary>
        /// Optional name for the comment.
        /// </summary>
        /// <value>The name.</value>
        public string CommentName { get; set; }

        /// <summary>
        /// Free form text for the comment.
        /// </summary>
        /// <value>The text.</value>
        public string Value { get; set; }
    }
}
