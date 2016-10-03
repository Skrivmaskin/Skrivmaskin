using System;
using System.Collections.Generic;
using Skrivmaskin.Design;
using Skrivmaskin.Parsing;

namespace Skrivmaskin.Compiler
{
    /// <summary>
    /// Represents a sentence break. Will generate the empty string if either the previous or next node generates
    /// blank text, or the user's spacing string otherwise.
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
        public INode Location { get; private set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>The type.</value>
        public CompiledNodeType Type {
            get {
                return CompiledNodeType.SentenceBreak;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:Skrivmaskin.Compiled.ChoiceCompiledNode"/>
        /// has errors.
        /// </summary>
        /// <value><c>true</c> if has errors; otherwise, <c>false</c>.</value>
        public bool HasErrors { get; private set; }

        /// <summary>
        /// Gets the required variables.
        /// </summary>
        /// <value>The required variables.</value>
        public IEnumerable<string> RequiredVariables { get; private set; }

        internal SentenceBreakCompiledNode ()
        {
            Location = null;
            HasErrors = false;
            RequiredVariables = new string [0];
        }
    }
}

