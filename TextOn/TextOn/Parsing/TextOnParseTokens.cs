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
        VarDivide,
        VarFormName,
        VarEnd,
        Text,
        Error,
        InvalidCharacter,
        InvalidText
    }
}
