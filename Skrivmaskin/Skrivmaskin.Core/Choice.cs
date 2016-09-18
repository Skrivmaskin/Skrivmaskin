using System;
namespace Skrivmaskin.Core
{
    /// <summary>
    /// A generic choice between multiple language elements.
    /// </summary>
    public class Choice<T>
    {
        /// <summary>
        /// Gets or sets the choices.
        /// </summary>
        /// <value>The choices.</value>
        public T [] Choices { get; set; }
    }
}
