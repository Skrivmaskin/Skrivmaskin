using System;
namespace Skrivmaskin.Parsing
{
    public struct SkrivmaskinParseRange
    {
        readonly int startCharacter;
        readonly int endCharacter;

        public SkrivmaskinParseRange (int startCharacter, int endCharacter)
        {
            this.startCharacter = startCharacter;
            this.endCharacter = endCharacter;
        }

        public int StartCharacter { get { return startCharacter; } }

        public int EndCharacter { get { return endCharacter; } }

        public override string ToString ()
        {
            return string.Format ("[SkrivmaskinParseRange: StartCharacter={0}, EndCharacter={1}]", StartCharacter, EndCharacter);
        }
    }
}
