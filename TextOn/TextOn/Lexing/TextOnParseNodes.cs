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
        SimpleVariable,
        CompoundVariable,
        Variable,
        Sentence,
        SimpleChoice,
        MultiChoice,
        Choice,
        OrOp,
        Anything
    }
}
