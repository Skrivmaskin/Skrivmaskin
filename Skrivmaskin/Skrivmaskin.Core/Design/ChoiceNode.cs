using System;
using System.Collections.Generic;

namespace Skrivmaskin.Core.Design
{
    /// <summary>
    /// Choice node. Nodes below here are chosen at random.
    /// </summary>
    public sealed class ChoiceNode : INode
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
    }
}
