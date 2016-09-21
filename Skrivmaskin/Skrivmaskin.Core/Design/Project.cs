using System;
using System.Collections.Generic;

namespace Skrivmaskin.Core.Design
{
    /// <summary>
    /// The user's design time project.
    /// </summary>
    public sealed class Project : IEquatable<Project>
    {
        public Project ()
        {
        }

        /// <summary>
        /// The name of this project.
        /// </summary>
        /// <value>The name.</value>
        public string ProjectName { get; set; }

        /// <summary>
        /// The user's variable definitions.
        /// </summary>
        /// <value>The variable definitions.</value>
        public List<Variable> VariableDefinitions { get; set; }

        /// <summary>
        /// The definition of the project.
        /// </summary>
        /// <value>The definition.</value>
        public INode Definition { get; set; }

        public bool Equals (Project other)
        {
            if (this.ProjectName != other.ProjectName) return false;
            if (this.VariableDefinitions.Count != other.VariableDefinitions.Count) return false;
            for (int i = 0; i < this.VariableDefinitions.Count; i++) {
                if (!this.VariableDefinitions [i].Equals (other.VariableDefinitions [i])) return false;
            }
            if (!this.Definition.Equals (other.Definition)) return false;
            return true;
        }
    }
}
