using System;
using System.Linq;
using NUnit.Framework;
using TextOn.Compiler;
using TextOn.Design;
using TextOn.Lexing;
using TextOn.Parsing;

namespace TextOn.Test
{
    [TestFixture]
    public class TestTextOnParser
    {
        private readonly TextOnParser parser = new TextOnParser (new DefaultLexerSyntax ());

        [Test]
        public void TestCompileText ()
        {
            var inputText = "Hello world";
            var designNode = new TextNode () { Text = inputText };
            var compiledNode = parser.Compile (designNode) as SuccessCompiledNode;
            Assert.IsNotNull (compiledNode);
            var cn = compiledNode.Node as TextCompiledNode;
            Assert.IsNotNull (cn);
            Assert.AreEqual (designNode, compiledNode.Location);
            Assert.AreEqual (inputText, cn.Text);
        }

        [Test]
        public void TestCompileSimpleVariable ()
        {
            var inputText = "[SimpleVariable]";
            var designNode = new TextNode () { Text = inputText };
            var compiledNode = parser.Compile (designNode) as SuccessCompiledNode;
            Assert.IsNotNull (compiledNode);
            var variableNode = compiledNode.Node as VariableCompiledNode;
            Assert.IsNotNull (variableNode);
        }

        [Test]
        public void TestTwoTextChoices ()
        {
            var inputText = "{Hello|World} {World|Hello}";
            var designNode = new TextNode () { Text = inputText };
            var compiledNode = parser.Compile (designNode) as SuccessCompiledNode;
            Assert.IsNotNull (compiledNode);
            var sq = compiledNode.Node as SequentialCompiledNode;
            Assert.IsNotNull (sq);
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
            var compiledNode = parser.Compile (designNode) as SuccessCompiledNode;
            Assert.IsNotNull (compiledNode);
            var tn = compiledNode.Node as SequentialCompiledNode;
            Assert.IsNotNull (tn);
        }

        [Test]
        public void TestDiacriticsInVariable ()
        {
            var inputText = "[MÄRKE_Variant]";
            var designNode = new TextNode () { Text = inputText };
            var compiledNode = parser.Compile (designNode) as SuccessCompiledNode;
            Assert.IsNotNull (compiledNode);
            var vn = compiledNode.Node as VariableCompiledNode;
            Assert.IsNotNull (vn);
        }
    }
}
