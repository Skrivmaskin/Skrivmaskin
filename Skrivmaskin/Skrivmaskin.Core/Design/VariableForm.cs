using System;
namespace Skrivmaskin.Core.Design
{
    /// <summary>
    /// Variable form. This represents a grammatical form that the parent variable may be in, e.g. plural, possessive etc, in case there are language
    /// specific adjustments to be made.
    /// </summary>
    public sealed class VariableForm
    {
        public VariableForm ()
        {
        }

        /// <summary>
        /// The name of the form.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Suggested value for this form of this variable.
        /// </summary>
        /// <value>The suggestion.</value>
        public string Suggestion { get; set; }
    }
}
