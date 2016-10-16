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
	[Register ("ManageConstraintsDialogController")]
	partial class ManageConstraintsDialogController
	{
		[Outlet]
		AppKit.NSButton ApplyToFuture { get; set; }

		[Outlet]
		AppKit.NSTextField DescriptionTextField { get; set; }

		[Outlet]
		AppKit.NSTableView ManageConstraintsTableView { get; set; }

		[Outlet]
		AppKit.NSComboBox NounComboBox { get; set; }

		[Outlet]
		AppKit.NSComboBox ValueComboBox { get; set; }

		[Action ("AddRow_Clicked:")]
		partial void AddRow_Clicked (Foundation.NSObject sender);

		[Action ("Cancel_Clicked:")]
		partial void Cancel_Clicked (Foundation.NSObject sender);

		[Action ("OK_Clicked:")]
		partial void OK_Clicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ApplyToFuture != null) {
				ApplyToFuture.Dispose ();
				ApplyToFuture = null;
			}

			if (DescriptionTextField != null) {
				DescriptionTextField.Dispose ();
				DescriptionTextField = null;
			}

			if (ManageConstraintsTableView != null) {
				ManageConstraintsTableView.Dispose ();
				ManageConstraintsTableView = null;
			}

			if (NounComboBox != null) {
				NounComboBox.Dispose ();
				NounComboBox = null;
			}

			if (ValueComboBox != null) {
				ValueComboBox.Dispose ();
				ValueComboBox = null;
			}
		}
	}
}
