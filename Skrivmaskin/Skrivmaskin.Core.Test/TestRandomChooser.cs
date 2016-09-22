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
        ChoiceCompiledNode simpleChoice;

        [SetUp]
        public void Setup ()
        {
            var text = "{Option1|Option2|Option3|Option4}";
            var designNode = new TextNode (text);
            var parser = new SkrivmaskinParser (new DefaultLexerSyntax ());
            ICompiledNode compiledNode;
            int firstErrorChar;
            string firstErrorMessage;
            var ok = parser.Compile (designNode, out compiledNode, out firstErrorChar, out firstErrorMessage);
            Assert.True (ok);
            simpleChoice = compiledNode as ChoiceCompiledNode;
            Assert.IsNotNull (simpleChoice);
        }

        [TearDown]
        public void Teardown ()
        {
            simpleChoice = null;
        }

        [Test]
        public void TestLastSeed ()
        {

            var rc = new RandomChooser ();

            // Choice 1
            rc.Begin ();
            var tn1 = rc.Choose (simpleChoice) as TextCompiledNode;
            Assert.IsNotNull (tn1);
            rc.End ();

            // Choice 2
            var lastSeed = rc.LastSeed;
            Assert.IsNotNull (lastSeed);
            rc.BeginWithSeed (lastSeed.Value);
            var tn2 = rc.Choose (simpleChoice) as TextCompiledNode;
            Assert.IsNotNull (tn2);
            rc.End ();

            // ReferenceEquals
            Assert.True (Object.ReferenceEquals (tn1, tn2));
        }
    
        [Test]
        public void TestLastSeedGivenSeed ()
        {

            var rc = new RandomChooser ();
            var inputSeed = 57;

            // Choice 1
            rc.BeginWithSeed (inputSeed);
            var tn1 = rc.Choose (simpleChoice) as TextCompiledNode;
            Assert.IsNotNull (tn1);
            rc.End ();

            // Choice 2
            var lastSeed = rc.LastSeed;
            Assert.AreEqual (inputSeed, lastSeed.Value);
            Assert.IsNotNull (lastSeed);
            rc.BeginWithSeed (lastSeed.Value);
            var tn2 = rc.Choose (simpleChoice) as TextCompiledNode;
            Assert.IsNotNull (tn2);
            rc.End ();

            Assert.AreEqual (inputSeed, lastSeed.Value);

            // ReferenceEquals
            Assert.True (Object.ReferenceEquals (tn1, tn2));
        }
    }
}
