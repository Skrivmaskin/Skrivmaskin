using System;
using System.Collections.Generic;
using Skrivmaskin.Core.Design;

namespace Skrivmaskin.Core.Compiled
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
        /// The start character in the line of this item.
        /// </summary>
        /// <remarks>
        /// Will be null if this compiled tree is compiled for release.
        /// </remarks>
        /// <value>The start character.</value>
        public int? StartCharacter { get; private set; }

        /// <summary>
        /// The end character in the line of this item.
        /// </summary>
        /// <remarks>
        /// Will be null if this compiled tree is compiled for release.
        /// </remarks>
        /// <value>The end character.</value>
        public int? EndCharacter { get; private set; }

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
        /// Gets or sets a value indicating whether this <see cref="T:Skrivmaskin.Core.Compiled.ChoiceCompiledNode"/>
        /// has errors.
        /// </summary>
        /// <value><c>true</c> if has errors; otherwise, <c>false</c>.</value>
        public bool HasErrors { get; private set; }

        internal SequentialCompiledNode (IReadOnlyList<ICompiledNode> childNodes, INode node, int? startCharacter, int? endCharacter)
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
            StartCharacter = startCharacter;
            EndCharacter = endCharacter;
            HasErrors = hasErrors;
        }
    }
}
