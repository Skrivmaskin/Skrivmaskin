using System;
using TextOn.Design;

namespace TextOn.Search
{
    /// <summary>
    /// Utilities for search and replace.
    /// </summary>
    public static class TextOnSearchAndReplacer
    {
        /// <summary>
        /// Find the first TextNode for the specified search text in the design tree.
        /// </summary>
        /// <param name="node">Within this node.</param>
        /// <param name="searchText">Search text.</param>
        /// <param name="exact">If set to <c>true</c> perform an exact match.</param>
        public static INode Find (INode node, string searchText, bool exact)
        {
            switch (node.Type) {
            case NodeType.Text:
                var textNode = node as TextNode;
                if (textNode.Text.Contains (searchText))
                    return node;
                return null;
            case NodeType.ParagraphBreak:
                return null;
            case NodeType.Choice:
                var choiceNode = node as ChoiceNode;
                foreach (var childNode in choiceNode.Choices) {
                    var returnNode = Find (childNode, searchText, exact);
                    if (returnNode != null) return returnNode;
                }
                return null;
            case NodeType.Sequential:
                var sequentialNode = node as SequentialNode;
                foreach (var childNode in sequentialNode.Sequential) {
                    var returnNode = Find (childNode, searchText, exact);
                    if (returnNode != null) return returnNode;
                }
                return null;
            default:
                break;
            }
            return null;
        }
    }
}
