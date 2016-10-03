using System;
using System.Collections.Generic;
using Skrivmaskin.Design;
using Skrivmaskin.Parsing;

namespace Skrivmaskin.Compiler
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
        /// Gets a value indicating whether this <see cref="T:Skrivmaskin.Compiled.ErrorCompiledNode"/> has errors.
        /// </summary>
        /// <value><c>true</c> if has errors; otherwise, <c>false</c>.</value>
        public bool HasErrors { get { return false; } }

        public IEnumerable<string> RequiredVariables {
            get {
                return new string [0];
            }
        }

        internal TextCompiledNode (string text, INode node)
        {
            Text = text;
            Location = node;
        }
    }
}
