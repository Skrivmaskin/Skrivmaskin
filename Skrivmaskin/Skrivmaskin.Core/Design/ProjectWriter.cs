using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Skrivmaskin.Core.Design
{
    /// <summary>
    /// JSON serializer for design time Skrivmaskin projects.
    /// </summary>
    public static class ProjectWriter
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
                if (jObject ["CommentName"] != null) {
                    var commentNode = new CommentNode ();
                    serializer.Populate (jObject.CreateReader (), commentNode);
                    return commentNode;
                }
                if (jObject ["Choices"] != null) {
                    var choiceNode = new ChoiceNode ();
                    serializer.Populate (jObject.CreateReader (), choiceNode);
                    return choiceNode;
                }
                if (jObject ["Sequential"] != null) {
                    var concatNode = new ConcatNode ();
                    serializer.Populate (jObject.CreateReader (), concatNode);
                    return concatNode;
                }
                var textNode = new TextNode ();
                serializer.Populate (jObject.CreateReader (), textNode);
                return textNode;
            }

            public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
            {
                throw new NotImplementedException ();
            }
        }

        /// <summary>
        /// Serialize with JSON.
        /// </summary>
        public static void Write (FileInfo fileInfo, Project project)
        {
            var serializer = new JsonSerializer ();
            serializer.Formatting = Formatting.Indented;
            serializer.Converters.Add (new NodeConverter ());
            using (var stream = new FileStream (fileInfo.FullName, FileMode.Create, FileAccess.Write))
            using (var writer = new StreamWriter (stream))
                serializer.Serialize (writer, project);
        }

        /// <summary>
        /// Deserialize with JSON.
        /// </summary>
        public static Project Read (FileInfo fileInfo)
        {
            var serializer = new JsonSerializer ();
            serializer.Formatting = Formatting.Indented;
            serializer.Converters.Add (new NodeConverter ());
            using (var stream = new FileStream (fileInfo.FullName, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader (stream))
                return (Project)serializer.Deserialize (reader, typeof (Project));
        }
    }
}
