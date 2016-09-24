using System;
using NUnit.Framework;
using Skrivmaskin.Compiler;
using Skrivmaskin.Design;
using Skrivmaskin.Lexing;
using Skrivmaskin.Parsing;
using Skrivmaskin.Service;

namespace Skrivmaskin.Test
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
            simpleChoice = parser.Compile (designNode) as ChoiceCompiledNode;
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
            var tn1 = rc.Choose (simpleChoice.Choices) as TextCompiledNode;
            Assert.IsNotNull (tn1);
            rc.End ();

            // Choice 2
            var lastSeed = rc.LastSeed;
            Assert.IsNotNull (lastSeed);
            rc.BeginWithSeed (lastSeed.Value);
            var tn2 = rc.Choose (simpleChoice.Choices) as TextCompiledNode;
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
            var tn1 = rc.Choose (simpleChoice.Choices) as TextCompiledNode;
            Assert.IsNotNull (tn1);
            rc.End ();

            // Choice 2
            var lastSeed = rc.LastSeed;
            Assert.AreEqual (inputSeed, lastSeed.Value);
            Assert.IsNotNull (lastSeed);
            rc.BeginWithSeed (lastSeed.Value);
            var tn2 = rc.Choose (simpleChoice.Choices) as TextCompiledNode;
            Assert.IsNotNull (tn2);
            rc.End ();

            Assert.AreEqual (inputSeed, lastSeed.Value);

            // ReferenceEquals
            Assert.True (Object.ReferenceEquals (tn1, tn2));
            // One I checked earlier.
            Assert.AreEqual ("Option3", tn1.Text);
            Assert.AreEqual ("Option3", tn2.Text);
        }
    }
}