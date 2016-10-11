using System;
using System.Collections.Generic;

namespace TextOn.Version0
{
    /// <summary>
    /// A user variable. This sets up a random replacement in the runner.
    /// </summary>
    internal sealed class Variable : IEquatable<Variable>
    {
        public Variable ()
        {
        }

        /// <summary>
        /// The user name for this variable.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// User's description for this variable.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }

        /// <summary>
        /// Grammatical forms in which this variable may appear. One special form is defined with the empty string,
        /// which is the default form.
        /// </summary>
        /// <value>The forms.</value>
        public List<VariableForm> Forms { get; set; }

        public bool Equals (Variable other)
        {
            if (this.Name != other.Name) return false;
            if (this.Description != other.Description) return false;
            if (this.Forms.Count != other.Forms.Count) return false;
            for (int i = 0; i < this.Forms.Count; i++) {
                if (!this.Forms [i].Equals (other.Forms [i])) return false;
            }
            return true;
        }
    }
}
