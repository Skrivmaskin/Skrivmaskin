using System;
using System.Collections.Generic;
using TextOn.Interfaces;
using TextOn.Design;
using TextOn.Generation;

namespace TextOn.Services
{
    /// <summary>
    /// Wraps another random chooser and provides functionality for fixing random choices.
    /// </summary>
    public sealed class ChoiceFixingRandomChooser
    {
        private readonly IRandomChooser wrappedRandomChooser;
        public ChoiceFixingRandomChooser (IRandomChooser wrappedRandomChooser)
        {
            this.wrappedRandomChooser = wrappedRandomChooser;
        }

        private readonly Queue<int> fixedChoices = new Queue<int> ();
        private void SetupWithFixedChoices (int [] choices)
        {
            if (this.fixedChoices.Count > 0) throw new ApplicationException ("Queue should be empty on begin");
            if (this.partialRoute.Count > 0) throw new ApplicationException ("Partial route should be empty on begin");
            foreach (var item in choices) {
                fixedChoices.Enqueue (item);
            }
            this.choicesMadeList.Clear ();
        }

        private readonly Queue<PreviewPartialRouteChoiceNode> partialRoute = new Queue<PreviewPartialRouteChoiceNode> ();
        private void SetupWithPartialRoute (PreviewPartialRouteChoiceNode [] route)
        {
            if (this.fixedChoices.Count > 0) throw new ApplicationException ("Queue should be empty on begin");
            if (this.partialRoute.Count > 0) throw new ApplicationException ("Partial route should be empty on begin");
            foreach (var item in route) {
                partialRoute.Enqueue (item);
            }
            this.choicesMadeList.Clear ();
        }

        public void BeginWithFixedChoices (int [] choices)
        {
            SetupWithFixedChoices (choices);
            wrappedRandomChooser.Begin ();
        }

        public void BeginWithPartialRoute (PreviewPartialRouteChoiceNode [] route)
        {
            SetupWithPartialRoute (route);
            wrappedRandomChooser.Begin ();
        }

        // For testing.
        internal void BeginWithFixedChoicesAndSeed (int [] choices, int seed)
        {
            SetupWithFixedChoices (choices);
            wrappedRandomChooser.BeginWithSeed (seed);
        }

        // For testing.
        internal void BeginWithPartialRouteAndSeed (PreviewPartialRouteChoiceNode [] route, int seed)
        {
            SetupWithPartialRoute (route);
            wrappedRandomChooser.BeginWithSeed (seed);
        }

        private List<int> choicesMadeList = new List<int> ();
        public int [] ChoicesMade {
            get {
                return choicesMadeList.ToArray ();
            }
        }

        public void Begin ()
        {
            wrappedRandomChooser.Begin ();
        }

        public void BeginWithSeed (int seed)
        {
            wrappedRandomChooser.BeginWithSeed (seed);
        }

        public int Choose (ChoiceNode node, int numOptions, out bool reachedTarget)
        {
            int choice;
            if (fixedChoices.Count > 0) {
                choice = fixedChoices.Dequeue ();
                reachedTarget = false;
                if (choice >= numOptions) throw new ApplicationException ("Misuse of ChoiceFixingRandomChooser - the tree has changed, this should be reset");
            } else if (partialRoute.Count > 0 && Object.ReferenceEquals (partialRoute.Peek ().ChoiceNode, node)) {
                choice = partialRoute.Dequeue ().Decision;
                reachedTarget = false;
                if (choice >= numOptions) throw new ApplicationException ("Misuse of ChoiceFixingRandomChooser - the tree has changed, this should be reset");
            } else {
                choice = wrappedRandomChooser.Choose (numOptions);
                reachedTarget = partialRoute.Count == 0 && fixedChoices.Count == 0;
            }
            choicesMadeList.Add (choice);
            return choice;
        }

        public void End ()
        {
            if (this.fixedChoices.Count > 0) throw new ApplicationException ("Queue should be empty on end");
            wrappedRandomChooser.End ();
        }
    }
}
