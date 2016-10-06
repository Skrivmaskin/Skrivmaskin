using System;
using Skrivmaskin.Design;
namespace Skrivmaskin.Generation
{
    /// <summary>
    /// Annotates generated text with the design node that is responsible.
    /// </summary>
    public sealed class AnnotatedText
    {
        internal static AnnotatedText Blank = new AnnotatedText (null, "");

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Skrivmaskin.Generation.AnnotatedText"/> class.
        /// </summary>
        /// <param name="designNode">Design node.</param>
        /// <param name="text">Text.</param>
        internal AnnotatedText (INode designNode, string text)
        {
            DesignNode = designNode;
            Text = text;
        }

        /// <summary>
        /// Gets the design node that was responsible for this text.
        /// </summary>
        /// <value>The design node.</value>
        public INode DesignNode { get; private set; }

        /// <summary>
        /// Gets the output text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; private set;}
    }
}
