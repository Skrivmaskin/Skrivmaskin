using System;
using System.Linq;
using System.Collections.Generic;
using TextOn.Design;
using TextOn.Interfaces;
using TextOn.Services;

namespace TextOn.Generation
{
    /// <summary>
    /// Preview generator. Uses a special combined random chooser that allows fixing of a certain number of choices.
    /// </summary>
    /// <remarks>
    /// This can be exposed to the user in a preview pane to flatten out the view of a given design node while they are working.
    /// </remarks>
    public sealed class TextOnPreviewGenerator
    {
        readonly ChoiceFixingRandomChooser randomChooser;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Generation.TextOnPreviewGenerator"/> class.
        /// </summary>
        /// <remarks>
        /// This takes ownership of the random chooser and will manage its lifetime. Access to the last seed should be via the generator.
        /// </remarks>
        /// <param name="randomChooser">Random chooser.</param>
        public TextOnPreviewGenerator (IRandomChooser randomChooser)
        {
            this.randomChooser = new ChoiceFixingRandomChooser (randomChooser);
        }

        private IEnumerable<PreviewRouteNode> GenerateText (INode node, bool reachedTarget)
        {
            switch (node.Type) {
            case NodeType.Text:
            case NodeType.ParagraphBreak:
                yield return new PreviewRouteNode (node, randomChooser.ChoicesMade, reachedTarget);
                break;
            case NodeType.Sequential:
                foreach (var n in (node as SequentialNode).Sequential) {
                    foreach (var text in GenerateText (n, reachedTarget)) {
                        yield return text;
                    }
                }
                break;
            case NodeType.Choice:
                var choiceNode = (node as ChoiceNode);
                if (choiceNode.Choices.Count > 0) {
                    var n = randomChooser.Choose (choiceNode, choiceNode.Choices.Count, out reachedTarget);
                    var choice = choiceNode.Choices [n];
                    var li = GenerateText (choice, reachedTarget);
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
        /// Generate a route through the design tree, given the specified fixed choices.
        /// </summary>
        /// <param name="choices">Choices.</param>
        public IEnumerable<PreviewRouteNode> GenerateWithFixedChoices (INode rootNode, int [] choices)
        {
            randomChooser.BeginWithFixedChoices (choices);
            var result = GenerateText (rootNode, false).ToList ();
            randomChooser.End ();
            return result;
        }

        /// <summary>
        /// Generate a route through the design tree, given the specified partial route.
        /// </summary>
        /// <param name="partialRoute">The partial route that will arrive at the desired node on its way.</param>
        public IEnumerable<PreviewRouteNode> GenerateWithPartialRoute (INode rootNode, PreviewPartialRouteChoiceNode [] partialRoute)
        {
            randomChooser.BeginWithPartialRoute (partialRoute);
            var result = GenerateText (rootNode, false).ToList ();
            randomChooser.End ();
            return result;
        }
    }
}
