using System;
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
        public static Project Split (string sampleOutput)
        {
            var sequential = new List<INode> ();
            var sequentialNode = new SequentialNode ("Sentences", true, sequential);
            return new Project (new List<Variable> (), sequentialNode);
        }
    }
}
