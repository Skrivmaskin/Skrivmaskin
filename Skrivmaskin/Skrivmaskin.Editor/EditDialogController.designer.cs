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
	[Register ("EditDialogController")]
	partial class EditDialogController
	{
		[Outlet]
		AppKit.NSTextView NewDetails { get; set; }

		[Action ("Cancel_Pressed:")]
		partial void Cancel_Pressed (Foundation.NSObject sender);

		[Action ("OK_Pressed:")]
		partial void OK_Pressed (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (NewDetails != null) {
				NewDetails.Dispose ();
				NewDetails = null;
			}
		}
	}
}
