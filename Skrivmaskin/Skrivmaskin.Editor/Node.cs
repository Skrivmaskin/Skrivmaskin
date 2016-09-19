using System;
using Foundation;
using System.Collections.Generic;
using System.IO;
using Skrivmaskin.Lexing;

namespace Skrivmaskin.Editor
{
    // This is a little tree data structure to show off outline view display
    // OPS This wants to be in terms of the Lexer's types, because it is all pre-lex.
    public class Node : NSObject
    {
        public string Title { get; private set; }
        List<Node> Children;

        public static bool CreateExampleTree (string filePath, out Node node)
        {
            try {
                var fileInfo = new FileInfo (filePath);
                var project = ProjectWriter.read (fileInfo);

                node = new Node ("", "");
                Node variables = node.AddChild ("Variables", "");
                foreach (var variable in project.Variables) {
                    variables.AddChild (variable.Name, variable.Description);
                }

                Node paragraphs = node.AddChild ("Paragraphs", "");
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

                return true;
            } catch (Exception) {
                node = new Node ("", "");
                node.AddChild ("Variables", "");
                node.AddChild ("Paragraphs", "");
            }
            return false;
        }

        public Node (string title, string description)
        {
            Title = title;
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
            get { return description; }
        }

        public int ChildCount { get { return Children.Count; } }
        public bool IsLeaf { get { return ChildCount == 0; } }
    }
}

