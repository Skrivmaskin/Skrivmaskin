using System;
namespace Skrivmaskin.Core
{
    /// <summary>
    /// Straight up raw text for this block of text.
    /// </summary>
    public sealed class PhraseText : IBlockOfText
    {
        /// <summary>
        /// The raw text.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; set; }
    }
}
