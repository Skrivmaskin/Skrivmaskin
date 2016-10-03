using System;
namespace Skrivmaskin.Parsing
{
    public struct SkrivmaskinParseElement
    {
        readonly SkrivmaskinParseTokens token;
        readonly SkrivmaskinParseRange range;
        public SkrivmaskinParseElement (SkrivmaskinParseTokens token, SkrivmaskinParseRange range)
        {
            this.token = token;
            this.range = range;
        }
        public SkrivmaskinParseTokens Token { get { return token; } }
        public SkrivmaskinParseRange Range { get { return range; } }
        public override string ToString ()
        {
            return string.Format ("[SkrivmaskinParseElement: Token={0}, Range={1}]", Token, Range);
        }
    }
}
