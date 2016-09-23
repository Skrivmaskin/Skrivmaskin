﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Skrivmaskin.Core.Compiler;
using Skrivmaskin.Core.Design;
using Skrivmaskin.Core.Generation;
using Skrivmaskin.Core.Interfaces;

namespace Skrivmaskin.Core.Test
{
    [TestFixture]
    public class TestGenerator
    {
        [Test]
        public void TestGenerateSimpleText ()
        {
            var mockVariableSubstituter = new Mock<IVariableSubstituter> ();
            var mockRandomChooser = new Mock<IRandomChooser> ();
            var project = new CompiledProject ();
            project.ProjectName = "SimpleText";
            project.VariableDefinitions = new Dictionary<string, ICompiledVariable> ();
            var text = "Hello world";
            var mockDesignNode = new Mock<INode> ();
            project.Definition = new TextCompiledNode (text, mockDesignNode.Object, 1, 11);
            var generator = new SkrivmaskinGenerator (project, mockRandomChooser.Object);
            var seed = 42;
            var generatedText = generator.GenerateWithSeed (seed, mockVariableSubstituter.Object);
            Assert.AreEqual (text, generatedText);
        }

        [Test]
        public void TestGenerateVariable ()
        {
            var mockVariableSubstituter = new Mock<IVariableSubstituter> ();
            var varName = "MyVar";
            var varValue = "MyValue";
            mockVariableSubstituter.Setup ((vs) => vs.Substitute (varName)).Returns (varValue);
            var mockRandomChooser = new Mock<IRandomChooser> ();
            var project = new CompiledProject ();
            project.ProjectName = "Variable";
            project.VariableDefinitions = new Dictionary<string, ICompiledVariable> ();
            var mockDesignNode = new Mock<INode> ();
            project.Definition = new VariableCompiledNode (varName, mockDesignNode.Object, 2, 6);
            var generator = new SkrivmaskinGenerator (project, mockRandomChooser.Object);
            var seed = 42;
            var generatedText = generator.GenerateWithSeed (seed, mockVariableSubstituter.Object);
            Assert.AreEqual (varValue, generatedText);
            mockVariableSubstituter.Verify ((vs) => vs.Substitute (It.IsAny<string> ()), Times.Once ());
        }

        [Test]
        public void TestGenerateChoice ()
        {
            var mockVariableSubstituter = new Mock<IVariableSubstituter> ();
            var mockRandomChooser = new Mock<IRandomChooser> ();
            var expectedText = "Hello world";
            var unexpectedText = "Something else";
            var mockDesignNode = new Mock<INode> ();
            var chosen = new TextCompiledNode (expectedText, mockDesignNode.Object, 17, 27);
            var notChosen1 = new TextCompiledNode (unexpectedText, mockDesignNode.Object, 1, 15);
            var notChosen2 = new TextCompiledNode (unexpectedText, mockDesignNode.Object, 29, 45);
            var notChosen3 = new TextCompiledNode (unexpectedText, mockDesignNode.Object, 47, 64);
            var choices = new List<ICompiledNode> ();
            choices.Add (notChosen1);
            choices.Add (chosen);
            choices.Add (notChosen2);
            choices.Add (notChosen3);
            mockRandomChooser.Setup ((rc) => rc.Choose (choices)).Returns (chosen);
            var project = new CompiledProject ();
            project.ProjectName = "Choice";
            project.VariableDefinitions = new Dictionary<string, ICompiledVariable> ();
            project.Definition = new ChoiceCompiledNode (choices, mockDesignNode.Object, 2, 6);
            var generator = new SkrivmaskinGenerator (project, mockRandomChooser.Object);
            var seed = 42;
            var generatedText = generator.GenerateWithSeed (seed, mockVariableSubstituter.Object);
            Assert.AreEqual (expectedText, generatedText);
            mockRandomChooser.Verify ((rc) => rc.Choose (It.IsAny<IReadOnlyList<ICompiledNode>> ()), Times.Once ());
        }
    }
}
