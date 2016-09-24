using System;
using System.Linq;
using NUnit.Framework;
using Skrivmaskin.Compiler;
using Skrivmaskin.Design;
using Skrivmaskin.Lexing;
using Skrivmaskin.Parsing;

namespace Skrivmaskin.Test
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
            var compiledNode = parser.Compile (designNode) as TextCompiledNode;
            Assert.IsNotNull (compiledNode);
            var cn = compiledNode as TextCompiledNode;
            Assert.AreEqual (1, compiledNode.StartCharacter.Value);
            Assert.AreEqual (11, compiledNode.EndCharacter.Value);
            Assert.AreEqual (designNode, compiledNode.Location);
            Assert.AreEqual (inputText, cn.Text);
        }

        [Test]
        public void TestCompileSimpleVariable ()
        {
            var inputText = "[SimpleVariable]";
            var designNode = new TextNode () { Text = inputText };
            var compiledNode = parser.Compile (designNode) as VariableCompiledNode;
            Assert.IsNotNull (compiledNode);
        }

        [Test]
        public void TestCompileCompoundVariable ()
        {
            var inputText = "[Compound|Variable]";
            var designNode = new TextNode () { Text = inputText };
            var compiledNode = parser.Compile (designNode) as VariableCompiledNode;
            Assert.IsNotNull (compiledNode);
        }

        [Test]
        public void TestTwoTextChoices ()
        {
            var inputText = "{Hello|World} {World|Hello}";
            var designNode = new TextNode () { Text = inputText };
            var compiledNode = parser.Compile (designNode)as SequentialCompiledNode;
            Assert.IsNotNull (compiledNode);
            Assert.AreEqual (1, compiledNode.StartCharacter);
            Assert.AreEqual (27, compiledNode.EndCharacter);
            var sq = compiledNode;
            Assert.AreEqual (3, sq.Sequential.Count);
            var cn = sq.Sequential [0] as ChoiceCompiledNode;
            Assert.IsNotNull (cn);
            Assert.AreEqual (2, cn.Choices.Count);
            var tn = cn.Choices [0] as TextCompiledNode;
            Assert.IsNotNull (tn);
            Assert.AreEqual ("Hello", tn.Text);
            tn = cn.Choices [1] as TextCompiledNode;
            Assert.IsNotNull (tn);
            Assert.AreEqual ("World", tn.Text);
            tn = sq.Sequential [1] as TextCompiledNode;
            Assert.IsNotNull (tn);
            Assert.AreEqual (" ", tn.Text);
            cn = sq.Sequential [2] as ChoiceCompiledNode;
            Assert.IsNotNull (cn);
            Assert.AreEqual (2, cn.Choices.Count);
            tn = cn.Choices [0] as TextCompiledNode;
            Assert.IsNotNull (tn);
            Assert.AreEqual ("World", tn.Text);
            tn = cn.Choices [1] as TextCompiledNode;
            Assert.IsNotNull (tn);
            Assert.AreEqual ("Hello", tn.Text);
        }

        [Test]
        public void TestDiacriticsInText ()
        {
            var inputText = "När du ska hyra bil i [MÄRKE] gör du det snabbt och enkelt via oss på Sixt.";
            var designNode = new TextNode () { Text = inputText };
            var compiledNode = parser.Compile (designNode) as SequentialCompiledNode;
            Assert.IsNotNull (compiledNode);
        }

        [Test]
        public void TestDiacriticsInVariable ()
        {
            var inputText = "[MÄRKE|MÄRKE_Variant]";
            var designNode = new TextNode () { Text = inputText };
            var compiledNode = parser.Compile (designNode) as VariableCompiledNode;
            Assert.IsNotNull (compiledNode);
        }
    }
}
