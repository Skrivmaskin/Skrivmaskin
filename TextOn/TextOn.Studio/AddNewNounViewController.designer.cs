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
	[Register ("AddNewNounViewController")]
	partial class AddNewNounViewController
	{
		[Outlet]
		AppKit.NSButton AcceptsUserValueButton { get; set; }

		[Outlet]
		AppKit.NSTextField DescriptionTextField { get; set; }

		[Outlet]
		AppKit.NSTextField NameTextField { get; set; }

		[Action ("Cancel_Clicked:")]
		partial void Cancel_Clicked (Foundation.NSObject sender);

		[Action ("OK_Clicked:")]
		partial void OK_Clicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (AcceptsUserValueButton != null) {
				AcceptsUserValueButton.Dispose ();
				AcceptsUserValueButton = null;
			}

			if (DescriptionTextField != null) {
				DescriptionTextField.Dispose ();
				DescriptionTextField = null;
			}

			if (NameTextField != null) {
				NameTextField.Dispose ();
				NameTextField = null;
			}
		}
	}
}
