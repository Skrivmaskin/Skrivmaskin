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

        void OnModifiedClick (INode designNode)
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

            parent = cvc;
        }

        public bool canRegenerate {
            [Export (nameof (canRegenerate))]
            get {
                return (parent != null && parent.CompiledTemplate != null && generator.CanGenerate (parent.CompiledTemplate));
            }
        }

        readonly TextOnGenerator generator = new TextOnGenerator (new RandomChooser (), new SingleSpaceUnixGeneratorConfig ());

        public void Generate (bool isRegen)
        {
            WillChangeValue (nameof (canRegenerate));
            if (parent.CompiledTemplate != null) {
                if (isRegen) {
                    ResultsView.Output = (generator.Regenerate (parent.CompiledTemplate, new DictionaryBackedVariableSubstituter (parent.VariableValues)));
                } else {
                    ResultsView.Output = (generator.Generate (parent.CompiledTemplate, new DictionaryBackedVariableSubstituter (parent.VariableValues)));
                }

            } else {
                ResultsView.Output = null;
            }
            DidChangeValue (nameof (canRegenerate));
        }
    }
}
