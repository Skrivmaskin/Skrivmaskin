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
	[Register ("SetNounValuesItemViewController")]
	partial class SetNounValuesItemViewController
	{
		[Outlet]
		AppKit.NSTextView DescriptionTextField { get; set; }

		[Outlet]
		AppKit.NSTextField NounNameTextField { get; set; }

		[Outlet]
		AppKit.NSImageView StatusImageView { get; set; }

		[Outlet]
		AppKit.NSComboBox SuggestionsComboBox { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (SuggestionsComboBox != null) {
				SuggestionsComboBox.Dispose ();
				SuggestionsComboBox = null;
			}

			if (DescriptionTextField != null) {
				DescriptionTextField.Dispose ();
				DescriptionTextField = null;
			}

			if (NounNameTextField != null) {
				NounNameTextField.Dispose ();
				NounNameTextField = null;
			}

			if (StatusImageView != null) {
				StatusImageView.Dispose ();
				StatusImageView = null;
			}
		}
	}
}
