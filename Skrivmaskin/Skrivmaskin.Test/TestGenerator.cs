using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Skrivmaskin.Compiler;
using Skrivmaskin.Design;
using Skrivmaskin.Generation;
using Skrivmaskin.Interfaces;

namespace Skrivmaskin.Test
{
    [TestFixture]
    public class TestGenerator : TestGeneratorBase
    {
        Mock<IVariableSubstituter> mockVariableSubstituter;
        Mock<IRandomChooser> mockRandomChooser;
        SkrivmaskinGenerator generator;

        [SetUp]
        public void Setup ()
        {
            mockVariableSubstituter = new Mock<IVariableSubstituter> ();
            mockRandomChooser = new Mock<IRandomChooser> ();
            generator = new SkrivmaskinGenerator (mockRandomChooser.Object);
        }

        [TearDown]
        public void Teardown ()
        {
            mockVariableSubstituter = null;
            mockRandomChooser = null;
            generator = null;
        }

        [Test]
        public void TestSimpleText ()
        {
            var text = "Simple text";
            var definition = MakeSimpleText (text);
            var project = MakeProject (null, definition);
            var seed = 42;
            var generatedText = generator.GenerateWithSeed (project, mockVariableSubstituter.Object, seed);
            Assert.AreEqual (text, generatedText);
        }

        [Test]
        public void TestVariable ()
        {
            var varName = "MyVar";
            var varValue = "MyValue";
            mockVariableSubstituter.Setup ((vs) => vs.Substitute (varName)).Returns (varValue);
            var definition = MakeSimpleVariable (varName);
            var project = MakeProject (null, definition);
            var seed = 42;
            var generatedText = generator.GenerateWithSeed (project, mockVariableSubstituter.Object, seed);
            Assert.AreEqual (varValue, generatedText);
            mockRandomChooser.SetupGet ((rc) => rc.LastSeed).Returns (42);
            Assert.IsTrue (generator.CanRegenerate (project));
            mockVariableSubstituter.Verify ((vs) => vs.Substitute (It.IsAny<string> ()), Times.Once ());
        }

        [Test]
        public void TestChoice ()
        {
            var expectedText = "This is the expected text";
            var unexpectedText = "This would be less good";
            var notChosen1 = MakeSimpleText (unexpectedText);
            var notChosen2 = MakeSimpleText (unexpectedText);
            var notChosen3 = MakeSimpleText (unexpectedText);
            var chosen = MakeSimpleText (expectedText);
            var definition = MakeChoice (notChosen1, chosen, notChosen2, notChosen3);
            var project = MakeProject (null, definition);
            mockRandomChooser.Setup ((rc) => rc.Choose (definition.Choices)).Returns (chosen);
            var seed = 42;
            var generatedText = generator.GenerateWithSeed (project, mockVariableSubstituter.Object, seed);
            Assert.AreEqual (expectedText, generatedText);
            mockRandomChooser.Verify ((rc) => rc.Choose (It.IsAny<IReadOnlyList<ICompiledNode>> ()), Times.Once ());
        }

        [Test]
        public void TestConcat ()
        {
            var firstSentence = "This is the first sentence.";
            var spacing = " ";
            var secondSentence = "This is the second sentence.";
            var expectedText = firstSentence + spacing + secondSentence;
            var nodeFirstSentence = MakeSimpleText (firstSentence);
            var nodeSpacing = MakeSimpleText (spacing);
            var nodeSecondSentence = MakeSimpleText (secondSentence);
            var definition = MakeSequential (nodeFirstSentence, nodeSpacing, nodeSecondSentence);
            var project = MakeProject (null, definition);
            var seed = 42;
            var generatedText = generator.GenerateWithSeed (project, mockVariableSubstituter.Object, seed);
            Assert.AreEqual (expectedText, generatedText);
            mockRandomChooser.Verify ((rc) => rc.Choose (It.IsAny<IReadOnlyList<ICompiledNode>> ()), Times.Never ());
        }


    }
}
