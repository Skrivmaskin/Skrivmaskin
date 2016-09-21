using System;
namespace Skrivmaskin.Editor
{
    /// <summary>
    /// Represents the type of node in the outline view.
    /// </summary>
    public enum NodeType
    {
        Choice,
        Text,
        Comment,
        Variable,
        VariableForm,
        Sequential,
        Root
    }
}
