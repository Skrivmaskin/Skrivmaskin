using System;
using System.Collections.Generic;

namespace Skrivmaskin.Compiler
{
    /// <summary>
    /// The user's design time project.
    /// </summary>
    public sealed class CompiledProject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Skrivmaskin.Compiler.CompiledProject"/> class.
        /// </summary>
        /// <param name="projectName">Project name.</param>
        /// <param name="variables">Variables.</param>
        /// <param name="definition">Definition.</param>
        public CompiledProject (string projectName, IEnumerable<ICompiledVariable> variables, ICompiledNode definition)
        {
            ProjectName = projectName;
            var variableDefinitions = new Dictionary<string, ICompiledVariable> ();
            foreach (var item in variables) {
                variableDefinitions.Add (item.FullName, item);
            }
            VariableDefinitions = variableDefinitions;
            Definition = definition;
        }

        /// <summary>
        /// The name of this project.
        /// </summary>
        /// <value>The name.</value>
        public string ProjectName { get; private set; }

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
