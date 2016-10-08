using System;
namespace TextOn.Parsing
{
    public struct TextOnParseElement
    {
        readonly TextOnParseTokens token;
        readonly TextOnParseRange range;
        readonly int choiceDepth;
        public TextOnParseElement (TextOnParseTokens token, int choiceDepth, TextOnParseRange range)
        {
            this.token = token;
            this.choiceDepth = choiceDepth;
            this.range = range;
        }
        public TextOnParseTokens Token { get { return token; } }
        public int ChoiceDepth { get { return choiceDepth; } }
        public TextOnParseRange Range { get { return range; } }
        public override string ToString ()
        {
            return string.Format ("[TextOnParseElement: Token={0}, ChoiceDepth={1}, Range={2}]", Token, ChoiceDepth, Range);
        }
    }
}
