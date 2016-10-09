using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using TextOn.Design;

namespace TextOn.Test
{
    internal class TemplateHost : IDisposable
    {
        private static string namespaceName = "TextOn.Test";

        private readonly Stream stream;
        private readonly StreamReader streamReader;

        public TemplateHost ([CallerMemberName] string testName = "Don't know")
        {
            var assembly = Assembly.GetExecutingAssembly ();
            var resourceName = namespaceName + ".Scenario." + (testName.Replace ("Test", "").Split ('_') [0]) + ".json";
            var serializer = new JsonSerializer ();
            serializer.Formatting = Formatting.Indented;
            foreach (var converter in TemplateWriter.JsonConverters) {
                serializer.Converters.Add (converter);
            }
            stream = assembly.GetManifestResourceStream (resourceName);
            streamReader = new StreamReader (stream);
            Object = (TextOnTemplate)serializer.Deserialize (streamReader, typeof (TextOnTemplate));
        }

        public TextOnTemplate Object { get; private set; }

        public void Dispose ()
        {
            streamReader.Dispose ();
            stream.Dispose ();
        }
    }
}
