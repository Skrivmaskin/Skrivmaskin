using System;
using TextOn.Design;
namespace TextOn.Generation
{
    public sealed class PreviewPartialRouteChoiceNode
    {
        public PreviewPartialRouteChoiceNode (DesignNode choiceNode, int decision, int choicesMade, DesignNode targetNode)
        {
            ChoiceNode = choiceNode;
            Decision = decision;
            ChoicesMade = choicesMade;
            TargetNode = targetNode;
        }

        public DesignNode ChoiceNode { get; private set; }
        public int Decision { get; private set; }
        public int ChoicesMade { get; private set; }
        public DesignNode TargetNode { get; private set; }
    }
}
