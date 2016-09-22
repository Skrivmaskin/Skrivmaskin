using System;
using System.Collections.Generic;
using AppKit;
using CoreGraphics;
using Foundation;

namespace Skrivmaskin.Editor
{
    // Data sources walk a given data source and respond to questions from AppKit to generat    // the data used in your Delegate. In this example, we walk a simple tree.
    public class SkrivmaskinOutlineViewDataSource : NSOutlineViewDataSource
    {
        Node parentNode;
        public SkrivmaskinOutlineViewDataSource (Node node)
        {
            parentNode = node;
        }

        public override nint GetChildrenCount (NSOutlineView outlineView, NSObject item)
        {
            // If item is null, we are referring to the root element in the tree
            item = item == null ? parentNode : item;
            return ((Node)item).ChildCount;
        }

        public override NSObject GetChild (NSOutlineView outlineView, nint childIndex, NSObject item)
        {
            // If item is null, we are referring to the root element in the tree
            item = item == null ? parentNode : item;
            return ((Node)item).GetChild ((int)childIndex);
        }

        public override bool ItemExpandable (NSOutlineView outlineView, NSObject item)
        {
            // If item is null, we are referring to the root element in the tree
            item = item == null ? parentNode : item;
            return !((Node)item).IsLeaf;
        }
    }
}
