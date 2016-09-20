using System;
namespace Skrivmaskin.Core.Compiled
{
    /// <summary>
    /// Concrete random chooser, using a pseudo RNG.
    /// </summary>
    public sealed class RandomChooser : IRandomChooser
    {
        readonly Random random;
        public RandomChooser (Random random)
        {
            this.random = random;
        }

        /// <summary>
        /// Make the choice.
        /// </summary>
        /// <param name="choice">Choice.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public T Choose<T> (Choice<T> choice)
        {
            return choice.Choices [random.Next (0, choice.Choices.Length)];
        }
    }
}
