using System;
using System.Collections.Generic;
using TextOn.Design;
using TextOn.Parsing;

namespace TextOn.Compiler
{
    /// <summary>
    /// Immutable Compiled Node.
    /// </summary>
    public sealed class CompiledNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Compiler.CompiledNode"/> class.
        /// </summary>
        /// <param name="nodeType">Node type.</param>
        /// <param name="location">Location.</param>
        /// <param name="hasErrors">If set to <c>true</c> has errors.</param>
        /// <param name="requiredNouns">Required nouns.</param>
        /// <param name="text">Text.</param>
        /// <param name="childNodes">Child nodes.</param>
        public CompiledNode (CompiledNodeType nodeType, DesignNode location, bool hasErrors, IEnumerable<string> requiredNouns, string text, CompiledNode[] childNodes, IEnumerable<TextOnParseElement> elements)
        {
            Type = nodeType;
            Location = location;
            HasErrors = hasErrors;
            RequiredNouns = requiredNouns;
            Text = text;
            ChildNodes = childNodes;
            Elements = elements;
        }

        /// <summary>
        /// The type of this compiled node. 
        /// </summary>
        /// <value>The type.</value>
        public CompiledNodeType Type { get; private set; }

        /// <summary>
        /// The location in the design tree of this item.
        /// </summary>
        /// <remarks>
        /// Will be null if this compiled tree is compiled for release.
        /// </remarks>
        /// <value>The location.</value>
        public DesignNode Location { get; private set; }

        /// <summary>
        /// Are there errors in or below this node?
        /// </summary>
        public bool HasErrors { get; private set; }

        /// <summary>
        /// Gets the required nouns.
        /// </summary>
        /// <value>The required nouns.</value>
        public IEnumerable<string> RequiredNouns { get; private set; }

        /// <summary>
        /// The text for this item.
        /// </summary>
        /// <value>The text.</value>
        public string Text { get; private set; }

        /// <summary>
        /// Ths children.
        /// </summary>
        /// <value>The children.</value>
        public CompiledNode[] ChildNodes { get; private set; }

        /// <summary>
        /// Gets the parse elements.
        /// </summary>
        /// <value>The elements.</value>
        public IEnumerable<TextOnParseElement> Elements { get; private set; }
    }
}
