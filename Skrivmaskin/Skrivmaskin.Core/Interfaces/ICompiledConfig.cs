using System;
namespace Skrivmaskin.Core.Interfaces
{
    /// <summary>
    /// Provides the compiler with formatting information to replace special instructions with text.
    /// </summary>
    public interface ICompilerConfig
    {
        /// <summary>
        /// Spacing between sentences.
        /// </summary>
        /// <value>The spacing.</value>
        string Spacing { get; }

        /// <summary>
        /// Spacing between paragraphs.
        /// </summary>
        /// <value>The paragraph break.</value>
        string ParagraphBreak { get; }
    }
}
