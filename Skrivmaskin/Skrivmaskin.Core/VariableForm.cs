using System;
namespace Skrivmaskin.Core
{
    /// <summary>
    /// The grammatical form (e.g. plural or possessive) in which a variable is allowed to appear.
    /// </summary>
    /// <remarks>
    /// Due to differences between languages, we leave it entirely up to the user what the names of these forms are, it is also allowed to vary between variables, because
    /// (again) languages differ here, some vary adjectives based on gender or plural etc etc.
    /// </remarks>
    public sealed class VariableForm
    {
        /// <summary>
        /// The parent variable.
        /// </summary>
        /// <value>The variable.</value>
        public Variable Variable { get; set; }

        /// <summary>
        /// The user's name for this variable form.
        /// </summary>
        /// <value>The name.</value>
        /// <remarks>
        /// This may be null or empty, i.e. the root form of this word.
        /// </remarks>
        public string Name { get; set; }
    }
}
