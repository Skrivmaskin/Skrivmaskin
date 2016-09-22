using System;
using NUnit.Framework;
using Skrivmaskin.Core.Compiled;
using Skrivmaskin.Core.Design;
using Skrivmaskin.Core.Lexing;
using Skrivmaskin.Core.Parsing;

namespace Skrivmaskin.Core.Test
{
    [TestFixture]
    public class TestSkrivmaskinParser
    {
        private readonly SkrivmaskinParser parser = new SkrivmaskinParser (new DefaultLexerSyntax ());

        [Test]
        public void TestCompileText ()
        {
            var inputText = "Hello world";
            var designNode = new TextNode () { Text = inputText };
            ICompiledNode compiledNode;
            int firstErrorChar;
            string firstErrorMessage;
            var ok = parser.Compile (designNode, out compiledNode, out firstErrorChar, out firstErrorMessage);
            Assert.True (ok);
        }

        [Test]
        public void TestCompileSimpleVariable ()
        {
            var inputText = "[SimpleVariable]";
            var designNode = new TextNode () { Text = inputText };
            ICompiledNode compiledNode;
            int firstErrorChar;
            string firstErrorMessage;
            var ok = parser.Compile (designNode, out compiledNode, out firstErrorChar, out firstErrorMessage);
            Assert.True (ok);
        }

        [Test]
        public void TestCompileCompoundVariable ()
        {
            var inputText = "[Compound|Variable]";
            var designNode = new TextNode () { Text = inputText };
            ICompiledNode compiledNode;
            int firstErrorChar;
            string firstErrorMessage;
            var ok = parser.Compile (designNode, out compiledNode, out firstErrorChar, out firstErrorMessage);
            Assert.True (ok);
        }

        [Test]
        public void TestTwoTextChoices ()
        {
            var inputText = "{Hello|World}";
            var designNode = new TextNode () { Text = inputText };
            ICompiledNode compiledNode;
            int firstErrorChar;
            string firstErrorMessage;
            var ok = parser.Compile (designNode, out compiledNode, out firstErrorChar, out firstErrorMessage);
            Assert.True (ok);
        }

    }
}
