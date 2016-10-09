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
		AppKit.NSSlider ChoiceFixSlider { get; set; }

		[Outlet]
		TextOn.Studio.DesignPreviewTextView TextView { get; set; }

		[Action ("Respin_Clicked:")]
		partial void Respin_Clicked (Foundation.NSObject sender);

		[Action ("Slider_Moved:")]
		partial void Slider_Moved (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ChoiceFixSlider != null) {
				ChoiceFixSlider.Dispose ();
				ChoiceFixSlider = null;
			}

			if (TextView != null) {
				TextView.Dispose ();
				TextView = null;
			}
		}
	}
}
