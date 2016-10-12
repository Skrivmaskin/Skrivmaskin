using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace TextOn.Studio
{
    public partial class NounItemView : AppKit.NSView
    {
        #region Constructors

        // Called when created from unmanaged code
        public NounItemView (IntPtr handle) : base (handle)
        {
            Initialize ();
        }

        // Called when created directly from a XIB file
        [Export ("initWithCoder:")]
        public NounItemView (NSCoder coder) : base (coder)
        {
            Initialize ();
        }

        // Shared initialization code
        void Initialize ()
        {
        }

        #endregion
    }
}
