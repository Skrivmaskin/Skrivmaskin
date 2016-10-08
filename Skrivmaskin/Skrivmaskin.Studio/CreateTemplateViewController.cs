// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using AppKit;

namespace Skrivmaskin.Studio
{
	public partial class CreateTemplateViewController : NSViewController
	{
		public CreateTemplateViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            Title = TitleText;
            TitleLabel.StringValue = TitleText;
            DescriptionLabel.StringValue = DescriptionText;
        }

        public string TitleText { get; set; }

        public string DescriptionText { get; set; }

        public string SampleText {
            get {
                return TextView.Value;
            }
        }

        public NSViewController Presentor { get; set; }

        private void CloseDialog ()
        {
            Presentor.DismissViewController (this);
        }

        partial void Cancel_Clicked (Foundation.NSObject sender)
        {
            CloseDialog ();
        }

        partial void OK_Clicked (Foundation.NSObject sender)
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
