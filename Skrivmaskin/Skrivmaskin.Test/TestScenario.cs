using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;
using Skrivmaskin.Design;

namespace Skrivmaskin.Test
{
    [TestFixture]
    public class TestScenario
    {
        private static string namespaceName = "Skrivmaskin.Test";

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
                }
            }
        }
    }
}
