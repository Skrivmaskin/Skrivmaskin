using System;
using System.Collections.Generic;
using Skrivmaskin.Parsing;

namespace Skrivmaskin.Compiler
{
    public interface ICompiledText
    {
        /// <summary>
        /// Gets the elements included in this line of text.
        /// </summary>
        /// <value>The elements.</value>
        IEnumerable<SkrivmaskinParseElement> Elements { get; }
    }
}
