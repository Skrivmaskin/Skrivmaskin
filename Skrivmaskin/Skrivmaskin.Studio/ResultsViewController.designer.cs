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
		Skrivmaskin.Studio.SkrivmaskinResultsTextView ResultsView { get; set; }

		[Action ("Generate_Clicked:")]
		partial void Generate_Clicked (Foundation.NSObject sender);

		[Action ("Regenerate_Clicked:")]
		partial void Regenerate_Clicked (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ResultsView != null) {
				ResultsView.Dispose ();
				ResultsView = null;
			}
		}
	}
}
