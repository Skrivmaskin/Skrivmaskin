using System;
using System.Collections.Generic;
using Skrivmaskin.Core.Design;

namespace Skrivmaskin.Core.Compiled
{
    /// <summary>
    /// A compiled node representing raw text.
    /// </summary>
    internal sealed class TextCompiledNode : ICompiledNode
    {
        /// <summary>
        /// The location in the design tree of this item.
        /// </summary>
        /// <remarks>
        /// Will be null if this compiled tree is compiled for release.
        /// </remarks>
        /// <value>The location.</value>
        public INode Location { get; set; }

        /// <summary>
        /// The start character in the line of this item.
        /// </summary>
        /// <remarks>
        /// Will be null if this compiled tree is compiled for release.
        /// </remarks>
        /// <value>The start character.</value>
        public int? StartCharacter { get; set; }

        /// <summary>
        /// The end character in the line of this item.
        /// </summary>
        /// <remarks>
        /// Will be null if this compiled tree is compiled for release.
        /// </remarks>
        /// <value>The end character.</value>
        public int? EndCharacter { get; set; }

        /// <summary>
        /// The text for this item.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        internal static ICompiledNode Make (string text, INode node, int? startCharacter, int? endCharacter)
        {
            return new TextCompiledNode () { Text = text, Location = node, StartCharacter = startCharacter, EndCharacter = endCharacter };
        }
    }
}
