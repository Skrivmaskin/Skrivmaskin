using System;
using AppKit;
using CoreGraphics;
using Foundation;
using System.Collections;
using System.Collections.Generic;

namespace Skrivmaskin.Editor
{
    public class SkrivmaskinOutlineViewDelegate : NSOutlineViewDelegate
    {
        private const string CellIdentifier = "SkrivmaskinCell";
        private SkrivmaskinOutlineViewDataSource DataSource;

        public SkrivmaskinOutlineViewDelegate (SkrivmaskinOutlineViewDataSource datasource)
        {
            this.DataSource = datasource;
        }

        public override NSView GetView (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
        {
            // This pattern allows you reuse existing views when they are no-longer in use.
            // If the returned view is null, you instance up a new view
            // If a non-null view is returned, you modify it enough to reflect the new data
            NSTextField view = (NSTextField)outlineView.MakeView (CellIdentifier, this);
            if (view == null) {
                view = new NSTextField ();
                view.Identifier = CellIdentifier;
                view.BackgroundColor = NSColor.Clear;
                view.Bordered = false;
                view.Selectable = false;
                view.Editable = false;
            }

            // Cast item
            var node = item as Node;

            // Setup view based on the column selected
            switch (tableColumn.Title) {
            case "Title":
                view.StringValue = node.Title;
                break;
            case "Description":
                view.StringValue = node.Description;
                break;
            }

            return view;
        }

    }
}
