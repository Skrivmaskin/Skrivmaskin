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
	[Register ("DesignViewController")]
	partial class DesignViewController
	{
		[Outlet]
		TextOn.Studio.DesignOutlineView OutlineView { get; set; }

		[Outlet]
		TextOn.Studio.DesignTreeController TreeController { get; set; }

		[Action ("Add_ParagraphBreak:")]
		partial void Add_ParagraphBreak (Foundation.NSObject sender);

		[Action ("HideShowPreview_Clicked:")]
		partial void HideShowPreview_Clicked (Foundation.NSObject sender);

		[Action ("MoveDown_Clicked:")]
		partial void MoveDown_Clicked (Foundation.NSObject sender);

		[Action ("MoveUp_Clicked:")]
		partial void MoveUp_Clicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (OutlineView != null) {
				OutlineView.Dispose ();
				OutlineView = null;
			}

			if (TreeController != null) {
				TreeController.Dispose ();
				TreeController = null;
			}
		}
	}
}
