using System;
using System.Collections.Generic;
using TextOn.Version0;

namespace TextOn.Design
{
    /// <summary>
    /// The user's design time project.
    /// </summary>
    public sealed class TextOnTemplate : IEquatable<TextOnTemplate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Design.TextOnTemplate"/> class.
        /// </summary>
        /// <param name="variableDefinitions">Variable definitions.</param>
        /// <param name="definition">Definition.</param>
        public TextOnTemplate (IReadOnlyList<Variable> variableDefinitions, INode definition)
        {
            VariableDefinitions = variableDefinitions;
            Definition = definition;
            Migrate ();
            //TODO KILL.
        }

        /// <summary>
        /// Migrate this instance to the latest version. Called during deserialization.
        /// </summary>
        internal void Migrate ()
        {
            if (Version == 0) {
                Migrate_0_1 ();
            }
        }

        private void Migrate_0_1 ()
        {
            if (VariableDefinitions == null) VariableDefinitions = new List<Variable> ();
            Version = 1;
        }

        /// <summary>
        /// The user's variable definitions.
        /// </summary>
        /// <value>The variable definitions.</value>
        public IReadOnlyList<Variable> VariableDefinitions { get; private set; }
        public bool ShouldSerializeVariableDefinitions ()
        {
            return VariableDefinitions != null && VariableDefinitions.Count > 0;
        }

        /// <summary>
        /// The definition of the template.
        /// </summary>
        /// <value>The definition.</value>
        public INode Definition { get; private set; }

        public bool Equals (TextOnTemplate other)
        {
            if (this.VariableDefinitions.Count != other.VariableDefinitions.Count) return false;
            for (int i = 0; i < this.VariableDefinitions.Count; i++) {
                if (!this.VariableDefinitions [i].Equals (other.VariableDefinitions [i])) return false;
            }
            if (!this.Definition.Equals (other.Definition)) return false;
            return true;
        }

        /// <summary>
        /// Gets or sets the version for the storage format.
        /// </summary>
        /// <value>The version.</value>
        public int Version { get; set; }
    }
}
