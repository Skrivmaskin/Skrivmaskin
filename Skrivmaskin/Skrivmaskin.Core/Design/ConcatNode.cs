using System;
using System.Collections.Generic;

namespace Skrivmaskin.Core.Design
{
    /// <summary>
    /// Concat node. Nodes below here are concatenated.
    /// </summary>
    public sealed class ConcatNode : INode
    {
        public ConcatNode () : this ("", new List<INode> ())
        {

        }

        public ConcatNode (string concatName, List<INode> sequential)
        {
            ConcatName = concatName;
            Sequential = sequential;
        }

        /// <summary>
        /// User's name for this concat node.
        /// </summary>
        /// <value>The name.</value>
        public string ConcatName { get; set; }

        /// <summary>
        /// Gets or sets the subnodes.
        /// </summary>
        /// <value>The nodes.</value>
        public List<INode> Sequential { get; set; }
    }
}
