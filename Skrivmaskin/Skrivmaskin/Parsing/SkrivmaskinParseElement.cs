using System;
namespace Skrivmaskin.Parsing
{
    public struct SkrivmaskinParseElement
    {
        readonly SkrivmaskinParseTokens token;
        readonly SkrivmaskinParseRange range;
        readonly int choiceDepth;
        public SkrivmaskinParseElement (SkrivmaskinParseTokens token, int choiceDepth, SkrivmaskinParseRange range)
        {
            this.token = token;
            this.choiceDepth = choiceDepth;
            this.range = range;
        }
        public SkrivmaskinParseTokens Token { get { return token; } }
        public int ChoiceDepth { get { return choiceDepth; } }
        public SkrivmaskinParseRange Range { get { return range; } }
        public override string ToString ()
        {
            return string.Format ("[SkrivmaskinParseElement: Token={0}, ChoiceDepth={1}, Range={2}]", Token, ChoiceDepth, Range);
        }
    }
}
