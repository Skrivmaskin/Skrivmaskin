using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace TextOn.Design
{
    /// <summary>
    /// Choice node. Nodes below here are chosen at random.
    /// </summary>
    public sealed class ChoiceNode : INode, IEquatable<ChoiceNode>
    {
        internal ChoiceNode () : this ("", true, new List<INode> ())
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Design.ChoiceNode"/> class.
        /// </summary>
        /// <param name="choiceName">Choice name.</param>
        /// <param name="isActive">If set to <c>true</c> is active.</param>
        /// <param name="choices">Choices.</param>
        public ChoiceNode (string choiceName, bool isActive, List<INode> choices)
        {
            ChoiceName = choiceName;
            IsActive = isActive;
            Choices = choices;
        }

        /// <summary>
        /// User's name for this choice node.
        /// </summary>
        /// <value>The name.</value>
        public string ChoiceName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:TextOn.Design.ChoiceNode"/> is active.
        /// </summary>
        /// <value><c>true</c> if is active; otherwise, <c>false</c>.</value>
        [DefaultValue(true)]
        public bool IsActive { get; set; }

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

        public override string ToString ()
        {
            return string.Format ("[ChoiceNode: ChoiceName={0}, IsActive={1}, Choices={2}]", ChoiceName, IsActive, Choices);
        }
    }
}
