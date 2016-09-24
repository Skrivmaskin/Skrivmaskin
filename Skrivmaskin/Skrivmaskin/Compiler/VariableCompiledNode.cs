using System;
using System.Collections.Generic;
using Skrivmaskin.Design;

namespace Skrivmaskin.Compiler
{
    /// <summary>
    /// A compiled node representing raw text.
    /// </summary>
    internal sealed class VariableCompiledNode : ICompiledNode
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
        /// The full name for this variable.
        /// </summary>
        /// <value>The full name.</value>
        public string VariableFullName { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public CompiledNodeType Type {
            get {
                return CompiledNodeType.Variable;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:Skrivmaskin.Compiled.ErrorCompiledNode"/> has errors.
        /// </summary>
        /// <value><c>true</c> if has errors; otherwise, <c>false</c>.</value>
        public bool HasErrors { get { return false; } }

        public IEnumerable<string> RequiredVariables { get { return new string [1] { VariableFullName }; } }

        internal VariableCompiledNode (string variableFullName, INode node, int? startCharacter, int? endCharacter)
        {
            VariableFullName = variableFullName;
            Location = node;
            StartCharacter = startCharacter;
            EndCharacter = endCharacter;
        }
    }
}
