using System;
using System.Linq;
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

        public void TestWrite (TestTemplates testTemplate)
        {
            var testCase = projects [testTemplate];
            var template = testCase.Item1;
            var expectedText = testCase.Item2;
            using (var stringWriter = new StringWriter ()) {
                TextOn.Storage.TemplateWriter.Write (stringWriter, template);
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
                var actualTemplate = TextOn.Storage.TemplateWriter.Read (stringReader);
                AssertTemplatesAreEqual (expectedTemplate, actualTemplate);
            }
        }

        public void AssertTemplatesAreEqual (TextOnTemplate expected, TextOnTemplate actual)
        {
            AssertNounProfilesAreEqual (expected.Nouns, actual.Nouns);
            AssertNodesAreEqual (expected.DesignTree, actual.DesignTree, new int [0]);
        }

        public void AssertNounProfilesAreEqual (NounProfile expected, NounProfile actual)
        {
        }

        public void AssertNodesAreEqual (DesignNode expected, DesignNode actual, int [] indexPath)
        {
            Assert.AreEqual (expected.IsActive, actual.IsActive, indexPath.ToString ());
            Assert.AreEqual (expected.Text, actual.Text, indexPath.ToString ());
            Assert.AreEqual (expected.Type, actual.Type, indexPath.ToString ());
            Assert.AreEqual (expected.ChildNodes.Length, actual.ChildNodes.Length, indexPath.ToString ());
            for (int i = 0; i < actual.ChildNodes.Length; i++) {
                var newIndexPath = indexPath.Concat (new int [1] { i }).ToArray ();
                AssertNodesAreEqual (expected.ChildNodes [i], actual.ChildNodes [i], newIndexPath);
            }
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