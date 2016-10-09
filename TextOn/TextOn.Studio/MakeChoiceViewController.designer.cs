// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace TextOn.Studio
{
	[Register ("MakeChoiceViewController")]
	partial class MakeChoiceViewController
	{
		[Outlet]
		AppKit.NSTextField DescriptionLabel { get; set; }

		[Outlet]
		AppKit.NSTextField NameTextField { get; set; }

		[Outlet]
		TextOn.Studio.TextOnTextView SampleTextView { get; set; }

		[Outlet]
		AppKit.NSTextField TitleLabel { get; set; }

		[Outlet]
		TextOn.Studio.TextOnTextView ToBeAddedTextView { get; set; }

		[Action ("AddOne_Clicked:")]
		partial void AddOne_Clicked (Foundation.NSObject sender);

		[Action ("Cancel_Clicked:")]
		partial void Cancel_Clicked (Foundation.NSObject sender);

		[Action ("OK_Clicked:")]
		partial void OK_Clicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (DescriptionLabel != null) {
				DescriptionLabel.Dispose ();
				DescriptionLabel = null;
			}

			if (SampleTextView != null) {
				SampleTextView.Dispose ();
				SampleTextView = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}

			if (ToBeAddedTextView != null) {
				ToBeAddedTextView.Dispose ();
				ToBeAddedTextView = null;
			}

			if (NameTextField != null) {
				NameTextField.Dispose ();
				NameTextField = null;
			}
		}
	}
}
