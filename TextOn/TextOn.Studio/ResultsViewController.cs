// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using AppKit;
using TextOn.Generation;
using TextOn.Services;
using System.Collections.Generic;
using TextOn.Compiler;
using TextOn.Design;

namespace TextOn.Studio
{
    public partial class ResultsViewController : NSViewController
    {
        public ResultsViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad ()
        {
            Console.Error.WriteLine ("Results ViewDidLoad");

            base.ViewDidLoad ();

            // Set the initial value for the label
            ResultsView.Output = null;

            // Listen to modified clicks from the user.
            ResultsView.ModifiedClick += OnModifiedClick;
        }

        void OnModifiedClick (DesignNode designNode)
        {
            if (parent != null) {
                parent.NavigateAndSelectDesignNode (designNode);
            }
        }

        partial void Regenerate_Clicked (NSObject sender)
        {
            Generate (true);
        }

        partial void Generate_Clicked (Foundation.NSObject sender)
        {
            Generate (false);
        }

        private CentralViewController parent = null;
        internal void SetControllerLinks (CentralViewController cvc)
        {
            Console.Error.WriteLine ("Results SetControllerLinks");

            WillChangeValue (nameof (generateTooltip));
            WillChangeValue (nameof (regenerateTooltip));
            parent = cvc;
            DidChangeValue (nameof (generateTooltip));
            DidChangeValue (nameof (regenerateTooltip));
        }

        public bool canGenerate {
            [Export (nameof (canGenerate))]
            get {
                return parent != null && parent.CompiledTemplate != null && parent.AllValuesAreSet && generator.CanGenerate (parent.CompiledTemplate);
            }
        }

        public bool canRegenerate {
            [Export (nameof (canRegenerate))]
            get {
                return (canGenerate && generator.CanRegenerate (parent.CompiledTemplate));
            }
        }

        public string generateTooltip {
            [Export (nameof (generateTooltip))]
            get {
                if (parent == null) return "Template not set up";
                if (parent.CompiledTemplate == null) return "Template not set up";
                if (generator.IsMissingRequiredNounDefinitions (parent.CompiledTemplate)) return "Template is missing Noun definitions";
                if (!generator.CanGenerate (parent.CompiledTemplate)) return "Template has errors to fix";
                if (!parent.AllValuesAreSet) return "User values missing";
                return "Generate results";
            }
        }

        public string regenerateTooltip {
            [Export (nameof (regenerateTooltip))]
            get {
                if (parent == null) return "Template not set up";
                if (parent.CompiledTemplate == null) return "Template not set up";
                if (generator.IsMissingRequiredNounDefinitions (parent.CompiledTemplate)) return "Template is missing Noun definitions";
                if (!generator.CanGenerate (parent.CompiledTemplate)) return "Template has errors to fix";
                if (!parent.AllValuesAreSet) return "User values missing";
                if (!generator.CanRegenerate (parent.CompiledTemplate)) return "Haven't got a seed to rerun with";
                return "Regenerate results using the same last seed";
            }
        }

        readonly TextOnGenerator generator = new TextOnGenerator (new RandomChooser (), new SingleSpaceUnixGeneratorConfig ());

        public void Generate (bool isRegen)
        {
            WillChangeValue (nameof (canRegenerate));
            if (parent.CompiledTemplate != null) {
                if (isRegen) {
                    ResultsView.Output = (generator.Regenerate (parent.CompiledTemplate, new DictionaryBackedVariableSubstituter (parent.NounValues)));
                } else {
                    ResultsView.Output = (generator.Generate (parent.CompiledTemplate, new DictionaryBackedVariableSubstituter (parent.NounValues)));
                }

            } else {
                ResultsView.Output = null;
            }
            DidChangeValue (nameof (canRegenerate));
        }

        internal void DidChangeCanGenerate ()
        {
            DidChangeValue (nameof (canGenerate));
            DidChangeValue (nameof (canRegenerate));
            DidChangeValue (nameof (generateTooltip));
            DidChangeValue (nameof (regenerateTooltip));
        }

        internal void WillChangeCanGenerate ()
        {
            WillChangeValue (nameof (canGenerate));
            WillChangeValue (nameof (canRegenerate));
            WillChangeValue (nameof (generateTooltip));
            WillChangeValue (nameof (regenerateTooltip));
        }
	}
}
