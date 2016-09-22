using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using Skrivmaskin.Core.Compiled;
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
            var mockGeneratorConfig = new Mock<IGeneratorConfig> ();
            var project = new CompiledProject ();
            project.ProjectName = "SimpleText";
            project.VariableDefinitions = new Dictionary<string, ICompiledVariable> ();
            var text = "Hello world";
            var mockDesignNode = new Mock<INode> ();
            project.Definition = TextCompiledNode.Make (text, mockDesignNode.Object, 1, 11);
            var generator = new SkrivmaskinGenerator (project, mockRandomChooser.Object, mockGeneratorConfig.Object);
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
            var mockGeneratorConfig = new Mock<IGeneratorConfig> ();
            var project = new CompiledProject ();
            project.ProjectName = "Variable";
            project.VariableDefinitions = new Dictionary<string, ICompiledVariable> ();
            var mockDesignNode = new Mock<INode> ();
            project.Definition = VariableCompiledNode.Make (varName, mockDesignNode.Object, 2, 6);
            var generator = new SkrivmaskinGenerator (project, mockRandomChooser.Object, mockGeneratorConfig.Object);
            var seed = 42;
            var generatedText = generator.GenerateWithSeed (seed, mockVariableSubstituter.Object);
            Assert.AreEqual (varValue, generatedText);
            mockVariableSubstituter.Verify ((vs) => vs.Substitute (It.IsAny<string> ()), Times.Once ());
        }
    
        [Test]
        public void TestGenerateChoice ()
        {
            var mockVariableSubstituter = new Mock<IVariableSubstituter> ();
            var varName = "MyVar";
            var varValue = "MyValue";
            mockVariableSubstituter.Setup ((vs) => vs.Substitute (varName)).Returns (varValue);
            var mockRandomChooser = new Mock<IRandomChooser> ();
            var mockGeneratorConfig = new Mock<IGeneratorConfig> ();
            var project = new CompiledProject ();
            project.ProjectName = "Variable";
            project.VariableDefinitions = new Dictionary<string, ICompiledVariable> ();
            var mockDesignNode = new Mock<INode> ();
            project.Definition = VariableCompiledNode.Make (varName, mockDesignNode.Object, 2, 6);
            var generator = new SkrivmaskinGenerator (project, mockRandomChooser.Object, mockGeneratorConfig.Object);
            var seed = 42;
            var generatedText = generator.GenerateWithSeed (seed, mockVariableSubstituter.Object);
            Assert.AreEqual (varValue, generatedText);
            mockVariableSubstituter.Verify ((vs) => vs.Substitute (It.IsAny<string> ()), Times.Once ());
        }
}
}
