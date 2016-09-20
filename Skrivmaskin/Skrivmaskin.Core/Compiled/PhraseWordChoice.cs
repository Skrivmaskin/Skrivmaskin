using System;
namespace Skrivmaskin.Core.Compiled
{
    /// <summary>
    /// A choice of words or short phrases that may comprise this block of text.
    /// </summary>
    public sealed class PhraseWordChoice : Choice<string>, IBlockOfText
    {
    }
}
