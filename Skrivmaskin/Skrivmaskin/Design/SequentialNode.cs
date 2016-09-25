using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Skrivmaskin.Design
{
    /// <summary>
    /// Sequential node. Nodes below here are inserted sequentially.
    /// </summary>
    public class SequentialNode : INode, IEquatable<SequentialNode>
    {
        internal SequentialNode () : this ("", true, new List<INode> ())
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Skrivmaskin.Design.SequentialNode"/> class.
        /// </summary>
        /// <param name="sequentialName">Sequential name.</param>
        /// <param name="isActive">If set to <c>true</c> is active.</param>
        /// <param name="sequential">Sequential.</param>
        public SequentialNode (string sequentialName, bool isActive, List<INode> sequential)
        {
            SequentialName = sequentialName;
            IsActive = isActive;
            Sequential = sequential;
        }

        /// <summary>
        /// User's name for this sequential node.
        /// </summary>
        /// <value>The name.</value>
        public string SequentialName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Skrivmaskin.Design.SequentialNode"/> is active.
        /// </summary>
        /// <value><c>true</c> if is active; otherwise, <c>false</c>.</value>
        [DefaultValue (true)]
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the subnodes.
        /// </summary>
        /// <value>The nodes.</value>
        public List<INode> Sequential { get; set; }

        [JsonIgnore]
        public NodeType Type {
            get {
                return NodeType.Sequential;
            }
        }

        public bool Equals (SequentialNode other)
        {
            if (this.SequentialName != other.SequentialName) return false;
            if (this.Sequential.Count != other.Sequential.Count) return false;
            for (int i = 0; i < this.Sequential.Count; i++) {
                if (!this.Sequential [i].Equals (other.Sequential [i])) return false;
            }
            return true;
        }

        public bool Equals (INode other)
        {
            var o = other as SequentialNode;
            if (o == null) return false;
            return this.Equals (o);
        }
    }
}
