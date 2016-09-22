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
		AppKit.NSTextField Results { get; set; }

		[Outlet]
		AppKit.NSTableView VariablesOutline { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (VariablesOutline != null) {
				VariablesOutline.Dispose ();
				VariablesOutline = null;
			}

			if (Results != null) {
				Results.Dispose ();
				Results = null;
			}
		}
	}
}
