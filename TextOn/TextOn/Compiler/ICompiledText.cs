using System;
using System.Collections.Generic;
using TextOn.Parsing;

namespace TextOn.Compiler
{
    public interface ICompiledText
    {
        /// <summary>
        /// Gets the elements included in this line of text.
        /// </summary>
        /// <value>The elements.</value>
        IEnumerable<TextOnParseElement> Elements { get; }
    }
}
