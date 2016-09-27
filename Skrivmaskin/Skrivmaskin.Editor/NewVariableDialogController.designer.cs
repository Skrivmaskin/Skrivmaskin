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
	[Register ("NewVariableDialogController")]
	partial class NewVariableDialogController
	{
		[Outlet]
		AppKit.NSTextField VariableDescription { get; set; }

		[Outlet]
		AppKit.NSTextField VariableName { get; set; }

		[Outlet]
		AppKit.NSTextField VariableSuggestion { get; set; }

		[Action ("Cancel_Pressed:")]
		partial void Cancel_Pressed (Foundation.NSObject sender);

		[Action ("OK_Pressed:")]
		partial void OK_Pressed (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (VariableName != null) {
				VariableName.Dispose ();
				VariableName = null;
			}

			if (VariableDescription != null) {
				VariableDescription.Dispose ();
				VariableDescription = null;
			}

			if (VariableSuggestion != null) {
				VariableSuggestion.Dispose ();
				VariableSuggestion = null;
			}
		}
	}
}
