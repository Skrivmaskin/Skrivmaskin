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
            int choiceDepth = 0;
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.ChoiceStart, choiceDepth, new SkrivmaskinParseRange (0, 0)));
            ++choiceDepth;
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, choiceDepth, new SkrivmaskinParseRange (1, 5)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.ChoiceDivide, choiceDepth, new SkrivmaskinParseRange (6, 6)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, choiceDepth, new SkrivmaskinParseRange (7, 8)));
            --choiceDepth;
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.ChoiceEnd, choiceDepth, new SkrivmaskinParseRange (9, 9)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, choiceDepth, new SkrivmaskinParseRange (10, 10)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarStart, choiceDepth, new SkrivmaskinParseRange (11, 11)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarName, choiceDepth, new SkrivmaskinParseRange (12, 16)));
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
            int choiceDepth = 0;
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.ChoiceStart, choiceDepth, new SkrivmaskinParseRange (0, 0)));
            ++choiceDepth;
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, choiceDepth, new SkrivmaskinParseRange (1, 5)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.ChoiceDivide, choiceDepth, new SkrivmaskinParseRange (6, 6)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, choiceDepth, new SkrivmaskinParseRange (7, 8)));
            --choiceDepth;
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.ChoiceEnd, choiceDepth, new SkrivmaskinParseRange (9, 9)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, choiceDepth, new SkrivmaskinParseRange (10, 10)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarStart, choiceDepth, new SkrivmaskinParseRange (11, 11)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarName, choiceDepth, new SkrivmaskinParseRange (12, 16)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarDivide, choiceDepth, new SkrivmaskinParseRange (17, 17)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarFormName, choiceDepth, new SkrivmaskinParseRange (18, 21)));
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
            int choiceDepth = 0;
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, choiceDepth, new SkrivmaskinParseRange (0, 11)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.ChoiceStart, choiceDepth, new SkrivmaskinParseRange (12, 12)));
            ++choiceDepth;
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, choiceDepth, new SkrivmaskinParseRange (13, 28)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.ChoiceDivide, choiceDepth, new SkrivmaskinParseRange (29, 29)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, choiceDepth, new SkrivmaskinParseRange (30, 39)));
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
            int choiceDepth = 0;
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, choiceDepth, new SkrivmaskinParseRange (0, 11)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarStart, choiceDepth, new SkrivmaskinParseRange (12, 12)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.InvalidCharacter, choiceDepth, new SkrivmaskinParseRange (13, 13)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.InvalidText, choiceDepth, new SkrivmaskinParseRange (14, 27)));
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
            int choiceDepth = 0;
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, choiceDepth, new SkrivmaskinParseRange (0, 11)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarStart, choiceDepth, new SkrivmaskinParseRange (12, 12)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarName, choiceDepth, new SkrivmaskinParseRange (13, 16)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.InvalidText, choiceDepth, new SkrivmaskinParseRange (17, 25)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.InvalidText, choiceDepth, new SkrivmaskinParseRange (26, 26)));
            Assert.IsNotNull (errorNode);
            var elements = errorNode.Elements.ToList ();
            Assert.AreEqual (expected.Count, elements.Count);
            for (int i = 0; i < elements.Count; i++) {
                Assert.AreEqual (expected [i], elements [i]);
            }
        }

        [Test]
        public void TestSuccessTextElements ()
        {
            var compiler = new SkrivmaskinCompiler (new DefaultLexerSyntax ());
            var successNode = compiler.CompileText ("Hello there, what is going on?") as SuccessCompiledNode;
            var expected = new List<SkrivmaskinParseElement> ();
            int choiceDepth = 0;
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, choiceDepth, new SkrivmaskinParseRange (0, 29)));
            Assert.IsNotNull (successNode);
            var elements = successNode.Elements.ToList ();
            Assert.AreEqual (expected.Count, elements.Count);
            for (int i = 0; i < elements.Count; i++) {
                Assert.AreEqual (expected [i], elements [i]);
            }
        }

        [Test]
        public void TestSuccessVariableElements ()
        {
            var compiler = new SkrivmaskinCompiler (new DefaultLexerSyntax ());
            var successNode = compiler.CompileText ("Hello there, [QUESTION]?") as SuccessCompiledNode;
            var expected = new List<SkrivmaskinParseElement> ();
            int choiceDepth = 0;
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, choiceDepth, new SkrivmaskinParseRange (0, 12)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarStart, choiceDepth, new SkrivmaskinParseRange (13, 13)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarName, choiceDepth, new SkrivmaskinParseRange (14, 21)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.VarEnd, choiceDepth, new SkrivmaskinParseRange (22, 22)));
            expected.Add (new SkrivmaskinParseElement (SkrivmaskinParseTokens.Text, choiceDepth, new SkrivmaskinParseRange (23, 23)));
            Assert.IsNotNull (successNode);
            var elements = successNode.Elements.ToList ();
            Assert.AreEqual (expected.Count, elements.Count);
            for (int i = 0; i < elements.Count; i++) {
                Assert.AreEqual (expected [i], elements [i]);
            }
        }
    }
}
