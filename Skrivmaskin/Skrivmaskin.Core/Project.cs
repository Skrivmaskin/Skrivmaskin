using System;

namespace Skrivmaskin.Core
{
    /// <summary>
    /// A Skrivmaskin project.
    /// </summary>
    public sealed class Project : Concat<Paragraph>
    {
        /// <summary>
        /// The variables that can be used in this project.
        /// </summary>
        /// <value>The variables.</value>
        public Variable [] Variables { get; set; }

    }
}
