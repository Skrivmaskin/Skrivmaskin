using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Skrivmaskin.Design
{
    /// <summary>
    /// Split some sample output into sentences and paragraphs to produce an outline of a design project, ready for editing.
    /// </summary>
    public static class OutputSplitter
    {
        /// <summary>
        /// Given some sample output, split into a new Project with no variables and a sequential node containing text nodes representing each sentence,
        /// and paragraph break nodes.
        /// </summary>
        /// <param name="sampleOutput">Sample output.</param>
        public static INode Split (string sampleOutput)
        {
            var sequential = new List<INode> ();
            var paragraphs = sampleOutput.Split (new char [] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var sequentialNode = new SequentialNode ((paragraphs.Length > 1) ? "Paragraphs" : "Sentences", true, sequential);
            if (paragraphs.Length == 0)
                return sequentialNode;
            var sentenceSplitRegex = new Regex (@"\s*([^\.^\?^!]+[\.\?!]+)[\s$]+");
            if (paragraphs.Length == 1) {
                foreach (var text in sentenceSplitRegex.Split (paragraphs [0])) {
                    if (!String.IsNullOrWhiteSpace (text))
                        sequential.Add (new TextNode (text, true));
                }
                return sequentialNode;
            }
            for (int i = 0; i < paragraphs.Length; i++) {
                var sentences = new List<INode> ();
                var paragraph = new SequentialNode (("Sentences " + (i + 1)), true, sentences);
                foreach (var text in sentenceSplitRegex.Split (paragraphs [i])) {
                    if (!String.IsNullOrWhiteSpace (text))
                        sentences.Add (new TextNode (text, true));
                }
                sequential.Add (paragraph);
                if (i < paragraphs.Length - 1) sequential.Add (new ParagraphBreakNode (true));
            }
            return sequentialNode;
        }
    }
}
