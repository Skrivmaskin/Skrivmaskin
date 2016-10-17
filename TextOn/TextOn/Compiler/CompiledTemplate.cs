using System;
using System.Collections.Generic;
using TextOn.Nouns;

namespace TextOn.Compiler
{
    /// <summary>
    /// Compiled representation of the template.
    /// </summary>
    public sealed class CompiledTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Compiler.CompiledTemplate"/> class.
        /// </summary>
        /// <param name="nouns">Nouns.</param>
        /// <param name="definition">Definition.</param>
        public CompiledTemplate (NounProfile nouns, CompiledNode definition)
        {
            Nouns = nouns;
            Definition = definition;
        }

        /// <summary>
        /// The user's noun profile.
        /// </summary>
        /// <value>The noun profile.</value>
        public NounProfile Nouns { get; private set; }

        /// <summary>
        /// The definition of the template.
        /// </summary>
        /// <value>The definition.</value>
        public CompiledNode Definition { get; private set; }
    }
}
