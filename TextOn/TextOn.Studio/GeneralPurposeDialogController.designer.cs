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
	[Register ("GeneralPurposeDialogController")]
	partial class GeneralPurposeDialogController
	{
		[Outlet]
		TextOn.Studio.TextOnTextView DetailsTextView { get; set; }

		[Outlet]
		AppKit.NSButton IsActive { get; set; }

		[Outlet]
		AppKit.NSTextField NameTextField { get; set; }

		[Outlet]
		AppKit.NSTextField SuggestionTextField { get; set; }

		[Action ("Cancel_Clicked:")]
		partial void Cancel_Clicked (Foundation.NSObject sender);

		[Action ("OK_Clicked:")]
		partial void OK_Clicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (DetailsTextView != null) {
				DetailsTextView.Dispose ();
				DetailsTextView = null;
			}

			if (NameTextField != null) {
				NameTextField.Dispose ();
				NameTextField = null;
			}

			if (SuggestionTextField != null) {
				SuggestionTextField.Dispose ();
				SuggestionTextField = null;
			}

			if (IsActive != null) {
				IsActive.Dispose ();
				IsActive = null;
			}
		}
	}
}
