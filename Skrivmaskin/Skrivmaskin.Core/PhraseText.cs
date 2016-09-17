using System;
namespace Skrivmaskin.Core
{
    public sealed class RawText : IBlockOfText
    {
        public RawText ()
        {
        }

        public string Text { get; set; }
    }
}
