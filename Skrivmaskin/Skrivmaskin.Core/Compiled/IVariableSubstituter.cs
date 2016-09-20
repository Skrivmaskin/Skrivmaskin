using System;
namespace Skrivmaskin.Core.Compiled
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
        /// <param name="variable">Variable.</param>
        string Substitute (VariableForm variable);
    }
}
