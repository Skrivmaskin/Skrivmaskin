using System;
using TextOn.Design;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;

namespace TextOn.Test
{
    [TestFixture]
    public class TestTemplateWriter
    {
        private static IDictionary<TestTemplates,Tuple<TextOnTemplate, string>> projects = TestTemplateUtils.SetupProjects ();

        public void TestWrite (TestTemplates testProject)
        {
            var jsonConverters = TemplateWriter.JsonConverters;
            var jsonSerializer = new JsonSerializer ();
            jsonSerializer.Formatting = Formatting.Indented;
            jsonSerializer.DefaultValueHandling = DefaultValueHandling.Ignore;
            foreach (var converter in jsonConverters) {
                jsonSerializer.Converters.Add (converter);
            }
            var testCase = projects [testProject];
            var project = testCase.Item1;
            var expectedText = testCase.Item2;
            using (var stringWriter = new StringWriter ()) {
                jsonSerializer.Serialize (stringWriter, project);
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
        public void TestWriteOneVariable ()
        {
            TestWrite (TestTemplates.OneVariable);
        }

        [Test]
        public void TestWriteParagraphBreak ()
        {
            TestWrite (TestTemplates.ParagraphBreak);
        }


        public void TestRead (TestTemplates testProject)
        {
            var jsonConverters = TemplateWriter.JsonConverters;
            var jsonSerializer = new JsonSerializer ();
            jsonSerializer.Formatting = Formatting.Indented;
            foreach (var converter in jsonConverters) {
                jsonSerializer.Converters.Add (converter);
            }
            var testCase = projects [testProject];
            var expectedProject = testCase.Item1;
            var text = testCase.Item2;
            using (var stringReader = new StringReader (text)) {
                TextOnTemplate actualProject = (TextOnTemplate)jsonSerializer.Deserialize (stringReader, typeof (TextOnTemplate));
                Assert.AreEqual (expectedProject, actualProject);
            }
        }

        [Test]
        public void TestReadEmpty ()
        {
            TestRead (TestTemplates.Empty);
        }

        [Test]
        public void TestReadOneVariable ()
        {
            TestRead (TestTemplates.OneVariable);
        }


    }
}