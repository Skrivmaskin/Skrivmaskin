using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TextOn.Design
{
    /// <summary>
    /// JSON serializer for design time TextOn templates.
    /// </summary>
    public static class TemplateWriter
    {
        internal sealed class NodeConverter : JsonConverter
        {
            public override bool CanConvert (Type objectType)
            {
                return (objectType == typeof (INode));
            }

            //TODO stringify type programmatically using generics and nameof or sthg?
            public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                // Load JObject from stream
                JObject jObject = JObject.Load (reader);
                if (jObject ["Choices"] != null) {
                    var choiceNode = new ChoiceNode ();
                    serializer.Populate (jObject.CreateReader (), choiceNode);
                    return choiceNode;
                }
                if (jObject ["Sequential"] != null) {
                    var sequentialNode = new SequentialNode ();
                    serializer.Populate (jObject.CreateReader (), sequentialNode);
                    return sequentialNode;
                }
                if (jObject ["Text"] != null) {
                    var textNode = new TextNode ();
                    serializer.Populate (jObject.CreateReader (), textNode);
                    return textNode;
                }
                var paragraphBreakNode = new ParagraphBreakNode ();
                serializer.Populate (jObject.CreateReader (), paragraphBreakNode);
                return paragraphBreakNode;
            }

            public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException ();
            }
        }

        /// <summary>
        /// Get the JSON converters needed to write projects, for testing and for users to be abl        /// to serialize Projects themselves.
        /// </summary>
        /// <value>The json converters.</value>
        public static JsonConverter [] JsonConverters {
            get {
                return new JsonConverter [1] { new NodeConverter () };
            }
        }

        /// <summary>
        /// Serialize with JSON.
        /// </summary>
        public static void Write (FileInfo fileInfo, TextOnTemplate template)
        {
            var serializer = new JsonSerializer ();
            serializer.Formatting = Formatting.Indented;
            serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
            serializer.Converters.Add (new NodeConverter ());
            using (var stream = new FileStream (fileInfo.FullName, FileMode.Create, FileAccess.Write))
            using (var writer = new StreamWriter (stream))
                serializer.Serialize (writer, template);
        }

        /// <summary>
        /// Deserialize with JSON.
        /// </summary>
        public static TextOnTemplate Read (FileInfo fileInfo)
        {
            var serializer = new JsonSerializer ();
            serializer.Formatting = Formatting.Indented;
            serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
            serializer.Converters.Add (new NodeConverter ());
            using (var stream = new FileStream (fileInfo.FullName, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader (stream))
                return (TextOnTemplate)serializer.Deserialize (reader, typeof (TextOnTemplate));
        }
    }
}
