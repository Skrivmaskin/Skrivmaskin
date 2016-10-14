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
	[Register ("SetNounValuesViewController")]
	partial class SetNounValuesViewController
	{
		[Outlet]
		AppKit.NSTableView SetNounValuesTableView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (SetNounValuesTableView != null) {
				SetNounValuesTableView.Dispose ();
				SetNounValuesTableView = null;
			}
		}
	}
}
