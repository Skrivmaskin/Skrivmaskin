using System;
namespace Skrivmaskin.Core
{
    public class PhraseWordChoice : IBlockOfText
    {
        public PhraseWordChoice ()
        {
        }

        public string [] Choices { get; set; }
    }
}
