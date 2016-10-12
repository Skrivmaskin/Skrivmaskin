using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace TextOn.Studio
{
    public partial class NounItemViewController : AppKit.NSCollectionViewItem
    {
        #region Constructors

        // Called when created from unmanaged code
        public NounItemViewController (IntPtr handle) : base (handle)
        {
            Initialize ();
        }

        // Called when created directly from a XIB file
        [Export ("initWithCoder:")]
        public NounItemViewController (NSCoder coder) : base (coder)
        {
            Initialize ();
        }

        // Call to load from the XIB/NIB file
        public NounItemViewController () : base ("NounItemView", NSBundle.MainBundle)
        {
            Initialize ();
        }

        // Shared initialization code
        void Initialize ()
        {
        }

        #endregion

        //strongly typed view accessor
        public new NounItemView View {
            get {
                return (NounItemView)base.View;
            }
        }
    }
}
