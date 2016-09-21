using System;
using NUnit.Framework;
using Skrivmaskin.Core.Lexing;
using Skrivmaskin.Core.Parsing;

namespace Skrivmaskin.Core.Test
{
    [TestFixture]
    public class TestSkrivmaskinGrammar
    {
        private readonly SkrivmaskinParser parser = new SkrivmaskinParser (new DefaultLexerSyntax ());

        [Test]
        public void TestText ()
        {
            var parseTree = parser.ParseToTree ("blah blah blah");
            Assert.False (parseTree.HasErrors ());
        }

        [Test]
        public void TestEscapedText ()
        {
            var parseTree = parser.ParseToTree (@"blah blah \{blah");
            Assert.False (parseTree.HasErrors ());
        }

        [Test]
        public void TestOr ()
        {
            var parseTree = parser.ParseToTree ("{blah blah blah|Hello}");
            Assert.False (parseTree.HasErrors ());
        }

        [Test]
        public void TestSimpleVariable ()
        {
            var parseTree = parser.ParseToTree ("[Variable]");
            Assert.False (parseTree.HasErrors ());
        }

        [Test]
        public void TestCompoundVariable ()
        {
            var parseTree = parser.ParseToTree ("[Variable|Form]");
            Assert.False (parseTree.HasErrors ());
        }

        [Test]
        public void TestSentenceWithCompoundVariable ()
        {
            var parseTree = parser.ParseToTree ("I would like some [Variable|Form] please");
            Assert.False (parseTree.HasErrors ());
        }

    }
}
