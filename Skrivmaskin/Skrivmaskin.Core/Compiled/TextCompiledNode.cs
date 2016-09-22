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
        /// The text for this item.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public CompiledNodeType Type {
            get {
                return CompiledNodeType.Text;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Skrivmaskin.Core.Compiled.ErrorCompiledNode"/> has errors.
        /// </summary>
        /// <value><c>true</c> if has errors; otherwise, <c>false</c>.</value>
        public bool HasErrors { get { return false; } }

        internal TextCompiledNode (string text, INode node, int? startCharacter, int? endCharacter)
        {
            Text = text;
            Location = node;
            StartCharacter = startCharacter;
            EndCharacter = endCharacter;
        }
    }
}
