using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TextOn.Design;
using TextOn.Nouns;

namespace TextOn.Storage
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

        internal sealed class NounProfileConverter : JsonConverter
        {
            public override bool CanConvert (Type objectType)
            {
                return (objectType == typeof (NounProfile));
            }

            public override object ReadJson (JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                var nounProfile = new NounProfile ();
                nounProfile.SetAllNouns (serializer.Deserialize<IEnumerable<Noun>> (reader));
                return nounProfile;
            }

            public override void WriteJson (JsonWriter writer, object value, JsonSerializer serializer)
            {
                var nounProfile = (NounProfile)value;
                serializer.Serialize (writer, nounProfile.GetAllNouns (), typeof (IEnumerable<Noun>));
            }
        }

        private static JsonConverter [] JsonConverters {
            get {
                return new JsonConverter [] { new NodeConverter (), new NounProfileConverter () };
            }
        }

        private static readonly DesignNode [] emptyDesignNodeSubtree = new DesignNode [0];
        private static DesignNode ConvertNodeFromSerialized (INode node)
        {
            switch (node.Type) {
            case NodeType.Text:
                var textNode = node as TextNode;
                return new DesignNode (NodeType.Text, textNode.IsActive, textNode.Text, emptyDesignNodeSubtree);
            case NodeType.ParagraphBreak:
                var paragraphBreakNode = node as ParagraphBreakNode;
                return new DesignNode (NodeType.ParagraphBreak, paragraphBreakNode.IsActive, "", emptyDesignNodeSubtree);
            case NodeType.Choice:
                var choiceNode = node as ChoiceNode;
                return new DesignNode (NodeType.Choice, choiceNode.IsActive, choiceNode.ChoiceName, choiceNode.Choices.Select ((n) => ConvertNodeFromSerialized (n)).ToArray ());
            case NodeType.Sequential:
                var sequentialNode = node as SequentialNode;
                return new DesignNode (NodeType.Sequential, sequentialNode.IsActive, sequentialNode.SequentialName, sequentialNode.Sequential.Select ((n) => ConvertNodeFromSerialized (n)).ToArray ());
            default:
                throw new ApplicationException ("Unrecognised node type " + node.Type);
            }
        }

        private static SerializedTemplate ConvertToSerialized (TextOnTemplate template)
        {
            var definition = ConvertNodeToSerialized (template.DesignTree);
            return new SerializedTemplate (template.Nouns, null, definition, 1);
        }

        private static INode ConvertNodeToSerialized (DesignNode designNode)
        {
            switch (designNode.Type) {
            case NodeType.Text:
                return new TextNode (designNode.Text, designNode.IsActive);
            case NodeType.ParagraphBreak:
                return new ParagraphBreakNode (designNode.IsActive);
            case NodeType.Sequential:
                return new SequentialNode (designNode.Text, designNode.IsActive, designNode.ChildNodes.Select ((n) => ConvertNodeToSerialized (n)).ToList());
            case NodeType.Choice:
                return new ChoiceNode (designNode.Text, designNode.IsActive, designNode.ChildNodes.Select ((n) => ConvertNodeToSerialized (n)).ToList());
            default:
                throw new ApplicationException ("Unrecognised node type " + designNode.Type);
            }
        }

        /// <summary>
        /// Serialize.
        /// </summary>
        /// <param name="writer">Writer.</param>
        /// <param name="template">Template.</param>
        public static void Write (TextWriter writer, TextOnTemplate template)
        {
            var serializer = new JsonSerializer ();
            serializer.Formatting = Formatting.Indented;
            serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
            foreach (var converter in JsonConverters) {
                serializer.Converters.Add (converter);
            }
            serializer.Serialize (writer, ConvertToSerialized (template));
        }

        /// <summary>
        /// Serialize.
        /// </summary>
        public static void Write (FileInfo fileInfo, TextOnTemplate template)
        {
            using (var stream = new FileStream (fileInfo.FullName, FileMode.Create, FileAccess.Write))
            using (var writer = new StreamWriter (stream))
                Write (writer, template);
        }

        /// <summary>
        /// Deserialize.
        /// </summary>
        public static TextOnTemplate Read (TextReader reader)
        {
            var serializer = new JsonSerializer ();
            serializer.Formatting = Formatting.Indented;
            serializer.DefaultValueHandling = DefaultValueHandling.Ignore;
            foreach (var converter in JsonConverters) {
                serializer.Converters.Add (converter);
            }
            var serializedTemplate = (SerializedTemplate)serializer.Deserialize (reader, typeof (SerializedTemplate));
            var designNode = ConvertNodeFromSerialized (serializedTemplate.Definition);
            return new TextOnTemplate (serializedTemplate.Nouns, designNode);
        }

        /// <summary>
        /// Deserialize.
        /// </summary>
        public static TextOnTemplate Read (FileInfo fileInfo)
        {
            using (var stream = new FileStream (fileInfo.FullName, FileMode.Open, FileAccess.Read))
            using (var reader = new StreamReader (stream)) {
                return Read (reader);
            }
        }
    }
}
