using System;
using System.Collections.Generic;

namespace Skrivmaskin.Design
{
    /// <summary>
    /// The user's design time project.
    /// </summary>
    public sealed class Project : IEquatable<Project>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Skrivmaskin.Design.Project"/> class.
        /// </summary>
        /// <param name="variableDefinitions">Variable definitions.</param>
        /// <param name="definition">Definition.</param>
        public Project (IReadOnlyList<Variable> variableDefinitions, INode definition)
        {
            VariableDefinitions = variableDefinitions;
            Definition = definition;
        }

        /// <summary>
        /// The user's variable definitions.
        /// </summary>
        /// <value>The variable definitions.</value>
        public IReadOnlyList<Variable> VariableDefinitions { get; private set; }

        /// <summary>
        /// The definition of the project.
        /// </summary>
        /// <value>The definition.</value>
        public INode Definition { get; private set; }

        public bool Equals (Project other)
        {
            if (this.VariableDefinitions.Count != other.VariableDefinitions.Count) return false;
            for (int i = 0; i < this.VariableDefinitions.Count; i++) {
                if (!this.VariableDefinitions [i].Equals (other.VariableDefinitions [i])) return false;
            }
            if (!this.Definition.Equals (other.Definition)) return false;
            return true;
        }
    }
}
