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
        public static DesignNode Find (DesignNode node, string searchText, bool exact)
        {
            switch (node.Type) {
            case NodeType.Text:
                if (node.Text.Contains (searchText))
                    return node;
                return null;
            case NodeType.ParagraphBreak:
                return null;
            case NodeType.Choice:
            case NodeType.Sequential:
                foreach (var childNode in node.ChildNodes) {
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
