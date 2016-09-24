using System;
namespace Skrivmaskin.Interfaces
{
    /// <summary>
    /// Variable substituter.
    /// </summary>
    /// <remarks>
    /// Implementations are expected to give the same value if asked for the same variable form, obviously.
    /// </remarks>
    public interface IVariableSubstituter
    {
        /// <summary>
        /// Perform the substitution.
        /// </summary>
        /// <param name="variableFullName">Variable full name.</param>
        string Substitute (string variableFullName);
    }
}
