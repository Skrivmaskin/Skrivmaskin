using System;
using Skrivmaskin.Core.Interfaces;

namespace Skrivmaskin.Core.Services
{
    /// <summary>
    /// Generator config that inserts one space between sentences.
    /// </summary>
    public sealed class SingleSpaceGeneratorConfig : IGeneratorConfig
    {
        /// <summary>
        /// Two new lines using Unix line endings.
        /// </summary>
        /// <value>The paragraph break.</value>
        public string ParagraphBreak {
            get {
                return "\n\n";
            }
        }

        /// <summary>
        /// Just the one space.
        /// </summary>
        /// <value>The spacing.</value>
        public string Spacing {
            get {
                return " ";
            }
        }
    }
}
