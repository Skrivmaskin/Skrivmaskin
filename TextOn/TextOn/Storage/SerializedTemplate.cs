using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TextOn.Nouns;
using TextOn.Design;
using TextOn.Version0;

namespace TextOn.Storage
{
    /// <summary>
    /// The user's design time project.
    /// </summary>
    internal sealed class SerializedTemplate
    {
        [JsonConstructor]
        internal SerializedTemplate (NounProfile nouns, List<Variable> variableDefinitions, INode definition, int version)
        {
            Nouns = nouns;
            VariableDefinitions = variableDefinitions;
            Definition = definition;
            Version = version;
            Initialize ();
        }

        public SerializedTemplate (NounProfile nouns, INode definition)
        {
            Nouns = nouns;
            Definition = definition;
            VariableDefinitions = null;
            Version = 1;
        }

        /// <summary>
        /// Initialize this instance of the <see cref="T:TextOn.Design.TextOnTemplate"/> class.
        /// </summary>
        private void Initialize ()
        {
            if (Version == 0) {
                Migrate_0_1 ();
            }
        }

        private void Migrate_0_1 ()
        {
            if (VariableDefinitions == null) VariableDefinitions = new List<Variable> ();
            Nouns = new NounProfile ();
            foreach (var variable in VariableDefinitions) {
                foreach (var variant in variable.Forms) {
                    var nounName = variable.Name + variant.Name;
                    Nouns.AddNewNoun (nounName, variable.Description, true);
                    Nouns.AddSuggestion (nounName, variant.Suggestion, new NounSuggestionDependency [0]);
                    if (!String.IsNullOrEmpty (variant.Name)) {
                        ReplaceVariantInTemplate_0_1 (("[" + variable.Name + "|" + variant.Name + "]"), ("[" + variable.Name + variant.Name + "]"), Definition);
                    }
                }
            }
            VariableDefinitions = null;
            Version = 1;
        }

        private void ReplaceVariantInTemplate_0_1 (string searchText, string replaceText, INode node)
        {
            IEnumerable<INode> children = null;
            switch (node.Type) {
            case NodeType.Text:
                var textNode = (TextNode)node;
                var text = textNode.Text;
                textNode.Text = text.Replace (searchText, replaceText);
                break;
            case NodeType.Choice:
                children = ((ChoiceNode)node).Choices;
                break;
            case NodeType.Sequential:
                children = ((SequentialNode)node).Sequential;
                break;
            default:
                break;
            }
            if (children != null) {
                foreach (var child in children) {
                    ReplaceVariantInTemplate_0_1 (searchText, replaceText, child);
                }
            }
        }

        /// <summary>
        /// The user's variable definitions.
        /// </summary>
        /// <value>The variable definitions.</value>
        internal IReadOnlyList<Variable> VariableDefinitions { get; private set; }
        public bool ShouldSerializeVariableDefinitions ()
        {
            return VariableDefinitions != null && VariableDefinitions.Count > 0;
        }

        /// <summary>
        /// The user's noun profile.
        /// </summary>
        /// <value>The nouns.</value>
        public NounProfile Nouns { get; private set; }

        /// <summary>
        /// The definition of the template.
        /// </summary>
        /// <value>The definition.</value>
        public INode Definition { get; private set; }

        public bool Equals (SerializedTemplate other)
        {
            if (this.VariableDefinitions.Count != other.VariableDefinitions.Count) return false;
            for (int i = 0; i < this.VariableDefinitions.Count; i++) {
                if (!this.VariableDefinitions [i].Equals (other.VariableDefinitions [i])) return false;
            }
            if (!this.Definition.Equals (other.Definition)) return false;
            return true;
        }

        /// <summary>
        /// Gets or sets the version for the storage format.
        /// </summary>
        /// <value>The version.</value>
        public int Version { get; set; }
    }
}
