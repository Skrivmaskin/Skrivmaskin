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
        public string Title { get; private set; }
        List<Node> Children;

        public static void GenerateSubTree (Node parentNode, INode designNode)
        {
            if (designNode is TextNode)
                parentNode.AddChild ("", ((TextNode)designNode).Value);
            else if (designNode is CommentNode)
                parentNode.AddChild ("", ((CommentNode)designNode).Value);
            else if (designNode is ChoiceNode) {
                var choiceNode = designNode as ChoiceNode;
                var node = parentNode.AddChild (choiceNode.ChoiceName, "");
                if (choiceNode.Choices != null)
                    foreach (var subNode in choiceNode.Choices) {
                        GenerateSubTree (node, subNode);
                    }
            } else if (designNode is ConcatNode) {
                var concatNode = designNode as ConcatNode;
                var node = parentNode.AddChild (concatNode.ConcatName, "");
                if (concatNode.Sequential != null)
                    foreach (var subNode in concatNode.Sequential) {
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

                node = new Node ("Name", "");
                Node variables = node.AddChild ("Variables", "");
                foreach (var variable in project.VariableDefinitions) {
                    var variableNode = variables.AddChild (variable.Name, variable.Description);
                    foreach (var form in variable.Forms) {
                        variableNode.AddChild (form.Name, form.Suggestion);
                    }
                }

                Node paragraphs = node.AddChild ("Definition", "");
                GenerateSubTree (paragraphs, project.Definition);

                errorText = null;
                return true;
            } catch (Exception e) {
                errorText = e.ToString ();
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

