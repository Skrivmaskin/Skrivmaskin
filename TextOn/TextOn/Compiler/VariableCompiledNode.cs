using System;
using System.Collections.Generic;
using TextOn.Design;
using TextOn.Parsing;

namespace TextOn.Compiler
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
        /// Gets a value indicating whether this <see cref="T:TextOn.Compiled.ErrorCompiledNode"/> has errors.
        /// </summary>
        /// <value><c>true</c> if has errors; otherwise, <c>false</c>.</value>
        public bool HasErrors { get { return false; } }

        public IEnumerable<string> RequiredVariables { get { return new string [1] { VariableFullName }; } }

        internal VariableCompiledNode (string variableFullName, INode node)
        {
            VariableFullName = variableFullName;
            Location = node;
        }
    }
}
