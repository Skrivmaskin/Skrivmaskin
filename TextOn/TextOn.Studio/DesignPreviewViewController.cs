// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Linq;
using Foundation;
using AppKit;
using TextOn.Design;
using TextOn.Generation;
using TextOn.Services;
using System.Collections.Generic;
using TextOn.Compiler;

namespace TextOn.Studio
{
	public partial class DesignPreviewViewController : NSViewController
	{
		public DesignPreviewViewController (IntPtr handle) : base (handle)
		{
		}

        private CentralViewController centralViewController = null;
        internal void SetControllerLinks (CentralViewController cvc)
        {
            Console.Error.WriteLine ("Preview SetControllerLinks");

            centralViewController = cvc;
        }

        bool firstAppearance = true;
        public override void ViewDidAppear ()
        {
            Console.Error.WriteLine ("Preview ViewDidAppear");

            base.ViewDidAppear ();

            if (firstAppearance) {
                ChoiceFixSlider.MaxValue = 1;
                ChoiceFixSlider.TickMarksCount = 2;
                ChoiceFixSlider.IntValue = 1;
                ChoiceFixSlider.Enabled = false;
                firstAppearance = false;
            }
        }

        private string NodeString (PreviewRouteNode node)
        {
            switch (node.Node.Type) {
            case NodeType.Text:
                return ((TextNode)node.Node).Text;
            case NodeType.ParagraphBreak:
                return "<pr/>";
            default:
                throw new ApplicationException ("Unexpected node type " + node.Node.Type);
            }
        }

        public void UpdatePreview (PreviewPartialRouteChoiceNode [] partialRoute)
        {
            Console.Error.WriteLine ("Preview UpdatePreview");

            var rootNode = centralViewController.Template.Definition;
            var compiledTemplate = centralViewController.CompiledTemplate;
            if (rootNode == null) {
                nodes = new PreviewRouteNode [0];
                this.partialRoute = new PreviewPartialRouteChoiceNode [0];
                TextView.SetValue ("", nodes, null);
                ChoiceFixSlider.MaxValue = 1;
                ChoiceFixSlider.TickMarksCount = 2;
                ChoiceFixSlider.IntValue = 1;
                ChoiceFixSlider.Enabled = false;
            } else {
                if (partialRoute.Length == 0) {
                    if (nodes.Length != 0) {
                        // fix the choices at the slider level
                        var numChoicesToKeep = ChoiceFixSlider.MaxValue - ChoiceFixSlider.IntValue;
                        var fixedChoices = nodes.SkipWhile ((p) => p.ChoicesMadeSoFar.Length < numChoicesToKeep).First ().ChoicesMadeSoFar;
                        nodes = generator.GenerateWithFixedChoices (rootNode, fixedChoices).ToArray ();
                    } else {
                        nodes = generator.GenerateWithFixedChoices (rootNode, new int [0]).ToArray();
                    }
                } else
                    nodes = generator.GenerateWithPartialRoute (rootNode, partialRoute).ToArray ();
                TextView.SetValue (string.Join ("\n", nodes.Select (NodeString)), nodes, compiledTemplate);
                if ((nodes.Length == 0) || (nodes [nodes.Length - 1].ChoicesMadeSoFar.Length == 0)) {
                    this.partialRoute = new PreviewPartialRouteChoiceNode [0];
                    ChoiceFixSlider.MaxValue = 1;
                    ChoiceFixSlider.TickMarksCount = 2;
                    ChoiceFixSlider.IntValue = 1;
                    ChoiceFixSlider.Enabled = false;
                } else {
                    this.partialRoute = partialRoute;
                    var finalChoices = nodes [nodes.Length - 1].ChoicesMadeSoFar;
                    var maxNumChoices = finalChoices.Length;
                    ChoiceFixSlider.MaxValue = maxNumChoices;
                    ChoiceFixSlider.TickMarksCount = maxNumChoices + 1;
                    var lastNonReached = nodes.LastOrDefault ((p) => !p.ReachedTarget);
                    var numChoicesMadeToTargetNode = (lastNonReached == null) ? 0 : lastNonReached.ChoicesMadeSoFar.Length;
                    ChoiceFixSlider.IntValue = maxNumChoices - numChoicesMadeToTargetNode; // set to fix the choices
                    ChoiceFixSlider.Enabled = true;
                }
            }
        }

        partial void Slider_Moved (NSObject sender)
        {
            Console.Error.WriteLine ("Preview Slider_Moved");

            // this signifies that the fixed choices setup will be used instead.
            if (partialRoute.Length > 0)
                partialRoute = new PreviewPartialRouteChoiceNode [0];

            // update the highlighting
            TextView.DoHighlightBackground = GetDoHighlightPredicate ();
            TextView.Highlight ();
        }

        partial void Respin_Clicked (NSObject sender)
        {
            Console.Error.WriteLine ("Preview Respin_Clicked");

            if (centralViewController != null) {
                UpdatePreview (partialRoute);
            }
        }

        private Func<PreviewRouteNode, bool> GetDoHighlightPredicate ()
        {
            if (partialRoute.Length == 0) {
                var numChoicesToKeep = ChoiceFixSlider.MaxValue - ChoiceFixSlider.IntValue;
                return ((n) => n.ChoicesMadeSoFar.Length <= numChoicesToKeep);
            } else {
                return ((n) => !n.ReachedTarget);
            }
        }

        private PreviewRouteNode [] nodes = new PreviewRouteNode [0];

        private PreviewPartialRouteChoiceNode [] partialRoute = new PreviewPartialRouteChoiceNode [0];

        private readonly TextOnPreviewGenerator generator = new TextOnPreviewGenerator (new RandomChooser ());
	}
}
