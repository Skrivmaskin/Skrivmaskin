// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using AppKit;

namespace TextOn.Studio
{
	public partial class DeleteSuggestionViewController : NSViewController
	{
		public DeleteSuggestionViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            Title = "Delete Suggestion?";
            DescriptionTextField.StringValue = "Are you sure that you want to delete the suggested value \"" + SuggestionValue + "\" for Noun [" + NounName + "]? This action is permanent and cannot be undone.";
        }

        public string NounName { get; internal set; }

        public string SuggestionValue { get; internal set; }

        public NSViewController Presentor { get; set; }

        private void CloseDialog ()
        {
            Presentor.DismissViewController (this);
        }

        partial void Cancel_Clicked (NSObject sender)
        {
            CloseDialog ();
        }

        partial void OK_Clicked (NSObject sender)
        {
            RaiseDialogAccepted ();
            CloseDialog ();
        }

        public EventHandler DialogAccepted;

        internal void RaiseDialogAccepted ()
        {
            if (this.DialogAccepted != null)
                this.DialogAccepted (this, EventArgs.Empty);
        }
	}
}
