using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;
using Skrivmaskin.Compiler;
using Skrivmaskin.Design;
using Skrivmaskin.Lexing;
using Moq;
using Skrivmaskin.Interfaces;
using Skrivmaskin.Generation;

namespace Skrivmaskin.Test
{
    [TestFixture]
    public class TestScenario
    {
        private static string namespaceName = "Skrivmaskin.Test";
        private SkrivmaskinCompiler compiler;
        private SkrivmaskinGenerator generator;
        private Mock<IRandomChooser> mockRandomChooser;
        private Mock<IVariableSubstituter> mockVariableSubstituter;

        [SetUp]
        public void Setup ()
        {
            compiler = new SkrivmaskinCompiler (new DefaultLexerSyntax ());
            mockRandomChooser = new Mock<IRandomChooser> ();
            mockVariableSubstituter = new Mock<IVariableSubstituter> ();
            generator = new SkrivmaskinGenerator (mockRandomChooser.Object);
        }

        [TearDown]
        public void Teardown ()
        {
            compiler = null;
            mockRandomChooser = null;
            mockVariableSubstituter = null;
            generator = null;
        }

        [Test]
        public void TestProject1 ()
        {
            var assembly = Assembly.GetExecutingAssembly ();
            var resourceName = namespaceName + "." + "Scenario.Project1.json";
            var serializer = new JsonSerializer ();
            serializer.Formatting = Formatting.Indented;
            foreach (var converter in ProjectWriter.JsonConverters) {
                serializer.Converters.Add (converter);
            }
            using (Stream stream = assembly.GetManifestResourceStream (resourceName)) {
                using (StreamReader reader = new StreamReader (stream)) {
                    var project = (Project)serializer.Deserialize (reader, typeof (Project));
                    var stopwatch = new System.Diagnostics.Stopwatch ();
                    stopwatch.Start ();
                    var compiledProject = compiler.Compile (project);
                    stopwatch.Stop ();
                    Assert.IsNotNull (compiledProject);
                    Assert.AreEqual (3, compiledProject.Definition.RequiredVariables.Count ());
                }
            }
        }
    }
}
