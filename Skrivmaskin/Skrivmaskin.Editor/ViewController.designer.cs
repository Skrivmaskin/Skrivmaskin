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
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		AppKit.NSTableColumn DescriptionTableColumn { get; set; }

		[Outlet]
		AppKit.NSOutlineView SkrivmaskinOutline { get; set; }

		[Outlet]
		AppKit.NSTableColumn TitleTableColumn { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (DescriptionTableColumn != null) {
				DescriptionTableColumn.Dispose ();
				DescriptionTableColumn = null;
			}

			if (TitleTableColumn != null) {
				TitleTableColumn.Dispose ();
				TitleTableColumn = null;
			}

			if (SkrivmaskinOutline != null) {
				SkrivmaskinOutline.Dispose ();
				SkrivmaskinOutline = null;
			}
		}
	}
}
