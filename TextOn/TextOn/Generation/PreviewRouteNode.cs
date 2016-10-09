using System;
using TextOn.Design;

namespace TextOn.Generation
{
    /// <summary>
    /// One step in a path through the design tree.
    /// </summary>
    public sealed class PreviewRouteNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Generation.PreviewRouteNode"/> class.
        /// </summary>
        /// <param name="node">Node.</param>
        /// <param name="choicesMadeSoFar">Choices made so far.</param>
        /// <param name="reachedTarget">Calculated by the generator - determines if the target of the preview has been reached.</param>
        internal PreviewRouteNode (INode node, int [] choicesMadeSoFar, bool reachedTarget)
        {
            Node = node;
            ChoicesMadeSoFar = choicesMadeSoFar;
            ReachedTarget = reachedTarget;
        }

        /// <summary>
        /// Gets the design node at this step of the route.
        /// </summary>
        /// <remarks>
        /// This is a simple text node or a paragraph break node.
        /// </remarks>
        /// <value>The node.</value>
        public INode Node { get; private set; }

        /// <summary>
        /// Gets all of the choices made so far in this route.
        /// </summary>
        /// <value>The choices made so far.</value>
        public int [] ChoicesMadeSoFar { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="T:TextOn.Generation.PreviewRouteNode"/> reached its target.
        /// </summary>
        /// <value><c>true</c> if reached target; otherwise, <c>false</c>.</value>
        public bool ReachedTarget { get; private set; }
    }
}
