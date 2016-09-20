using System;
using System.Collections.Generic;

namespace Skrivmaskin.Core.Design
{
    /// <summary>
    /// A user variable. This sets up a random replacement in the runner.
    /// </summary>
    public sealed class Variable
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
    }
}
