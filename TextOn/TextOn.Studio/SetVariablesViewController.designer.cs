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
	[Register ("SetVariablesViewController")]
	partial class SetVariablesViewController
	{
		[Outlet]
		AppKit.NSCollectionView SetVariablesCollectionView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (SetVariablesCollectionView != null) {
				SetVariablesCollectionView.Dispose ();
				SetVariablesCollectionView = null;
			}
		}
	}
}
