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
	[Register ("RunViewController")]
	partial class RunViewController
	{
		[Outlet]
		AppKit.NSTextView Results { get; set; }

		[Outlet]
		AppKit.NSTableView Variables { get; set; }

		[Action ("GenerateClicked:")]
		partial void GenerateClicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (Results != null) {
				Results.Dispose ();
				Results = null;
			}

			if (Variables != null) {
				Variables.Dispose ();
				Variables = null;
			}
		}
	}
}
