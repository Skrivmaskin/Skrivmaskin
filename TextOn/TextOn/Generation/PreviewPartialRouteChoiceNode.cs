using System;
using TextOn.Design;
namespace TextOn.Generation
{
    public sealed class PreviewPartialRouteChoiceNode
    {
        public PreviewPartialRouteChoiceNode (ChoiceNode choiceNode, int decision, int choicesMade, INode targetNode)
        {
            ChoiceNode = choiceNode;
            Decision = decision;
            ChoicesMade = choicesMade;
            TargetNode = targetNode;
        }

        public ChoiceNode ChoiceNode { get; private set; }
        public int Decision { get; private set; }
        public int ChoicesMade { get; private set; }
        public INode TargetNode { get; private set; }
    }
}
