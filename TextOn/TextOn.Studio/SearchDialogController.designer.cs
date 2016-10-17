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
	[Register ("SearchDialogController")]
	partial class SearchDialogController
	{
		[Outlet]
		AppKit.NSButton ReplaceGloballyButton { get; set; }

		[Outlet]
		TextOn.Studio.TextOnTextView ReplaceTextView { get; set; }

		[Outlet]
		TextOn.Studio.TextOnTextView SearchTextView { get; set; }

		[Action ("Cancel_Clicked:")]
		partial void Cancel_Clicked (Foundation.NSObject sender);

		[Action ("OK_Clicked:")]
		partial void OK_Clicked (Foundation.NSObject sender);

		[Action ("ReplaceGlobally_Clicked:")]
		partial void ReplaceGlobally_Clicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ReplaceGloballyButton != null) {
				ReplaceGloballyButton.Dispose ();
				ReplaceGloballyButton = null;
			}

			if (ReplaceTextView != null) {
				ReplaceTextView.Dispose ();
				ReplaceTextView = null;
			}

			if (SearchTextView != null) {
				SearchTextView.Dispose ();
				SearchTextView = null;
			}
		}
	}
}
