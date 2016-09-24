using System;
using Foundation;
using System.Collections.Generic;
using System.IO;
using Skrivmaskin.Design;

namespace Skrivmaskin.Editor
{
    // This is a little tree data structure to show off outline view display
    // OPS This wants to be in terms of the Lexer's types, because it is all pre-lex.
    public class DesignNode : NSObject
    {
        public string Title { get; set; }
        List<DesignNode> Children;

        public static void GenerateSubTree (DesignNode parentNode, INode designNode)
        {
            switch (designNode.Type) {
            case NodeType.Text:
                parentNode.AddChild (DesignNodeType.Text, "", ((TextNode)designNode).Text);
                break;
            case NodeType.Comment:
                parentNode.AddChild (DesignNodeType.Comment, "", ((CommentNode)designNode).Value);
                break;
            case NodeType.Choice:
                var choiceNode = designNode as ChoiceNode;
                var nodeC = parentNode.AddChild (DesignNodeType.Choice, choiceNode.ChoiceName, "");
                if (choiceNode.Choices != null)
                    foreach (var subNode in choiceNode.Choices) {
                        GenerateSubTree (nodeC, subNode);
                    }
                break;
            case NodeType.Sequential:
                var sequentialNode = designNode as SequentialNode;
                var nodeS = parentNode.AddChild (DesignNodeType.Sequential, sequentialNode.SequentialName, "");
                if (sequentialNode.Sequential != null)
                    foreach (var subNode in sequentialNode.Sequential) {
                        GenerateSubTree (nodeS, subNode);
                    }
                break;
            default:
                break;
            }
        }

        public static bool CreateTree (Project project, out DesignNode node, out string errorText)
        {
            try {
                node = new DesignNode (DesignNodeType.Root, "Name", "");
                DesignNode variables = node.AddChild (DesignNodeType.Root, "Variables", "");
                foreach (var variable in project.VariableDefinitions) {
                    var variableNode = variables.AddChild (DesignNodeType.Variable, variable.Name, variable.Description);
                    foreach (var form in variable.Forms) {
                        variableNode.AddChild (DesignNodeType.VariableForm, form.Name, form.Suggestion);
                    }
                }

                DesignNode paragraphs = node.AddChild (DesignNodeType.Root, "Definition", "");
                GenerateSubTree (paragraphs, project.Definition);

                errorText = null;
                return true;
            } catch (Exception e) {
                errorText = e.ToString ();
                node = new DesignNode (DesignNodeType.Root, "", "");
                node.AddChild (DesignNodeType.Root, "Variables", "");
                node.AddChild (DesignNodeType.Root, "Paragraphs", "");
            }
            return false;
        }

        public DesignNodeType Type { get; private set;}

        public DesignNode (DesignNodeType nodeType, string title, string description)
        {
            Type = nodeType;
            Title = title;
            Children = new List<DesignNode> ();
            this.description = description;
        }

        public DesignNode AddChild (DesignNodeType nodeType, string name, string description)
        {
            DesignNode n = new DesignNode (nodeType, name, description);
            Children.Add (n);
            return n;
        }

        public DesignNode GetChild (int index)
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

