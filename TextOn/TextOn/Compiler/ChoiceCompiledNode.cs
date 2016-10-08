using System;
using System.Linq;
using System.Collections.Generic;
using TextOn.Design;
using TextOn.Parsing;

namespace TextOn.Compiler
{
    /// <summary>
    /// Represents a compiled set of nodes to be chosen between.
    /// </summary>
    internal sealed class ChoiceCompiledNode : ICompiledNode
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
        public IReadOnlyList<ICompiledNode> Choices { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public CompiledNodeType Type {
            get {
                return CompiledNodeType.Choice;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:TextOn.Compiled.ChoiceCompiledNode"/>
        /// has errors.
        /// </summary>
        /// <value><c>true</c> if has errors; otherwise, <c>false</c>.</value>
        public bool HasErrors { get; private set;}

        /// <summary>
        /// Gets the required variables.
        /// </summary>
        /// <value>The required variables.</value>
        public IEnumerable<string> RequiredVariables { get; private set; }

        internal ChoiceCompiledNode (IReadOnlyList<ICompiledNode> childNodes, INode node)
        {
            bool hasErrors = false;
            foreach (var item in childNodes) {
                if (item.HasErrors) {
                    hasErrors = true;
                    break;
                }
            }
            Choices = childNodes;
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
