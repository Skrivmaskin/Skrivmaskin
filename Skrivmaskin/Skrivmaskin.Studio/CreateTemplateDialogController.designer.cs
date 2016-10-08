// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Skrivmaskin.Studio
{
	[Register ("CreateTemplateDialogController")]
	partial class CreateTemplateDialogController
	{
		[Outlet]
		AppKit.NSTextView Sample { get; set; }

		[Action ("Cancel_Clicked:")]
		partial void Cancel_Clicked (Foundation.NSObject sender);

		[Action ("OK_Clicked:")]
		partial void OK_Clicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (Sample != null) {
				Sample.Dispose ();
				Sample = null;
			}
		}
	}
}
