using System;
using System.Collections.Generic;
using Skrivmaskin.Core.Design;

namespace Skrivmaskin.Core.Compiled
{
    /// <summary>
    /// Represents a compiled node, with meta data for debug compilation.
    /// </summary>
    /// <remarks>
    /// The references back to the Design tree in debug mode can be used to aid the user in correcting malformed generated text.
    /// </remarks>
    public interface ICompiledNode
    {
        /// <summary>
        /// The type of this compiled node. 
        /// </summary>
        /// <value>The type.</value>
        CompiledNodeType Type { get; }

        /// <summary>
        /// The location in the design tree of this item.
        /// </summary>
        /// <remarks>
        /// Will be null if this compiled tree is compiled for release.
        /// </remarks>
        /// <value>The location.</value>
        INode Location { get; }

        /// <summary>
        /// The start character in the line of this item.
        /// </summary>
        /// <remarks>
        /// Will be null if this compiled tree is compiled for release.
        /// </remarks>
        /// <value>The start character.</value>
        int? StartCharacter { get; }

        /// <summary>
        /// The end character in the line of this item.
        /// </summary>
        /// <remarks>
        /// Will be null if this compiled tree is compiled for release.
        /// </remarks>
        /// <value>The end character.</value>
        int? EndCharacter { get; }

        /// <summary>
        /// Are there errors in or below this node?
        /// </summary>
        bool HasErrors { get; }
    }
}
