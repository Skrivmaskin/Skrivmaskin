using System;
namespace TextOn.Design
{
    /// <summary>
    /// Storage representation of a node in the calculation graph.
    /// </summary>
    public interface INode : IEquatable<INode>
    {
        /// <summary>
        /// Gets the type of this node in the design tree.
        /// </summary>
        /// <value>The type.</value>
        NodeType Type { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:TextOn.Design.INode"/> is active.
        /// </summary>
        /// <value><c>true</c> if is active; otherwise, <c>false</c>.</value>
        bool IsActive { get; }
    }
}
