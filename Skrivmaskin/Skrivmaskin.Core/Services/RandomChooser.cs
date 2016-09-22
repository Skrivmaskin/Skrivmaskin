using System;
using Skrivmaskin.Core.Compiled;
using Skrivmaskin.Core.Interfaces;

namespace Skrivmaskin.Core.Service{
    /// <summary>
    /// Concrete random chooser, using a pseudo RNG.
    /// </summary>
    public sealed class RandomChooser : IRandomChooser
    {
        Random random = null;
        int? lastSeed = null;

        /// <summary>
        /// Gets the last seed.
        /// </summary>
        /// <remarks>
        /// Returns null if the RandomChooser has never run.
        /// </remarks>
        /// <value>The last seed.</value>
        public int? LastSeed {
            get {
                if (random != null) throw new ApplicationException ("Attempt to ask for the last seed while RandomChooser is active");
                return lastSeed;
            }
        }

        /// <summary>
        /// Begin a generation session.
        /// </summary>
        public void Begin ()
        {
            if (random != null) throw new ApplicationException ("Attempt to restart RandomChooser when it is already initialized");
            lastSeed = DateTime.Now.GetHashCode ();
            random = new Random (lastSeed.Value);
        }

        /// <summary>
        /// Begins a generation session with the specified seed.
        /// </summary>
        /// <param name="seed">Seed.</param>
        public void BeginWithSeed (int seed)
        {
            if (random != null) throw new ApplicationException ("Attempt to restart RandomChooser when it is already initialized");
            lastSeed = seed;
            random = new Random (lastSeed.Value);
        }

        /// <summary>
        /// Make the choice.
        /// </summary>
        /// <param name="choice">Choice.</param>
        public ICompiledNode Choose (ChoiceCompiledNode choice)
        {
            return choice.Choices [random.Next (0, choice.Choices.Count)];
        }

        /// <summary>
        /// End the current generation session.
        /// </summary>
        public void End ()
        {
            if (random == null) throw new ApplicationException ("Attempt to terminate RandomChooser before it has started");
            random = null;
        }
    }
}
