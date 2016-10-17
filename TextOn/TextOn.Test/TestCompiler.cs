using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using TextOn.Compiler;
using TextOn.Lexing;
using TextOn.Parsing;
using TextOn.Design;
using TextOn.Version0;
using TextOn.Nouns;
using System.Text.RegularExpressions;

namespace TextOn.Test
{
    [TestFixture]
    public class TestCompiler : TestCompilerBase
    {
        [Test]
        public void TestErrorIncompleteVariable ()
        {
            var compiler = new TextOnCompiler (new DefaultLexerSyntax ());
            var errorNode = compiler.CompileText ("{Hello|Hi} [HELLO");
            Assert.AreEqual (CompiledNodeType.Error, errorNode.Type);
            var expected = new List<TextOnParseElement> ();
            int choiceDepth = 0;
            expected.Add (new TextOnParseElement (TextOnParseTokens.ChoiceStart, choiceDepth, new TextOnParseRange (0, 0)));
            ++choiceDepth;
            expected.Add (new TextOnParseElement (TextOnParseTokens.Text, choiceDepth, new TextOnParseRange (1, 5)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.ChoiceDivide, choiceDepth, new TextOnParseRange (6, 6)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.Text, choiceDepth, new TextOnParseRange (7, 8)));
            --choiceDepth;
            expected.Add (new TextOnParseElement (TextOnParseTokens.ChoiceEnd, choiceDepth, new TextOnParseRange (9, 9)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.Text, choiceDepth, new TextOnParseRange (10, 10)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.VarStart, choiceDepth, new TextOnParseRange (11, 11)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.VarName, choiceDepth, new TextOnParseRange (12, 16)));
            Assert.IsNotNull (errorNode);
            var elements = errorNode.Elements.ToList ();
            Assert.AreEqual (expected.Count, elements.Count);
            for (int i = 0; i < elements.Count; i++) {
                Assert.AreEqual (expected [i], elements [i]);
            }
        }

        [Test, Ignore ("Currently there is a problem with the RegularExpresison terminal")]
        public void TestSuccessWithPowerCharacter ()
        {
            var compiler = new TextOnCompiler (new DefaultLexerSyntax ());
            var successNode = compiler.CompileText ("This is ^ some text");
            Assert.IsNotNull (successNode);
            Assert.AreEqual (CompiledNodeType.Success, successNode.Type);
        }

        [Test]
        public void TestIronyRegex ()
        {
            var regexString = TextOnTextTerminal.MakeRegex (new DefaultLexerSyntax (), true);
            var regex = new Regex (regexString);
            var match = regex.Match ("This is ^ some text");
            Assert.IsTrue (match.Success);
        }

