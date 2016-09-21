using System;
using Skrivmaskin.Core.Compiled;
using Skrivmaskin.Core.Interfaces;

namespace Skrivmaskin.Core.Services
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
        public ICompiledNode Choose (ChoiceCompiledNode choice)
        {
            return choice.Choices [random.Next (0, choice.Choices.Count)];
        }
    }
}
