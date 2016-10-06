using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Skrivmaskin.Generation
{
    /// <summary>
    /// Annotated output from the generator.
    /// </summary>
    public sealed class AnnotatedOutput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Skrivmaskin.Generation.AnnotatedOutput"/> class.
        /// </summary>
        /// <param name="text">Text.</param>
        internal AnnotatedOutput (IReadOnlyList<AnnotatedText> text)
        {
            Text = text;
        }

        /// <summary>
        /// Gets the annotated text components.
        /// </summary>
        /// <value>The text.</value>
        public IReadOnlyList<AnnotatedText> Text { get; private set; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Skrivmaskin.Generation.AnnotatedOutput"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Skrivmaskin.Generation.AnnotatedOutput"/>.</returns>
        public override string ToString ()
        {
            var builder = new StringBuilder ();
            foreach (var text in Text) {
                builder.Append (text.Text);
            }
            return builder.ToString ();
        }
    }
}
