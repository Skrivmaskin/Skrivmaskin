using System;
namespace Skrivmaskin.Core.Lexing
{
    /// <summary>
    /// All the possible constructs when parsing a short phrase.
    /// </summary>
    public enum TokenType
    {
        Text,
        VariableName,
        MultipleChoice,
        Choice
    }
}
