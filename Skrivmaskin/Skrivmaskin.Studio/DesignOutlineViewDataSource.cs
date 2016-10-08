using System;
using AppKit;

namespace Skrivmaskin.Studio
{
    public class DesignOutlineViewDataSource : NSOutlineViewDataSource
    {
        public DesignOutlineViewDataSource ()
        {
        }

        public override bool AcceptDrop (NSOutlineView outlineView, NSDraggingInfo info, Foundation.NSObject item, nint index)
        {
            return base.AcceptDrop (outlineView, info, item, index);
        }
    }
}
