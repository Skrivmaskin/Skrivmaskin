using System;
using Newtonsoft.Json;

namespace Skrivmaskin.Design
{
    /// <summary>
    /// Free form comment that the user can set at any point.
    /// </summary>
    public sealed class CommentNode : INode, IEquatable<CommentNode>
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

        [JsonIgnore]
        public NodeType Type {
            get {
                return NodeType.Comment;
            }
        }

        public bool Equals (CommentNode other)
        {
            return ((this.CommentName == other.CommentName) && (this.Value == other.Value));
        }

        public bool Equals (INode other)
        {
            var o = other as CommentNode;
            if (o == null) return false;
            return this.Equals (o);
        }
    }
}
