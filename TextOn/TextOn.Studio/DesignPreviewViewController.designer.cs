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
	[Register ("DesignPreviewViewController")]
	partial class DesignPreviewViewController
	{
		[Outlet]
		TextOn.Studio.DesignPreviewTextView TextView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TextView != null) {
				TextView.Dispose ();
				TextView = null;
			}
		}
	}
}
