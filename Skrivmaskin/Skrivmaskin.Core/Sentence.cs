using System;
namespace Skrivmaskin.Core
{
    public sealed class Sentence
    {
        public Sentence ()
        {
        }

        public SentenceChoice [] Choices { get; set; }
    }
}
