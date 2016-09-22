using System;
using NUnit.Framework;
using Skrivmaskin.Core.Compiled;
using Skrivmaskin.Core.Design;
using Skrivmaskin.Core.Lexing;
using Skrivmaskin.Core.Parsing;
using Skrivmaskin.Core.Service;

namespace Skrivmaskin.Core.Test
{
    [TestFixture]
    public class TestRandomChooser
    {
        [Test]
        public void TestLastSeed ()
        {
            var rc = new RandomChooser ();
            var text = "{Option1|Option2|Option3|Option4}";
            var designNode = new TextNode (text);
            var parser = new SkrivmaskinParser (new DefaultLexerSyntax ());
            ICompiledNode compiledNode;
            int firstErrorChar;
            string firstErrorMessage;
            var ok = parser.Compile (designNode, out compiledNode, out firstErrorChar, out firstErrorMessage);
            Assert.True (ok);
            var cn = compiledNode as ChoiceCompiledNode;
            Assert.IsNotNull (cn);

            // Choice 1
            rc.Begin ();
            var tn1 = rc.Choose (cn) as TextCompiledNode;
            Assert.IsNotNull (tn1);
            rc.End ();

            // Choice 2
            var lastSeed = rc.LastSeed;
            Assert.IsNotNull (lastSeed);
            rc.BeginWithSeed (lastSeed.Value);
            var tn2 = rc.Choose (cn) as TextCompiledNode;
            Assert.IsNotNull (tn2);
            rc.End ();

            // ReferenceEquals
            Assert.True (Object.ReferenceEquals (tn1, tn2));
        }
    }
}
