using System;
using Skrivmaskin.Interfaces;

namespace Skrivmaskin.Services
{
    /// <summary>
    /// Compiler config that inserts one space between sentences and uses UNIX line endings.
    /// </summary>
    public sealed class SingleSpaceUnixGeneratorConfig : IGeneratorConfig
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
