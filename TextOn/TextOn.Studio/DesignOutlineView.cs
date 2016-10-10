using System;
using System.Collections.Generic;
using AppKit;
using Foundation;

namespace TextOn.Studio
{
    [Register (nameof (DesignOutlineView))]
    public class DesignOutlineView : NSOutlineView
    {
        public DesignOutlineView (IntPtr handle) : base (handle)
        {
        }

        internal DesignTreeController TreeController = null;
    }
}
