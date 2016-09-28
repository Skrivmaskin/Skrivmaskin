// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Skrivmaskin.Editor
{
	[Register ("GeneralPurposeDialogController")]
	partial class GeneralPurposeDialogController
	{
		[Outlet]
		AppKit.NSButton Active_Checked { get; set; }

		[Outlet]
		AppKit.NSTextField DetailsText { get; set; }

		[Outlet]
		AppKit.NSTextField NameText { get; set; }

		[Outlet]
		AppKit.NSTextField SuggestionText { get; set; }

		[Action ("Cancel_Clicked:")]
		partial void Cancel_Clicked (Foundation.NSObject sender);

		[Action ("OK_Clicked:")]
		partial void OK_Clicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (Active_Checked != null) {
				Active_Checked.Dispose ();
				Active_Checked = null;
			}

			if (NameText != null) {
				NameText.Dispose ();
				NameText = null;
			}

			if (DetailsText != null) {
				DetailsText.Dispose ();
				DetailsText = null;
			}

			if (SuggestionText != null) {
				SuggestionText.Dispose ();
				SuggestionText = null;
			}
		}
	}
}
