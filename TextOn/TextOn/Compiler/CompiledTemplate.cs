using System;
using System.Collections.Generic;

namespace TextOn.Compiler
{
    /// <summary>
    /// Compiled representation of the template.
    /// </summary>
    public sealed class CompiledTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Compiler.CompiledProject"/> class.
        /// </summary>
        /// <param name="variables">Variables.</param>
        /// <param name="definition">Definition.</param>
        public CompiledTemplate (IEnumerable<ICompiledVariable> variables, ICompiledNode definition)
        {
            var variableDefinitions = new Dictionary<string, ICompiledVariable> ();
            foreach (var item in variables) {
                variableDefinitions.Add (item.FullName, item);
            }
            VariableDefinitions = variableDefinitions;
            Definition = definition;
        }

        /// <summary>
        /// The user's variable definitions.
        /// </summary>
        /// <value>The variable definitions.</value>
        public IReadOnlyDictionary<string, ICompiledVariable> VariableDefinitions { get; private set; }

        /// <summary>
        /// The definition of the project.
        /// </summary>
        /// <value>The definition.</value>
        public ICompiledNode Definition { get; private set; }
    }
}
