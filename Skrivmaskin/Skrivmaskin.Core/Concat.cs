using System;
namespace Skrivmaskin.Core
{
    /// <summary>
    /// A generic concatenation of multiple language elements.
    /// </summary>
    public class Concat<T>
    {
        /// <summary>
        /// Gets or sets the elements.
        /// </summary>
        /// <value>The elements.</value>
        public T [] Elements { get; set; }
    }
}
