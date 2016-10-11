using System;
namespace TextOn.Parsing
{
    public enum TextOnParseTokens
    {
        ChoiceStart,
        ChoiceDivide,
        ChoiceEnd,
        VarStart,
        VarName,
        VarEnd,
        Text,
        Error,
        InvalidCharacter,
        InvalidText
    }
}
