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
        private const string TitleIdentifier = "Title";
        private const string DescriptionIdentifier = "Description";

        private SkrivmaskinOutlineViewDataSource DataSource;

        public SkrivmaskinOutlineViewDelegate (SkrivmaskinOutlineViewDataSource datasource)
        {
            this.DataSource = datasource;
        }

        public override NSView GetView (NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
        {
            // Cast item
            var node = item as DesignNode;
            NSView retVal;

            // This pattern allows you reuse existing views when they are no-longer in use.
            // If the returned view is null, you instance up a new view
            // If a non-null view is returned, you modify it enough to reflect the new data
            //TODO find suitable icons for the different types
            NSTableCellView view = (NSTableCellView)outlineView.MakeView (CellIdentifier, this);
            if (view == null) {
                view = new NSTableCellView ();
                view.Identifier = CellIdentifier;
                view.ImageView = new NSImageView (new CGRect (0, 0, 16, 16));
                view.AddSubview (view.ImageView);
                view.TextField = new NSTextField (new CGRect (20, 0, 400, 16));
                view.TextField.BackgroundColor = NSColor.Clear;
                view.TextField.Bordered = false;
                view.TextField.Selectable = false;
                view.AddSubview (view.TextField);
                view.TextField.AutoresizingMask = NSViewResizingMask.WidthSizable;

            }
            view.TextField.TextColor = NSColor.Black;
            view.ImageView.Image = null;

            if (tableColumn.Identifier == TitleIdentifier) {
                retVal = view;

                // Set formatting, ?icons and whether the value is editable.
                //TODO LOOOOADS of stuff.
                switch (node.Type) {
                case DesignNodeType.Root:
                    view.TextField.Editable = false;
                    view.ImageView.Image = NSImage.ImageNamed (NSImageName.Folder);
                    break;
                case DesignNodeType.Sequential:
                    view.TextField.Editable = (tableColumn.Identifier == TitleIdentifier);
                    view.ImageView.Image = NSImage.ImageNamed (NSImageName.StatusNone);
                    break;
                case DesignNodeType.Choice:
                    view.TextField.Editable = (tableColumn.Identifier == TitleIdentifier);
                    view.ImageView.Image = NSImage.ImageNamed (NSImageName.StatusPartiallyAvailable);
                    break;
                case DesignNodeType.Text:
                    view.TextField.TextColor = (tableColumn.Identifier == TitleIdentifier) ? NSColor.Brown : NSColor.Blue;
                    view.TextField.Editable = (tableColumn.Identifier == DescriptionIdentifier);
                    break;
                case DesignNodeType.Comment:
                    view.TextField.TextColor = (tableColumn.Identifier == TitleIdentifier) ? NSColor.Brown : NSColor.Purple;
                    view.ImageView.Image = NSImage.ImageNamed (NSImageName.StatusUnavailable);
                    view.TextField.Editable = true;
                    break;
                case DesignNodeType.Variable:
                case DesignNodeType.VariableForm:
                    view.TextField.Editable = true;
                    view.ImageView.Image = NSImage.ImageNamed (NSImageName.UserGuest);
                    break;
                default:
                    break;
                }

                // Tag view
                view.TextField.Tag = outlineView.RowForItem (item);

                // Save after edit
                //TODO Certain fields not allowed to be left blank?
                view.TextField.EditingEnded += (sender, e) => {

                    // Grab node
                    var nd = outlineView.ItemAtRow (view.TextField.Tag) as DesignNode;

                    // Take action based on type
                    switch (tableColumn.Title) {
                    case TitleIdentifier:
                        nd.Title = view.TextField.StringValue;
                        break;
                    case DescriptionIdentifier:
                        nd.SetDescription (view.TextField.StringValue);
                        break;
                    }
                };

                //TODO Contextual drop down menu or whateve
                // Setup view based on the column selected
                switch (tableColumn.Title) {
                case TitleIdentifier:
                    view.TextField.StringValue = node.Title;
                    break;
                case DescriptionIdentifier:
                    view.TextField.StringValue = node.Description;
                    break;
                }
            }
            else
            {
                retVal = view.TextField;

                // Set formatting, ?icons and whether the value is editable.
                //TODO LOOOOADS of stuff.
                switch (node.Type) {
                case DesignNodeType.Root:
                    view.TextField.Editable = false;
                    break;
                case DesignNodeType.Sequential:
                    view.TextField.Editable = (tableColumn.Identifier == TitleIdentifier);
                    break;
                case DesignNodeType.Choice:
                    view.TextField.Editable = (tableColumn.Identifier == TitleIdentifier);
                    break;
                case DesignNodeType.Text:
                    view.TextField.TextColor = (tableColumn.Identifier == TitleIdentifier) ? NSColor.Brown : NSColor.Blue;
                    view.TextField.Editable = (tableColumn.Identifier == DescriptionIdentifier);
                    break;
                case DesignNodeType.Comment:
                    view.TextField.TextColor = (tableColumn.Identifier == TitleIdentifier) ? NSColor.Brown : NSColor.Purple;
                    view.TextField.Editable = true;
                    break;
                case DesignNodeType.Variable:
                case DesignNodeType.VariableForm:
                    view.TextField.Editable = true;
                    break;
                default:
                    break;
                }

                // Tag view
                view.TextField.Tag = outlineView.RowForItem (item);

                // Save after edit
                //TODO Certain fields not allowed to be left blank?
                view.TextField.EditingEnded += (sender, e) => {

                    // Grab node
                    var nd = outlineView.ItemAtRow (view.Tag) as DesignNode;

                    // Take action based on type
                    switch (tableColumn.Title) {
                    case TitleIdentifier:
                        nd.Title = view.TextField.StringValue;
                        break;
                    case DescriptionIdentifier:
                        nd.SetDescription (view.TextField.StringValue);
                        break;
                    }
                };

                //TODO Contextual drop down menu or whateve
                // Setup view based on the column selected
                switch (tableColumn.Title) {
                case TitleIdentifier:
                    view.TextField.StringValue = node.Title;
                    break;
                case DescriptionIdentifier:
                    view.TextField.StringValue = node.Description;
                    break;
                }
            }

            return retVal;
        }

    }
}
