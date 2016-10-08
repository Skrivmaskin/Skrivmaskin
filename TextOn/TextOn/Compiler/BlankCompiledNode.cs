using System;
using System.Collections.Generic;
using TextOn.Design;
using TextOn.Parsing;

namespace TextOn.Compiler
{
    /// <summary>
    /// Represents a comment or something that compies to nothing.
    /// </summary>
    internal sealed class BlankCompiledNode : ICompiledNode
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
                return CompiledNodeType.Blank;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:TextOn.Compiled.ChoiceCompiledNode"/>
        /// has errors.
        /// </summary>
        /// <value><c>true</c> if has errors; otherwise, <c>false</c>.</value>
        public bool HasErrors { get; private set; }

        /// <summary>
        /// Gets the required variables.
        /// </summary>
        /// <value>The required variables.</value>
        public IEnumerable<string> RequiredVariables { get; private set; }

        internal BlankCompiledNode ()
        {
            Location = null;
            HasErrors = false;
            RequiredVariables = new string [0];
        }
    }
}

