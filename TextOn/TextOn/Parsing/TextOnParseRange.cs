using System;
namespace TextOn.Parsing
{
    public struct TextOnParseRange
    {
        readonly int startCharacter;
        readonly int endCharacter;

        public TextOnParseRange (int startCharacter, int endCharacter)
        {
            this.startCharacter = startCharacter;
            this.endCharacter = endCharacter;
        }

        public int StartCharacter { get { return startCharacter; } }

        public int EndCharacter { get { return endCharacter; } }

        public override string ToString ()
        {
            return string.Format ("[TextOnParseRange: StartCharacter={0}, EndCharacter={1}]", StartCharacter, EndCharacter);
        }
    }
}
