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
	[Register ("DesignViewController")]
	partial class DesignViewController
	{
		[Outlet]
		Foundation.NSObject DesignOutlineView { get; set; }

		[Outlet]
		AppKit.NSTreeController TreeController { get; set; }

		[Action ("Add_ParagraphBreak:")]
		partial void Add_ParagraphBreak (Foundation.NSObject sender);

		[Action ("ConvertToChoice:")]
		partial void ConvertToChoice (Foundation.NSObject sender);

		[Action ("ConvertToSequential:")]
		partial void ConvertToSequential (Foundation.NSObject sender);

		void ReleaseDesignerOutlets ()
		{
			if (DesignOutlineView != null) {
				DesignOutlineView.Dispose ();
				DesignOutlineView = null;
			}

			if (TreeController != null) {
				TreeController.Dispose ();
				TreeController = null;
			}
		}
	}
}
