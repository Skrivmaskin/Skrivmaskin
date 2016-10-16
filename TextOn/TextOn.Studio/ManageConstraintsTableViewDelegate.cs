using System;
using System.Linq;
using System.Collections.Generic;
using AppKit;
using CoreGraphics;

namespace TextOn.Studio
{
    public class ManageConstraintsTableViewDelegate : NSTableViewDelegate
    {
        private const string NounColumnIdentifier = "Noun";
        private const string ValueColumnIdentifier = "Value";

        readonly ManageConstraintsDialogController controller;
        readonly IDictionary<string, string> values;
        readonly string [] names;

        public ManageConstraintsTableViewDelegate (ManageConstraintsDialogController controller, IDictionary<string, string> values)
        {
            this.controller = controller;
            this.values = values;
            names = values.Select ((kvp) => kvp.Key).ToArray();
        }

        private void ConfigureTextField (NSTableCellView view, NSTextField field, nint row)
        {
            // Add to view
            field.AutoresizingMask = NSViewResizingMask.WidthSizable;
            view.AddSubview (field);

            // Configure
            field.BackgroundColor = NSColor.Clear;
            field.Bordered = false;
            field.Selectable = false;
            field.Editable = false;

            // Tag view
            field.Tag = row;
        }

        public override NSView GetViewForItem (NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            Console.Error.WriteLine ("ManageConstraintsTable GetViewForItem {0}", row);
            // This pattern allows you reuse existing views when they are no-longer in use.
            // If the returned view is null, you instance up a new view
            // If a non-null view is returned, you modify it enough to reflect the new data
            NSTableCellView view = (NSTableCellView)tableView.MakeView (tableColumn.Title, this);
            if (view == null) {
                view = new NSTableCellView ();

                // Configure the view
                view.Identifier = tableColumn.Title;

                // Take action based on title
                switch (tableColumn.Title) {
                case NounColumnIdentifier:
                    view.TextField = new NSTextField (new CGRect (0, 0, 135, 20));
                    ConfigureTextField (view, view.TextField, row);
                    break;
                case ValueColumnIdentifier:
                    var buttonStyle = NSBezelStyle.TexturedRounded;
                    var textField = new NSTextField (new CGRect (0, 0, 231, 20));
                    ConfigureTextField (view, textField, row);
                    var removeButton = new NSButton (new CGRect (233, 0, 20, 20));
                    removeButton.Image = NSImage.ImageNamed (NSImageName.RemoveTemplate);
                    view.AddSubview (removeButton);
                    removeButton.BezelStyle = buttonStyle;
                    removeButton.Tag = row;
                    removeButton.Activated += (s, e) => {
                        var rowView = tableView.GetRowView (removeButton.Tag, false);
                        var nounView = rowView.Subviews [0];
                        var nounTextField = nounView.Subviews [0] as NSTextField;
                        controller.RemoveConstraint (nounTextField.StringValue);
                    };
                    break;
                }
            }
            // Setup view based on the column selected.
            switch (tableColumn.Title) {
            case NounColumnIdentifier:
                view.TextField.StringValue = names [row];
                view.TextField.Tag = row;
                break;
            case ValueColumnIdentifier:
                foreach (var subview in view.Subviews) {
                    if (subview is NSTextField) {
                        var textField = subview as NSTextField;
                        textField.StringValue = values [names [row]];
                        textField.Tag = row;
                    } else {
                        var button = subview as NSButton;
                        button.Tag = row;
                    }
                }
                break;
            }
            Console.Error.WriteLine ("ManageConstraintsTable GetViewForItem {0} Exit", row);
            return view;
        }

        public override bool ShouldSelectRow (NSTableView tableView, nint row)
        {
            return true;
        }

        public override bool ShouldReorder (NSTableView tableView, nint columnIndex, nint newColumnIndex)
        {
            return false;
        }
    }
}
