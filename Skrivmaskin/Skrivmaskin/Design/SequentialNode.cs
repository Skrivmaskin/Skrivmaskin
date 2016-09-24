using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Skrivmaskin.Design
{
    /// <summary>
    /// Sequential node. Nodes below here are inserted sequentially.
    /// </summary>
    public class SequentialNode : INode,IEquatable<SequentialNode>
    {
        public SequentialNode () : this ("", new List<INode> ())
        {

        }

        public SequentialNode (string sequentialName, List<INode> sequential)
        {
            SequentialName = sequentialName;
            Sequential = sequential;
        }

        /// <summary>
        /// User's name for this sequential node.
        /// </summary>
        /// <value>The name.</value>
        public string SequentialName { get; set; }

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
