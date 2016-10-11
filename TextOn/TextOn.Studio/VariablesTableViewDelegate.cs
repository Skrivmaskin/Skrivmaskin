using System;
using AppKit;
using CoreGraphics;
using Foundation;
using System.Collections;
using System.Collections.Generic;

namespace TextOn.Studio
{
    public class VariablesTableViewDelegate : NSTableViewDelegate
    {
        private const string CellIdentifier = "TextOnCell";
        private const string FullNameIdentifier = "Name";
        private const string DescriptionIdentifier = "Description";
        private const string VariantIdentifier = "Variant";
        private const string ValueIdentifier = "Value";

        private VariablesTableViewDataSource DataSource;
        public VariablesTableViewDelegate (VariablesTableViewDataSource datasource)
        {
            this.DataSource = datasource;
        }

        public override NSView GetViewForItem (NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            // This pattern allows you reuse existing views when they are no-longer in use.
            // If the returned view is null, you instance up a new view
            // If a non-null view is returned, you modify it enough to reflect the new data
            NSTextField view = (NSTextField)tableView.MakeView (CellIdentifier, this);
            if (view == null) {
                view = new NSTextField ();
                view.Identifier = CellIdentifier;
                view.BackgroundColor = NSColor.Clear;
                view.Bordered = false;
                view.Selectable = false;
            }

            var variable = DataSource.Variables [(int)row];

            view.TextColor = NSColor.Black;

            switch (tableColumn.Identifier) {
            case FullNameIdentifier:
                view.StringValue = variable.Item1;
                view.Editable = false;
                break;
            case DescriptionIdentifier:
                view.StringValue = variable.Item2;
                view.Editable = false;
                break;
            case VariantIdentifier:
                view.StringValue = "";
                view.Editable = false;
                break;
            case ValueIdentifier:
                view.StringValue = DataSource.VariableValues [variable.Item1];
                // Save after edit
                //TODO not allowed to be left blank?
                //TODO Don't I have to worry about view reuse?
                view.EditingEnded += (sender, e) => {
                    DataSource.VariableValues [variable.Item1] = view.StringValue;
                };
                view.Editable = true;
                break;
            default:
                throw new ApplicationException ("Unexpected table identifier " + tableColumn.Identifier);
            }

            return view;
        }

    }
}
