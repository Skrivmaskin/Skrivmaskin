using System;
namespace TextOn.Parsing
{
    /// <summary>
    /// Describes an element of parsed text. For use in syntax highlighting.
    /// </summary>
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

        /// <summary>
        /// Gets the token for this element.
        /// </summary>
        /// <value>The token.</value>
        public TextOnParseTokens Token { get { return token; } }

        /// <summary>
        /// Gets the depth in a nested choice.
        /// </summary>
        /// <value>The choice depth.</value>
        public int ChoiceDepth { get { return choiceDepth; } }

        /// <summary>
        /// Gets the range within this line of text.
        /// </summary>
        /// <value>The range.</value>
        public TextOnParseRange Range { get { return range; } }

        public override string ToString ()
        {
            return string.Format ("[TextOnParseElement: Token={0}, ChoiceDepth={1}, Range={2}]", Token, ChoiceDepth, Range);
        }
    }
}
