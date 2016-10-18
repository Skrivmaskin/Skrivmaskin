using System;
namespace TextOn.Parsing
{
    /// <summary>
    /// A character range in a single line of text.
    /// </summary>
    public struct TextOnParseRange
    {
        readonly int startCharacter;
        readonly int endCharacter;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Parsing.TextOnParseRange"/> struct.
        /// </summary>
        /// <param name="startCharacter">Start character.</param>
        /// <param name="endCharacter">End character.</param>
        public TextOnParseRange (int startCharacter, int endCharacter)
        {
            this.startCharacter = startCharacter;
            this.endCharacter = endCharacter;
        }

        /// <summary>
        /// Gets the start character. 0 indexed.
        /// </summary>
        /// <value>The start character.</value>
        public int StartCharacter { get { return startCharacter; } }

        /// <summary>
        /// Gets the end character.
        /// </summary>
        /// <value>The end character.</value>
        public int EndCharacter { get { return endCharacter; } }

        public override string ToString ()
        {
            return string.Format ("[TextOnParseRange: StartCharacter={0}, EndCharacter={1}]", StartCharacter, EndCharacter);
        }
    }
}
