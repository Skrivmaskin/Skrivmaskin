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
            var designNode = new DesignNode (NodeType.Text, true, inputText, new DesignNode [0]);
            var compiledNode = parser.Compile (designNode);
            Assert.IsNotNull (compiledNode);
            Assert.AreEqual (CompiledNodeType.Success, compiledNode.Type);
            Assert.AreEqual (1, compiledNode.ChildNodes.Length);
            var cn = compiledNode.ChildNodes [0];
            Assert.IsNotNull (cn);
            Assert.AreEqual (CompiledNodeType.Text, cn.Type);
            Assert.AreEqual (designNode, compiledNode.Location);
            Assert.AreEqual (inputText, cn.Text);
        }

        [Test]
        public void TestCompileSimpleVariable ()
        {
            var inputText = "[SimpleVariable]";
            var designNode = new DesignNode (NodeType.Text, true, inputText, new DesignNode [0]);
            var compiledNode = parser.Compile (designNode);
            Assert.IsNotNull (compiledNode);
            Assert.AreEqual (CompiledNodeType.Success, compiledNode.Type);
            Assert.AreEqual (1, compiledNode.ChildNodes.Length);
            var variableNode = compiledNode.ChildNodes [0];
            Assert.IsNotNull (variableNode);
            Assert.AreEqual (CompiledNodeType.Variable, variableNode.Type);
            Assert.AreEqual ("SimpleVariable", variableNode.Text);
        }

        [Test]
        public void TestTwoTextChoices ()
        {
            var inputText = "{Hello|World} {World|Hello}";
            var designNode = new DesignNode (NodeType.Text, true, inputText, new DesignNode [0]);
            var compiledNode = parser.Compile (designNode);
            Assert.IsNotNull (compiledNode);
            Assert.AreEqual (CompiledNodeType.Success, compiledNode.Type);
            Assert.AreEqual (1, compiledNode.ChildNodes.Length);
            var sq = compiledNode.ChildNodes [0];
            Assert.IsNotNull (sq);
            Assert.AreEqual (CompiledNodeType.Sequential, sq.Type);
            Assert.AreEqual (3, sq.ChildNodes.Length);
            var cn = sq.ChildNodes [0];
            Assert.IsNotNull (cn);
            Assert.AreEqual (CompiledNodeType.Choice, cn.Type);
            Assert.AreEqual (2, cn.ChildNodes.Length);
            var tn = cn.ChildNodes [0];
            Assert.IsNotNull (tn);
            Assert.AreEqual (CompiledNodeType.Text, tn.Type);
            Assert.AreEqual ("Hello", tn.Text);
            tn = cn.ChildNodes [1];
            Assert.IsNotNull (tn);
            Assert.AreEqual (CompiledNodeType.Text, tn.Type);
            Assert.AreEqual ("World", tn.Text);
            tn = sq.ChildNodes [1];
            Assert.IsNotNull (tn);
            Assert.AreEqual (CompiledNodeType.Text, tn.Type);
            Assert.AreEqual (" ", tn.Text);
            cn = sq.ChildNodes [2];
            Assert.IsNotNull (cn);
            Assert.AreEqual (CompiledNodeType.Choice, cn.Type);
            Assert.AreEqual (2, cn.ChildNodes.Length);
            tn = cn.ChildNodes [0];
            Assert.IsNotNull (tn);
            Assert.AreEqual (CompiledNodeType.Text, tn.Type);
            Assert.AreEqual ("World", tn.Text);
            tn = cn.ChildNodes [1];
            Assert.IsNotNull (tn);
            Assert.AreEqual (CompiledNodeType.Text, tn.Type);
            Assert.AreEqual ("Hello", tn.Text);
        }

        [Test]
        public void TestDiacriticsInText ()
        {
            var inputText = "När du ska hyra bil i [MÄRKE] gör du det snabbt och enkelt via oss på Sixt.";
            var designNode = new DesignNode (NodeType.Text, true, inputText, new DesignNode [0]);
            var compiledNode = parser.Compile (designNode);
            Assert.IsNotNull (compiledNode);
            Assert.AreEqual (CompiledNodeType.Success, compiledNode.Type);
            Assert.AreEqual (1, compiledNode.ChildNodes.Length);
            var cn = compiledNode.ChildNodes [0];
            Assert.IsNotNull (cn);
            Assert.AreEqual (CompiledNodeType.Sequential, cn.Type);
            Assert.AreEqual (designNode, compiledNode.Location);
        }

        [Test]
        public void TestDiacriticsInVariable ()
        {
            var inputText = "[MÄRKE_Variant]";
            var designNode = new DesignNode (NodeType.Text, true, inputText, new DesignNode [0]);
            var compiledNode = parser.Compile (designNode);
            Assert.IsNotNull (compiledNode);
            Assert.AreEqual (CompiledNodeType.Success, compiledNode.Type);
            Assert.AreEqual (1, compiledNode.ChildNodes.Length);
            var variableNode = compiledNode.ChildNodes [0];
            Assert.IsNotNull (variableNode);
            Assert.AreEqual (CompiledNodeType.Variable, variableNode.Type);
            Assert.AreEqual ("MÄRKE_Variant", variableNode.Text);
        }
    }
}
