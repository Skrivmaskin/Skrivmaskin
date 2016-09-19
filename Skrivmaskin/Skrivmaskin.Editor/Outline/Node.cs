using System;
using Foundation;
using System.Collections.Generic;
using System.IO;
using Skrivmaskin.Lexing;

namespace Skrivmaskin.Editor.Outline
{
    // This is a little tree data structure to show off outline view display
    // OPS This wants to be in terms of the Lexer's types, because it is all pre-lex.
    class Node : NSObject
    {
        public string Name { get; private set; }
        List<Node> Children;

        public static Node CreateExampleTree ()
        {
            var fileInfo = new FileInfo ("/Users/Oliver/Projects/Skrivmaskin/Json/1aStycket.json");
            var project = ProjectWriter.read (fileInfo);

            Node parentNode = new Node ("", "");
            Node variables = parentNode.AddChild ("Variables", "");
            foreach (var variable in project.Variables) {
                variables.AddChild (variable.Name, variable.Description);
            }

            Node paragraphs = parentNode.AddChild ("Paragraphs", "");
            foreach (var paragraph in project.Paragraphs) {
                var paragraphNode = paragraphs.AddChild (paragraph.ParagraphName, "");
                foreach (var paragraphChoice in paragraph.Paragraph) {
                    var paragraphChoiceNode = paragraphNode.AddChild (paragraphChoice.ParagraphChoiceName, "");
                    foreach (var sentence in paragraphChoice.ParagraphChoice) {
                        var sentenceNode = paragraphChoiceNode.AddChild (sentence.SentenceName, "");
                        foreach (var sentenceChoice in sentence.Sentence) {
                            sentenceNode.AddChild (sentenceChoice, sentenceChoice);
                        }
                    }
                }
            }

            return parentNode;
        }

        public Node (string name, string description)
        {
            Name = name;
            Children = new List<Node> ();
            this.description = description;
        }

        public Node AddChild (string name, string description)
        {
            Node n = new Node (name, description);
            Children.Add (n);
            return n;
        }

        public Node GetChild (int index)
        {
            return Children [index];
        }

        private string description;
        public override string Description {
            get { return description;}
        }

        public int ChildCount { get { return Children.Count; } }
        public bool IsLeaf { get { return ChildCount == 0; } }
    }
}

