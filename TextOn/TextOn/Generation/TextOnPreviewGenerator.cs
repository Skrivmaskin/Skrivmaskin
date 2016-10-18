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
        private IEnumerable<PreviewRouteNode> GenerateText (DesignNode node, DesignNode targetNode)
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
                bool inTarget = Object.ReferenceEquals (node, targetNode);
                if (inTarget) {
                    yield return new PreviewRouteNode (node, randomChooser.ChoicesMade, PreviewRouteState.AtTarget);
                    state = PreviewRouteState.WithinTarget;
                }
                foreach (var n in node.ChildNodes) {
                    foreach (var text in GenerateText (n, targetNode)) {
                        yield return text;
                    }
                }
                if (inTarget) state = PreviewRouteState.AfterTarget;
                break;
            case NodeType.Choice:
                bool inTarget2 = Object.ReferenceEquals (node, targetNode);
                if (inTarget2) {
                    yield return new PreviewRouteNode (node, randomChooser.ChoicesMade, PreviewRouteState.AtTarget);
                    state = PreviewRouteState.WithinTarget;
                }
                if (node.ChildNodes.Length > 0) {
                    var n = randomChooser.Choose (node, node.ChildNodes.Length);
                    if (numChoicesRemaining != null) numChoicesRemaining = (numChoicesRemaining - 1);
                    var choice = node.ChildNodes [n];
                    var li = GenerateText (choice, targetNode);
                    foreach (var text in li) {
                        yield return text;
                    }
                }
                if (inTarget2)
                    state = PreviewRouteState.AfterTarget;
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
        public IEnumerable<PreviewRouteNode> GenerateWithFixedChoices (DesignNode rootNode, int [] choices)
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
        public IEnumerable<PreviewRouteNode> GenerateWithPartialRoute (DesignNode rootNode, PreviewPartialRouteChoiceNode [] partialRoute)
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
