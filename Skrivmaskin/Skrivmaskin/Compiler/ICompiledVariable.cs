using System;
namespace Skrivmaskin.Compiler
{
    /// <summary>
    /// A compiled variable.
    /// </summary>
    public interface ICompiledVariable : IEquatable<ICompiledVariable>
    {
        /// <summary>
        /// The root name of this variable.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        /// The user's form of this variable. This represents a particular variant of this variable, and can be used to handle language specific
        /// variations, or stylistic variations (use of pronouns) or other complexities that only the user can get right.
        /// </summary>
        /// <remarks>
        /// Every variable will have one entry with the empty string here. This is the root form of this variable.
        /// </remarks>
        /// <value>The name of the form.</value>
        string FormName { get; }

        /// <summary>
        /// The user's description of this variable. This is stored in order to provide it back to the user when the user defines the valu        /// at run time.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; }

        /// <summary>
        /// The user's sample suggestion for this variable value.
        /// </summary>
        /// <value>The suggestion.</value>
        string Suggestion { get; }

        /// <summary>
        /// The full name of this variable, including form (if necessary).
        /// </summary>
        /// <value>The full name.</value>
        string FullName { get; }
    }
}
