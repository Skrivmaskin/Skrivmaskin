using System;
using TextOn.Design;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using TextOn.Nouns;

namespace TextOn.Test
{
    [TestFixture]
    public class TestTemplateWriter
    {
        private static IDictionary<TestTemplates, Tuple<TextOnTemplate, string>> projects = TestTemplateUtils.SetupProjects ();

        public void TestWrite (TestTemplates testProject)
        {
            var testCase = projects [testProject];
            var project = testCase.Item1;
            var expectedText = testCase.Item2;
            using (var stringWriter = new StringWriter ()) {
                TemplateWriter.Write (stringWriter, project);
                var actualText = stringWriter.ToString ();
                Assert.AreEqual (expectedText, actualText);
            }
        }

        [Test]
        public void TestWriteEmpty ()
        {
            TestWrite (TestTemplates.Empty);
        }

        [Test]
        public void TestWriteParagraphBreak ()
        {
            TestWrite (TestTemplates.ParagraphBreak);
        }

        public void TestRead (TestTemplates testProject)
        {
            var testCase = projects [testProject];
            var expectedTemplate = testCase.Item1;
            var text = testCase.Item2;
            using (var stringReader = new StringReader (text)) {
                var actualTemplate = TemplateWriter.Read (stringReader);
                Assert.AreEqual (expectedTemplate.Definition, actualTemplate.Definition);
                AssertNounProfilesAreEqual (expectedTemplate.Nouns, actualTemplate.Nouns);
                Assert.AreEqual (expectedTemplate.VariableDefinitions, actualTemplate.VariableDefinitions);
                Assert.AreEqual (expectedTemplate.Version, actualTemplate.Version);
            }
        }

        public void AssertNounProfilesAreEqual (NounProfile expected, NounProfile actual)
        {
        }

        [Test]
        public void TestReadEmpty ()
        {
            TestRead (TestTemplates.Empty);
        }

        [Test]
        public void TestWriteOneNoun ()
        {
            TestWrite (TestTemplates.OneNoun);
        }

        [Test]
        public void TestReadOneNoun ()
        {
            TestRead (TestTemplates.OneNoun);
        }
    
        [Test]
        public void TestWriteTwoNouns ()
        {
            TestWrite (TestTemplates.TwoNouns);
        }

        [Test]
        public void TestReadTwoNouns ()
        {
            TestRead (TestTemplates.TwoNouns);
        }
}
}