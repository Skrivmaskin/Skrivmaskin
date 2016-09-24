using System;
namespace Skrivmaskin.Design
{
    /// <summary>
    /// Storage representation of a node in the calculation graph.
    /// </summary>
    public interface INode : IEquatable<INode>
    {
        NodeType Type { get; }
    }
}