using System;
namespace TextOn.Interfaces
{
    /// <summary>
    /// Provides the generator with formatting information to replace special instructions with text.
    /// </summary>
    public interface IGeneratorConfig
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
