using System;
using Skrivmaskin.Core.Compiled;

namespace Skrivmaskin.Core.Interfaces
{
    /// <summary>
    /// Make a random choice.
    /// </summary>
    public interface IRandomChooser
    {
        /// <summary>
        /// Make the choice.
        /// </summary>
        /// <param name="choice">Choice.</param>
        ICompiledNode Choose (ChoiceCompiledNode choice);
    }
}
