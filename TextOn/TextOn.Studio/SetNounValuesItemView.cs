using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace TextOn.Studio
{
    public partial class SetNounValuesItemView : AppKit.NSView
    {
        #region Constructors

        // Called when created from unmanaged code
        public SetNounValuesItemView (IntPtr handle) : base (handle)
        {
            Initialize ();
        }

        // Called when created directly from a XIB file
        [Export ("initWithCoder:")]
        public SetNounValuesItemView (NSCoder coder) : base (coder)
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
