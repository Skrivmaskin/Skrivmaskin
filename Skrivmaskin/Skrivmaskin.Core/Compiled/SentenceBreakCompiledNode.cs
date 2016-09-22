using System;
using Skrivmaskin.Core.Design;

namespace Skrivmaskin.Core.Compiled
{
    /// <summary>
    /// Rationale: The generator can be provisioned with information about double spacing and so forth.
    /// </summary>
    internal sealed class SentenceBreakCompiledNode : ICompiledNode
    {
        /// <summary>
        /// The location in the design tree of this item.
        /// </summary>
        /// <remarks>
        /// Will be null if this compiled tree is compiled for release.
        /// </remarks>
        /// <value>The location.</value>
        public INode Location { get { return null; } set { } }

        /// <summary>
        /// The start character in the line of this item.
        /// </summary>
        /// <remarks>
        /// Will be null if this compiled tree is compiled for release.
        /// </remarks>
        /// <value>The start character.</value>
        public int? StartCharacter { get { return null; } set { } }

        /// <summary>
        /// The end character in the line of this item.
        /// </summary>
        /// <remarks>
        /// Will be null if this compiled tree is compiled for release.
        /// </remarks>
        /// <value>The end character.</value>
        public int? EndCharacter { get { return null; } set { } }
    }
}
