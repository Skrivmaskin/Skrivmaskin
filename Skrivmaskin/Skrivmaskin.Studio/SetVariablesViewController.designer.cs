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
	[Register ("SetVariablesViewController")]
	partial class SetVariablesViewController
	{
		[Outlet]
		AppKit.NSTableView SetVariables { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (SetVariables != null) {
				SetVariables.Dispose ();
				SetVariables = null;
			}
		}
	}
}
