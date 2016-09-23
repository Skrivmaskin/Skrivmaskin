using System;
using System.Collections.Generic;
using Skrivmaskin.Core.Compiler;

namespace Skrivmaskin.Core.Interfaces
{
    /// <summary>
    /// Make a random choice.
    /// </summary>
    /// <remarks>
    /// The contract is that the user must call one of the Begin... methods before asking for any choices to be made. At the end of processing,
    /// the user must call End().
    /// </remarks>
    public interface IRandomChooser
    {
        /// <summary>
        /// Begin random choosing.
        /// </summary>
        void Begin ();

        /// <summary>
        /// Begin random choosing with a given seed.
        /// </summary>
        /// <param name="seed">Seed.</param>
        void BeginWithSeed (int seed);

        /// <summary>
        /// Make a choice.
        /// </summary>
        /// <param name="choice">Choice.</param>
        ICompiledNode Choose (IReadOnlyList<ICompiledNode> choice);

        /// <summary>
        /// End random choosing.
        /// </summary>
        void End ();

        /// <summary>
        /// Gets the last seed used by the RNG.
        /// </summary>
        /// <value>The last seed.</value>
        int? LastSeed { get; }
    }
}