        [Test]
        public void TestErrorIncompleteVariableForm ()
        {
            var compiler = new TextOnCompiler (new DefaultLexerSyntax ());
            var errorNode = compiler.CompileText ("{Hello|Hi} [HELLO|Blah");
            Assert.AreEqual (CompiledNodeType.Error, errorNode.Type);
            var expected = new List<TextOnParseElement> ();
            int choiceDepth = 0;
            expected.Add (new TextOnParseElement (TextOnParseTokens.ChoiceStart, choiceDepth, new TextOnParseRange (0, 0)));
            ++choiceDepth;
            expected.Add (new TextOnParseElement (TextOnParseTokens.Text, choiceDepth, new TextOnParseRange (1, 5)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.ChoiceDivide, choiceDepth, new TextOnParseRange (6, 6)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.Text, choiceDepth, new TextOnParseRange (7, 8)));
            --choiceDepth;
            expected.Add (new TextOnParseElement (TextOnParseTokens.ChoiceEnd, choiceDepth, new TextOnParseRange (9, 9)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.Text, choiceDepth, new TextOnParseRange (10, 10)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.VarStart, choiceDepth, new TextOnParseRange (11, 11)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.VarName, choiceDepth, new TextOnParseRange (12, 16)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.InvalidCharacter, choiceDepth, new TextOnParseRange (17, 17)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.InvalidText, choiceDepth, new TextOnParseRange (18, 21)));
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
            var compiler = new TextOnCompiler (new DefaultLexerSyntax ());
            var errorNode = compiler.CompileText ("Hello there {what is going on|how are yo");
            Assert.AreEqual (CompiledNodeType.Error, errorNode.Type);
            var expected = new List<TextOnParseElement> ();
            int choiceDepth = 0;
            expected.Add (new TextOnParseElement (TextOnParseTokens.Text, choiceDepth, new TextOnParseRange (0, 11)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.ChoiceStart, choiceDepth, new TextOnParseRange (12, 12)));
            ++choiceDepth;
            expected.Add (new TextOnParseElement (TextOnParseTokens.Text, choiceDepth, new TextOnParseRange (13, 28)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.ChoiceDivide, choiceDepth, new TextOnParseRange (29, 29)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.Text, choiceDepth, new TextOnParseRange (30, 39)));
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
            var compiler = new TextOnCompiler (new DefaultLexerSyntax ());
            var errorNode = compiler.CompileText ("Hello there [{what is going]");
            Assert.AreEqual (CompiledNodeType.Error, errorNode.Type);
            var expected = new List<TextOnParseElement> ();
            int choiceDepth = 0;
            expected.Add (new TextOnParseElement (TextOnParseTokens.Text, choiceDepth, new TextOnParseRange (0, 11)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.VarStart, choiceDepth, new TextOnParseRange (12, 12)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.InvalidCharacter, choiceDepth, new TextOnParseRange (13, 13)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.InvalidText, choiceDepth, new TextOnParseRange (14, 27)));
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
            var compiler = new TextOnCompiler (new DefaultLexerSyntax ());
            var errorNode = compiler.CompileText ("Hello there [what is going]");
            Assert.AreEqual (CompiledNodeType.Error, errorNode.Type);
            var expected = new List<TextOnParseElement> ();
            int choiceDepth = 0;
            expected.Add (new TextOnParseElement (TextOnParseTokens.Text, choiceDepth, new TextOnParseRange (0, 11)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.VarStart, choiceDepth, new TextOnParseRange (12, 12)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.VarName, choiceDepth, new TextOnParseRange (13, 16)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.InvalidText, choiceDepth, new TextOnParseRange (17, 25)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.InvalidText, choiceDepth, new TextOnParseRange (26, 26)));
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
            var compiler = new TextOnCompiler (new DefaultLexerSyntax ());
            var successNode = compiler.CompileText ("Hello there, what is going on?");
            Assert.AreEqual (CompiledNodeType.Success, successNode.Type);
            var expected = new List<TextOnParseElement> ();
            int choiceDepth = 0;
            expected.Add (new TextOnParseElement (TextOnParseTokens.Text, choiceDepth, new TextOnParseRange (0, 29)));
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
            var compiler = new TextOnCompiler (new DefaultLexerSyntax ());
            var successNode = compiler.CompileText ("Hello there, [QUESTION]?");
            Assert.AreEqual (CompiledNodeType.Success, successNode.Type);
            var expected = new List<TextOnParseElement> ();
            int choiceDepth = 0;
            expected.Add (new TextOnParseElement (TextOnParseTokens.Text, choiceDepth, new TextOnParseRange (0, 12)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.VarStart, choiceDepth, new TextOnParseRange (13, 13)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.VarName, choiceDepth, new TextOnParseRange (14, 21)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.VarEnd, choiceDepth, new TextOnParseRange (22, 22)));
            expected.Add (new TextOnParseElement (TextOnParseTokens.Text, choiceDepth, new TextOnParseRange (23, 23)));
            Assert.IsNotNull (successNode);
            var elements = successNode.Elements.ToList ();
            Assert.AreEqual (expected.Count, elements.Count);
            for (int i = 0; i < elements.Count; i++) {
                Assert.AreEqual (expected [i], elements [i]);
            }
        }

        [Test]
        public void TestInactiveParagraphBreak ()
        {
            var compiler = new TextOnCompiler (new DefaultLexerSyntax ());
            var template = new TextOnTemplate (new NounProfile (), new ParagraphBreakNode () { IsActive = false });
            var compiledTemplate = compiler.Compile (template);
            Assert.AreEqual (CompiledNodeType.Blank, compiledTemplate.Definition.Type);
        }

        [Test]
        public void TestInactiveText ()
        {
            var compiler = new TextOnCompiler (new DefaultLexerSyntax ());
            var template = new TextOnTemplate (new NounProfile (), new TextNode () { Text = "Hello world", IsActive = false });
            var compiledTemplate = compiler.Compile (template);
            Assert.AreEqual (CompiledNodeType.Blank, compiledTemplate.Definition.Type);
        }

        [Test]
        public void TextInactiveChoice ()
        {
            var compiler = new TextOnCompiler (new DefaultLexerSyntax ());
            var text1 = new TextNode ("ABC", true);
            var text2 = new TextNode ("DEF", true);
            var template = new TextOnTemplate (new NounProfile (), new ChoiceNode ("Some choice", false, new List<INode> (new INode [] { text1, text2 })));
            var compiledTemplate = compiler.Compile (template);
            Assert.AreEqual (CompiledNodeType.Blank, compiledTemplate.Definition.Type);
        }

        [Test]
        public void TextInactiveSequential ()
        {
            var compiler = new TextOnCompiler (new DefaultLexerSyntax ());
            var text1 = new TextNode ("ABC", true);
            var text2 = new TextNode ("DEF", true);
            var template = new TextOnTemplate (new NounProfile (), new SequentialNode ("Some sequential", false, new List<INode> (new INode [] { text1, text2 })));
            var compiledTemplate = compiler.Compile (template);
            Assert.AreEqual (CompiledNodeType.Blank, compiledTemplate.Definition.Type);
        }
    }
}
