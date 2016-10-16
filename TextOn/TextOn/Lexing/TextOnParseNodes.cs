using System;
namespace TextOn.Lexing
{
    internal enum TextOnParseNodes
    {
        Text,
        Escape,
        VarName,
        VarForm,
        CompoundText,
        Phrase,
        Noun,
        Sentence,
        SimpleChoice,
        MultiChoice,
        Choice,
        OrOp,
        Anything
    }
}
