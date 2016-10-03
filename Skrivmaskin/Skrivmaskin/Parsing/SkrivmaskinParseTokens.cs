using System;
namespace Skrivmaskin.Parsing
{
    public enum SkrivmaskinParseTokens
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
