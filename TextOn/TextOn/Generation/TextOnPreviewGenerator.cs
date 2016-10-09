using System;
using System.Linq;
using System.Collections.Generic;
using TextOn.Design;
using TextOn.Interfaces;

namespace TextOn.Generation
{
    /// <summary>
    /// Preview generator. Takes a random number generator and makes only the random choices at design template level, to give back
    /// a sequence of Text and ParagraphBreak design nodes.
    /// </summary>
    /// <remarks>
    /// This can be exposed to the user in a preview pane to flatten out the view of a given design node while they are working.
    /// </remarks>
    public sealed class TextOnPreviewGenerator
    {
        readonly IRandomChooser randomChooser;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Generation.TextOnPreviewGenerator"/> class.
        /// </summary>
        /// <remarks>
        /// This takes ownership of the random chooser and will manage its lifetime. Access to the last seed should be via the generator.
        /// </remarks>
        /// <param name="randomChooser">Random chooser.</param>
        public TextOnPreviewGenerator (IRandomChooser randomChooser)
        {
            this.randomChooser = randomChooser;
        }

        private IEnumerable<INode> GenerateText (INode node)
        {
            switch (node.Type) {
            case NodeType.Text:
            case NodeType.ParagraphBreak:
                yield return node;
                break;
            case NodeType.Sequential:
                foreach (var n in (node as SequentialNode).Sequential) {
                    foreach (var text in GenerateText (n)) {
                        yield return text;
                    }
                }
                break;
            case NodeType.Choice:
                var choices = (node as ChoiceNode).Choices;
                if (choices.Count > 0) {
                    var n = randomChooser.Choose (choices.Count);
                    var choice = choices [n];
                    var li = GenerateText (choice);
                    foreach (var text in li) {
                        yield return text;
                    }
                }
                break;
            default:
                break;
            }
        }

        /// <summary>
        /// Gets the last seed used in making random choices.
        /// </summary>
        /// <remarks>
        /// Exposed to the user for logging and reproducability purposes.
        /// </remarks>
        /// <value>The last seed.</value>
        public int? LastSeed { get { return randomChooser.LastSeed; } }

        /// <summary>
        /// Determines if it is possible for this generator to regenerate.
        /// </summary>
        /// <remarks>
        /// May be used by a user interface to allow the user to activate and hide controls that Regenerate.
        /// </remarks>
        /// <returns><c>true</c>, if regenerate was caned, <c>false</c> otherwise.</returns>
        public bool CanRegenerate ()
        {
            return LastSeed != null;
        }

        /// <summary>
        /// Generate the preview text.
        /// </summary>
        public IEnumerable<INode> Generate (INode node)
        {
            randomChooser.Begin ();
            var result = GenerateText (node).ToList ();
            randomChooser.End ();
            return result;
        }

        /// <summary>
        /// Regenerate the preview text, using the same random seed as before.
        /// </summary>
        public IEnumerable<INode> Regenerate (INode node)
        {
            var lastSeed = LastSeed;
            if (lastSeed == null) throw new ApplicationException ("Unable to regenerate when we haven't run before");
            return GenerateWithSeed (node, lastSeed.Value);
        }

        /// <summary>
        /// Generate the preview text, using a given random seed.
        /// </summary>
        public IEnumerable<INode> GenerateWithSeed (INode node, int seed)
        {
            randomChooser.BeginWithSeed (seed);
            var result = GenerateText (node).ToList ();
            randomChooser.End ();
            return result;
        }
    }
}
