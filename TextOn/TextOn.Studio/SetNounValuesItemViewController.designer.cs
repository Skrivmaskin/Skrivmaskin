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
		AppKit.NSTextView DescriptionTextView { get; set; }

		[Outlet]
		AppKit.NSTextField NounNameTextField { get; set; }

		[Outlet]
		AppKit.NSImageView StatusImage { get; set; }

		[Outlet]
		AppKit.NSComboBox SuggestionsComboBox { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DescriptionTextView != null) {
				DescriptionTextView.Dispose ();
				DescriptionTextView = null;
			}

			if (NounNameTextField != null) {
				NounNameTextField.Dispose ();
				NounNameTextField = null;
			}

			if (StatusImage != null) {
				StatusImage.Dispose ();
				StatusImage = null;
			}

			if (SuggestionsComboBox != null) {
				SuggestionsComboBox.Dispose ();
				SuggestionsComboBox = null;
			}
		}
	}
}
