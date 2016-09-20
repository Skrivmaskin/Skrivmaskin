using System;
namespace Skrivmaskin.Core.Compiled
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
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        T Choose<T> (Choice<T> choice);
    }
}
