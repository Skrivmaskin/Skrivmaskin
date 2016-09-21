using System;
using System.Collections.Generic;

namespace Skrivmaskin.Core.Compiled
{
    /// <summary>
    /// The user's design time project.
    /// </summary>
    public sealed class CompiledProject
    {
        /// <summary>
        /// The name of this project.
        /// </summary>
        /// <value>The name.</value>
        public string ProjectName { get; set; }

        /// <summary>
        /// The user's variable definitions.
        /// </summary>
        /// <value>The variable definitions.</value>
        public Dictionary<string, CompiledVariable> VariableDefinitions { get; set; }

        /// <summary>
        /// The definition of the project.
        /// </summary>
        /// <value>The definition.</value>
        public ICompiledNode Definition { get; set; }
    }
}
