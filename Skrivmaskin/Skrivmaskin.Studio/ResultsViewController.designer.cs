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
	[Register ("ResultsViewController")]
	partial class ResultsViewController
	{
		[Outlet]
		AppKit.NSTextView Results { get; set; }

		[Action ("Generate_Clicked:")]
		partial void Generate_Clicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (Results != null) {
				Results.Dispose ();
				Results = null;
			}
		}
	}
}