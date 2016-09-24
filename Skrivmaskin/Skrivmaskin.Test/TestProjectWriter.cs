using System;
using Skrivmaskin.Design;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;

namespace Skrivmaskin.Test
{
    [TestFixture]
    public class TestProjectWriter
    {
        private static IDictionary<TestProjects,Tuple<Project, string>> projects = TestProjectUtils.SetupProjects ();

        public void TestWrite (TestProjects testProject)
        {
            var jsonConverters = ProjectWriter.JsonConverters;
            var jsonSerializer = new JsonSerializer ();
            jsonSerializer.Formatting = Formatting.Indented;
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
            TestWrite (TestProjects.Empty);
        }

        [Test]
        public void TestWriteOneVariable ()
        {
            TestWrite (TestProjects.OneVariable);
        }

        [Test]
        public void TestWriteParagraphBreak ()
        {
            TestWrite (TestProjects.ParagraphBreak);
        }


        public void TestRead (TestProjects testProject)
        {
            var jsonConverters = ProjectWriter.JsonConverters;
            var jsonSerializer = new JsonSerializer ();
            jsonSerializer.Formatting = Formatting.Indented;
            foreach (var converter in jsonConverters) {
                jsonSerializer.Converters.Add (converter);
            }
            var testCase = projects [testProject];
            var expectedProject = testCase.Item1;
            var text = testCase.Item2;
            using (var stringReader = new StringReader (text)) {
                Project actualProject = (Project)jsonSerializer.Deserialize (stringReader, typeof (Project));
                Assert.AreEqual (expectedProject, actualProject);
            }
        }

        [Test]
        public void TestReadEmpty ()
        {
            TestRead (TestProjects.Empty);
        }

        [Test]
        public void TestReadOneVariable ()
        {
            TestRead (TestProjects.OneVariable);
        }


    }
}