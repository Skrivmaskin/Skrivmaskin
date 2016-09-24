using System;
using System.Collections.Generic;
using Skrivmaskin.Design;

namespace Skrivmaskin.Compiler
{
    /// <summary>
    /// Compiled node representing a compiler error.
    /// </summary>
    internal sealed class ErrorCompiledNode : ICompiledNode
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

        public IEnumerable<string> RequiredVariables {
            get {
                return new string [0];
            }
        }

        //TODO pass back info from Irony about the error
        internal ErrorCompiledNode (INode node, int? startCharacter, int? endCharacter)
        {
            Location = node;
            StartCharacter = startCharacter;
            EndCharacter = endCharacter;
        }
    }
}
