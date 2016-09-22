using System;
namespace Skrivmaskin.Core.Lexing
{
    internal enum SkrivmaskinTokens
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
