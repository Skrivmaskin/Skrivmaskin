using System;
using TextOn.Design;
namespace TextOn.Generation
{
    public sealed class PreviewPartialRouteChoiceNode
    {
        public PreviewPartialRouteChoiceNode (ChoiceNode choiceNode, int decision)
        {
            ChoiceNode = choiceNode;
            Decision = decision;
        }

        public ChoiceNode ChoiceNode { get; private set;}
        public int Decision { get; private set;}
    }
}
