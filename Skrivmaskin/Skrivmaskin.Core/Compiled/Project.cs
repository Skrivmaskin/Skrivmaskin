using System;

namespace Skrivmaskin.Core.Compiled
{
    /// <summary>
    /// A Skrivmaskin project.
    /// </summary>
    public sealed class Project : Concat<Paragraph>
    {
        //TODO OPS This is all well and good for the runtime, but this is not the right storage format. Sort it out.

        /// <summary>
        /// The variables that can be used in this project.
        /// </summary>
        /// <value>The variables.</value>
        public Variable [] Variables { get; set; }

    }
}
