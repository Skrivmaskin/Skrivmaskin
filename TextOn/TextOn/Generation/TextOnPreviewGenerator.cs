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

        PreviewRouteState state = PreviewRouteState.BeforeTarget;
        private IEnumerable<PreviewRouteNode> GenerateText (INode node, INode targetNode)
        {
            switch (node.Type) {
            case NodeType.Text:
            case NodeType.ParagraphBreak:
                if (Object.ReferenceEquals (node, targetNode)) {
                    yield return new PreviewRouteNode (node, randomChooser.ChoicesMade, PreviewRouteState.AtTarget);
                    state = PreviewRouteState.AfterTarget;
                } else {
                    if ((numChoicesRemaining != null) && (numChoicesRemaining == 0)) {
                        numChoicesRemaining = null;
                        state = PreviewRouteState.AfterTarget;
                    }
                    yield return new PreviewRouteNode (node, randomChooser.ChoicesMade, state);
                }
                break;
            case NodeType.Sequential:
                if (Object.ReferenceEquals (node, targetNode)) {
                    yield return new PreviewRouteNode (node, randomChooser.ChoicesMade, PreviewRouteState.AtTarget);
                    state = PreviewRouteState.AfterTarget;
                }
                foreach (var n in (node as SequentialNode).Sequential) {
                    foreach (var text in GenerateText (n, targetNode)) {
                        yield return text;
                    }
                }
                break;
            case NodeType.Choice:
                if (Object.ReferenceEquals (node, targetNode)) {
                    yield return new PreviewRouteNode (node, randomChooser.ChoicesMade, PreviewRouteState.AtTarget);
                    state = PreviewRouteState.AfterTarget;
                }
                var choiceNode = (node as ChoiceNode);
                if (choiceNode.Choices.Count > 0) {
                    var n = randomChooser.Choose (choiceNode, choiceNode.Choices.Count);
                    if (numChoicesRemaining != null) numChoicesRemaining = (numChoicesRemaining - 1);
                    var choice = choiceNode.Choices [n];
                    var li = GenerateText (choice, targetNode);
                    foreach (var text in li) {
                        yield return text;
                    }
                }
                break;
            default:
                break;
            }
        }

        int? numChoicesRemaining = null;

        /// <summary>
        /// Generate a route through the design tree, given the specified fixed choices.
        /// </summary>
        /// <param name="choices">Choices.</param>
        public IEnumerable<PreviewRouteNode> GenerateWithFixedChoices (INode rootNode, int [] choices)
        {
            state = PreviewRouteState.BeforeTarget;
            numChoicesRemaining = choices.Length;
            randomChooser.BeginWithFixedChoices (choices);
            var result = GenerateText (rootNode, null).ToList ();
            randomChooser.End ();
            return result;
        }

        /// <summary>
        /// Generate a route through the design tree, given the specified partial route.
        /// </summary>
        /// <param name="partialRoute">The partial route that will arrive at the desired node on its way.</param>
        public IEnumerable<PreviewRouteNode> GenerateWithPartialRoute (INode rootNode, PreviewPartialRouteChoiceNode [] partialRoute)
        {
            state = PreviewRouteState.BeforeTarget;
            numChoicesRemaining = null;
            randomChooser.BeginWithPartialRoute (partialRoute.Take (partialRoute.Length - 1).ToArray ());
            var result = GenerateText (rootNode, partialRoute [partialRoute.Length - 1].TargetNode).ToList ();
            randomChooser.End ();
            return result;
        }
    }
}
