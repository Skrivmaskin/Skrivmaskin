using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Skrivmaskin.Design
{
    /// <summary>
    /// Choice node. Nodes below here are chosen at random.
    /// </summary>
    public sealed class ChoiceNode : INode, IEquatable<ChoiceNode>
    {
        public ChoiceNode () : this ("", new List<INode> ())
        {

        }

        public ChoiceNode (string choiceName, List<INode> choices)
        {
            ChoiceName = choiceName;
            Choices = choices;
        }

        /// <summary>
        /// User's name for this choice node.
        /// </summary>
        /// <value>The name.</value>
        public string ChoiceName { get; set; }

        /// <summary>
        /// Gets or sets the subnodes.
        /// </summary>
        /// <value>The choices.</value>
        public List<INode> Choices { get; set; }

        [JsonIgnore]
        public NodeType Type {
            get {
                return NodeType.Choice;
            }
        }

        public bool Equals (INode other)
        {
            var o = other as ChoiceNode;
            if (o == null) return false;
            return this.Equals (o);
        }

        public bool Equals (ChoiceNode other)
        {
            if (this.ChoiceName != other.ChoiceName) return false;
            if (this.Choices.Count != other.Choices.Count) return false;
            for (int i = 0; i < this.Choices.Count; i++) {
                if (!this.Choices [i].Equals (other.Choices)) return false;
            }
            return true;
        }
    }
}
