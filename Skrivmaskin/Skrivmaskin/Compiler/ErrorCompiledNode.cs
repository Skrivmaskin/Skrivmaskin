using System;
using System.Collections.Generic;
using Skrivmaskin.Design;
using Skrivmaskin.Parsing;

namespace Skrivmaskin.Compiler
{
    /// <summary>
    /// Compiled node representing a compiler error.
    /// </summary>
    internal sealed class ErrorCompiledNode : ICompiledNode, ICompiledText
    {
        /// <summary>
        /// The location in the design tree of this item.
        /// </summary>
        /// <remarks>
        /// Will be null if this compiled tree is compiled for release.
        /// </remarks>
        /// <value>The location.</value>
        public INode Location { get; private set; }

        public IEnumerable<SkrivmaskinParseElement> Elements { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public CompiledNodeType Type {
            get {
                return CompiledNodeType.Error;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Skrivmaskin.Compiled.ErrorCompiledNode"/> has errors.
        /// </summary>
        /// <value><c>true</c> if has errors; otherwise, <c>false</c>.</value>
        public bool HasErrors { get { return true; } }

        /// <summary>
        /// Gets the child nodes.
        /// </summary>
        /// <value>The child nodes.</value>
        public IReadOnlyList<ICompiledNode> ChildNodes { get; private set; }

        /// <summary>
        /// Gets the required variables.
        /// </summary>
        /// <value>The required variables.</value>
        public IEnumerable<string> RequiredVariables {
            get {
                return new string [0];
            }
        }

        //TODO pass back info from Irony about the error
        internal ErrorCompiledNode (INode node, IEnumerable<SkrivmaskinParseElement> elements)
        {
            Location = node;
            Elements = elements;
        }
    }
}
