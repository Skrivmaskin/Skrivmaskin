using System;
using System.Collections.Generic;
using AppKit;
using Foundation;

namespace Skrivmaskin.Studio
{
    [Register (nameof (DesignOutlineView))]
    public class DesignOutlineView : NSOutlineView, INSDraggingSource, INSDraggingDestination
    {
        public DesignOutlineView (IntPtr handle) : base (handle)
        {
            DataSource = new DesignOutlineViewDataSource ();
            Delegate = new DesignOutlineViewDelegate ();
        }

        public override void ViewDidMoveToSuperview ()
        {
            base.ViewDidMoveToSuperview ();

            RegisterForDraggedTypes (new string [] { NSPasteboard.NSStringType });
        }

        internal DesignTreeController TreeController = null;

        public override void MouseDragged (NSEvent theEvent)
        {
            if (TreeController == null) return;
            if (TreeController.SelectionIndexPaths.Length != 1) return;

            // Pick up the previous selected index paths, and add it into the pasteboard in some friendly way.
            var sip = TreeController.SelectionIndexPaths [0];
            var draggingItems = new List<NSDraggingItem> ();
            for (nint i = 0; i < sip.Length; i++) {
                var n = (NSString)(sip.IndexAtPosition (i).ToString ());
                var draggingItem = new NSDraggingItem (n);
                draggingItems.Add (draggingItem);
            }
            BeginDraggingSession (draggingItems.ToArray (), theEvent, this);
        }

        public override NSDragOperation DraggingEntered (NSDraggingInfo sender)
        {
            // When we start dragging, inform the system that we will be handling this as
            // a cut/paste
            return NSDragOperation.Move;
        }

        public override void DraggingEnded (NSDraggingInfo sender)
        {
            var pasteboard = sender.GetDraggingPasteboard ();
            var items = pasteboard.PasteboardItems;
            base.DraggingEnded (sender);
        }
    }
}
