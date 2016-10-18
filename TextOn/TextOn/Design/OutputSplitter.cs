using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace TextOn.Design
{
    /// <summary>
    /// Split some sample output into sentences and paragraphs to produce an outline of a design template, ready for editing.
    /// </summary>
    public static class OutputSplitter
    {
        private static readonly DesignNode [] emptyDesignSubtree = new DesignNode [0];
        
        /// <summary>
        /// Given some sample output, split into a new sequential node containing text nodes representing each sentence,
        /// and paragraph break nodes.
        /// </summary>
        /// <param name="sampleOutput">Sample output.</param>
        public static DesignNode Split (string sampleOutput)
        {
            var sequential = new List<DesignNode> ();
            var paragraphs = sampleOutput.Split (new char [] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            //var sequentialNode = new SequentialNode ((paragraphs.Length > 1) ? "Paragraphs" : "Sentences", true, sequential);
            if (paragraphs.Length == 0)
                return new DesignNode (NodeType.Sequential, true, "Sentences", emptyDesignSubtree);
            var sentenceSplitRegex = new Regex (@"\s*([^\.^\?^!]+[\.\?!]+)[\s$]+");
            if (paragraphs.Length == 1) {
                foreach (var text in sentenceSplitRegex.Split (paragraphs [0])) {
                    if (!String.IsNullOrWhiteSpace (text))
                        sequential.Add (new DesignNode (NodeType.Text, true, text, emptyDesignSubtree));
                }
                return new DesignNode (NodeType.Sequential, true, "Sentences", sequential.ToArray ());
            }
            for (int i = 0; i < paragraphs.Length; i++) {
                var sentences = new List<DesignNode> ();
                foreach (var text in sentenceSplitRegex.Split (paragraphs [i])) {
                    if (!String.IsNullOrWhiteSpace (text))
                        sentences.Add (new DesignNode (NodeType.Text, true, text, emptyDesignSubtree));
                }
                var paragraph = new DesignNode (NodeType.Sequential, true, ("Sentences " + (i + 1)), sentences.ToArray ());
                sequential.Add (paragraph);
                if (i < paragraphs.Length - 1) sequential.Add (new DesignNode (NodeType.ParagraphBreak, true, "", emptyDesignSubtree));
            }
            return new DesignNode (NodeType.Sequential, true, "Paragraphs", sequential.ToArray ());
        }
    }
}
