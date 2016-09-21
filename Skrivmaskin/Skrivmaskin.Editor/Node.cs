using System;
using Foundation;
using System.Collections.Generic;
using System.IO;
using Skrivmaskin.Core.Design;

namespace Skrivmaskin.Editor
{
    // This is a little tree data structure to show off outline view display
    // OPS This wants to be in terms of the Lexer's types, because it is all pre-lex.
    public class Node : NSObject
    {
        public string Title { get; set; }
        List<Node> Children;

        public static void GenerateSubTree (Node parentNode, INode designNode)
        {
            if (designNode is TextNode)
                parentNode.AddChild (NodeType.Text, "", ((TextNode)designNode).Text);
            else if (designNode is CommentNode)
                parentNode.AddChild (NodeType.Comment, "", ((CommentNode)designNode).Value);
            else if (designNode is ChoiceNode) {
                var choiceNode = designNode as ChoiceNode;
                var node = parentNode.AddChild (NodeType.Choice, choiceNode.ChoiceName, "");
                if (choiceNode.Choices != null)
                    foreach (var subNode in choiceNode.Choices) {
                        GenerateSubTree (node, subNode);
                    }
            } else if (designNode is SequentialNode) {
                var sequentialNode = designNode as SequentialNode;
                var node = parentNode.AddChild (NodeType.Sequential, sequentialNode.SequentialName, "");
                if (sequentialNode.Sequential != null)
                    foreach (var subNode in sequentialNode.Sequential) {
                        GenerateSubTree (node, subNode);
                    }
            } else
                throw new ApplicationException ("Unexpected node type " + designNode.GetType ());
        }

        public static bool CreateTree (string filePath, out Node node, out string errorText)
        {
            try {
                var fileInfo = new FileInfo (filePath);
                var project = ProjectWriter.Read (fileInfo);

                node = new Node (NodeType.Root, "Name", "");
                Node variables = node.AddChild (NodeType.Root, "Variables", "");
                foreach (var variable in project.VariableDefinitions) {
                    var variableNode = variables.AddChild (NodeType.Variable, variable.Name, variable.Description);
                    foreach (var form in variable.Forms) {
                        variableNode.AddChild (NodeType.VariableForm, form.Name, form.Suggestion);
                    }
                }

                Node paragraphs = node.AddChild (NodeType.Root, "Definition", "");
                GenerateSubTree (paragraphs, project.Definition);

                errorText = null;
                return true;
            } catch (Exception e) {
                errorText = e.ToString ();
                node = new Node (NodeType.Root, "", "");
                node.AddChild (NodeType.Root, "Variables", "");
                node.AddChild (NodeType.Root, "Paragraphs", "");
            }
            return false;
        }

        public NodeType Type { get; private set;}

        public Node (NodeType nodeType, string title, string description)
        {
            Type = nodeType;
            Title = title;
            Children = new List<Node> ();
            this.description = description;
        }

        public Node AddChild (NodeType nodeType, string name, string description)
        {
            Node n = new Node (nodeType, name, description);
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
        public void SetDescription (string desc)
        {
            description = desc;
        }

        public int ChildCount { get { return Children.Count; } }
        public bool IsLeaf { get { return ChildCount == 0; } }
    }
}

