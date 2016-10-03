using System;
using System.Collections.Generic;
using Skrivmaskin.Design;
using Skrivmaskin.Parsing;

namespace Skrivmaskin.Compiler
{
    /// <summary>
    /// Represents a compiled set of nodes to be laid out sequentially.
    /// </summary>
    internal sealed class SequentialCompiledNode : ICompiledNode
    {
        /// <summary>
        /// The location in the design tree of this item.
        /// </summary>
        /// <remarks>
        /// Will be null if this compiled tree is compiled for release.
        /// </remarks>
        /// <value>The location.</value>
        public INode Location { get; private set; }

        /// <summary>
        /// Ths choices.
        /// </summary>
        /// <value>The choices.</value>
        public IReadOnlyList<ICompiledNode> Sequential { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public CompiledNodeType Type {
            get {
                return CompiledNodeType.Sequential;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Skrivmaskin.Compiled.ChoiceCompiledNode"/>
        /// has errors.
        /// </summary>
        /// <value><c>true</c> if has errors; otherwise, <c>false</c>.</value>
        public bool HasErrors { get; private set; }

        /// <summary>
        /// Gets the required variables.
        /// </summary>
        /// <value>The required variables.</value>
        public IEnumerable<string> RequiredVariables { get; private set; }

        internal SequentialCompiledNode (IReadOnlyList<ICompiledNode> childNodes, INode node)
        {
            bool hasErrors = false;
            foreach (var item in childNodes) {
                if (item.HasErrors) {
                    hasErrors = true;
                    break;
                }
            }
            Sequential = childNodes;
            Location = node;
            HasErrors = hasErrors;
            var rv = new HashSet<string> ();
            foreach (var cn in childNodes) {
                foreach (var item in cn.RequiredVariables) {
                    rv.Add (item);
                }
            }
            RequiredVariables = rv;
        }
    }
}
