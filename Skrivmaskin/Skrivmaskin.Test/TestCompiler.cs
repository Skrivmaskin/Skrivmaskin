using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using Skrivmaskin.Compiler;
using Skrivmaskin.Lexing;
using Skrivmaskin.Parsing;

namespace Skrivmaskin.Test
{
    [TestFixture]
    public class TestCompiler : TestCompilerBase
    {
        [Test]
        public void TestErrorIncompleteVariable ()
        {
            var compiler = new SkrivmaskinCompiler (new DefaultLexerSyntax ());
            var errorNode = compiler.CompileText ("{Hello|Hi} [HELLO") as ErrorCompiledNode;
            var expected = new List<SkrivmaskinParseElement> ();
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.ChoiceStart, new SkrivmaskinParseRange (0, 0)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, new SkrivmaskinParseRange (1, 5)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.ChoiceDivide, new SkrivmaskinParseRange (6, 6)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, new SkrivmaskinParseRange (7, 8)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.ChoiceEnd, new SkrivmaskinParseRange (9, 9)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, new SkrivmaskinParseRange (10, 10)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarStart, new SkrivmaskinParseRange (11, 11)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarName, new SkrivmaskinParseRange (12, 16)));
            Assert.IsNotNull (errorNode);
            var elements = errorNode.Elements.ToList ();
            Assert.AreEqual (expected.Count, elements.Count);
            for (int i = 0; i < elements.Count; i++) {
                Assert.AreEqual (expected [i], elements [i]);
            }
        }

        [Test]
        public void TestErrorIncompleteVariableForm ()
        {
            var compiler = new SkrivmaskinCompiler (new DefaultLexerSyntax ());
            var errorNode = compiler.CompileText ("{Hello|Hi} [HELLO|Blah") as ErrorCompiledNode;
            var expected = new List<SkrivmaskinParseElement> ();
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.ChoiceStart, new SkrivmaskinParseRange (0, 0)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, new SkrivmaskinParseRange (1, 5)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.ChoiceDivide, new SkrivmaskinParseRange (6, 6)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, new SkrivmaskinParseRange (7, 8)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.ChoiceEnd, new SkrivmaskinParseRange (9, 9)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, new SkrivmaskinParseRange (10, 10)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarStart, new SkrivmaskinParseRange (11, 11)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarName, new SkrivmaskinParseRange (12, 16)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarDivide, new SkrivmaskinParseRange (17, 17)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarFormName, new SkrivmaskinParseRange (18, 21)));
            Assert.IsNotNull (errorNode);
            var elements = errorNode.Elements.ToList ();
            Assert.AreEqual (expected.Count, elements.Count);
            for (int i = 0; i < elements.Count; i++) {
                Assert.AreEqual (expected [i], elements [i]);
            }
        }

        [Test]
        public void TestErrorIncompleteChoice ()
        {
            var compiler = new SkrivmaskinCompiler (new DefaultLexerSyntax ());
            var errorNode = compiler.CompileText ("Hello there {what is going on|how are yo") as ErrorCompiledNode;
            var expected = new List<SkrivmaskinParseElement> ();
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, new SkrivmaskinParseRange (0, 11)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.ChoiceStart, new SkrivmaskinParseRange (12, 12)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, new SkrivmaskinParseRange (13, 28)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.ChoiceDivide, new SkrivmaskinParseRange (29, 29)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, new SkrivmaskinParseRange (30, 39)));
            Assert.IsNotNull (errorNode);
            var elements = errorNode.Elements.ToList ();
            Assert.AreEqual (expected.Count, elements.Count);
            for (int i = 0; i < elements.Count; i++) {
                Assert.AreEqual (expected [i], elements [i]);
            }
        }

        [Test]
        public void TestErrorChoiceInvalidVariableNameChoice ()
        {
            var compiler = new SkrivmaskinCompiler (new DefaultLexerSyntax ());
            var errorNode = compiler.CompileText ("Hello there [{what is going]") as ErrorCompiledNode;
            var expected = new List<SkrivmaskinParseElement> ();
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, new SkrivmaskinParseRange (0, 11)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarStart, new SkrivmaskinParseRange (12, 12)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.InvalidCharacter, new SkrivmaskinParseRange (13, 13)));
            Assert.IsNotNull (errorNode);
            var elements = errorNode.Elements.ToList ();
            Assert.AreEqual (expected.Count, elements.Count);
            for (int i = 0; i < elements.Count; i++) {
                Assert.AreEqual (expected [i], elements [i]);
            }
        }

        [Test]
        public void TestErrorChoiceInvalidVariableNameSpace ()
        {
            var compiler = new SkrivmaskinCompiler (new DefaultLexerSyntax ());
            var errorNode = compiler.CompileText ("Hello there [what is going]") as ErrorCompiledNode;
            var expected = new List<SkrivmaskinParseElement> ();
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, new SkrivmaskinParseRange (0, 11)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarStart, new SkrivmaskinParseRange (12, 12)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarName, new SkrivmaskinParseRange (13, 16)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.InvalidText, new SkrivmaskinParseRange (17, 25)));
            Assert.IsNotNull (errorNode);
            var elements = errorNode.Elements.ToList ();
            Assert.AreEqual (expected.Count, elements.Count);
            for (int i = 0; i < elements.Count; i++) {
                Assert.AreEqual (expected [i], elements [i]);
            }
        }
    }
}
