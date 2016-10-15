using System;
using System.Linq;
using AppKit;
using CoreGraphics;
using TextOn.Nouns;

namespace TextOn.Studio
{
    public class DefineNounsTableViewDelegate : NSTableViewDelegate
    {
        private const string NounColumnIdentifier = "Noun";
        private const string DescriptionColumnIdentifier = "Description";
        private const string SuggestionsColumnIdentifier = "Suggestions";
        private const string ConstraintsColumnIdentifier = "Constraints";

        private readonly DefineNounsTableViewDataSource datasource;
        private readonly DefineNounsViewController controller;

        public DefineNounsTableViewDelegate (DefineNounsViewController controller, DefineNounsTableViewDataSource datasource)
        {
            this.datasource = datasource;
            this.controller = controller;
        }

        private void ConfigureTextField (NSTableCellView view, nint row)
        {
            // Add to view
            view.TextField.AutoresizingMask = NSViewResizingMask.WidthSizable;
            view.AddSubview (view.TextField);

            // Configure
            view.TextField.BackgroundColor = NSColor.Clear;
            view.TextField.Bordered = false;
            view.TextField.Selectable = false;
            view.TextField.Editable = false;

            // Tag view
            view.TextField.Tag = row;
        }

        private void ConfigureComboBox (NSTableCellView view,NSComboBox combobox, nint row)
        {
            // Add to view
            combobox.AutoresizingMask = NSViewResizingMask.WidthSizable;
            view.AddSubview (combobox);

            // Configure
            combobox.Editable = true;
            combobox.UsesDataSource = true;
            combobox.Selectable = false;

            // Tag view
            combobox.Tag = row;
        }

        public override NSView GetViewForItem (NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            // Get the noun for this row.
            var noun = datasource.NounProfile.GetNounByIndex ((int)row);

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
                case DescriptionColumnIdentifier:
                    view.TextField = new NSTextField (new CGRect (0, 0, 400, 16));
                    ConfigureTextField (view, row);
                    break;
                case SuggestionsColumnIdentifier:
                    var combobox = new NSComboBox ();
                    ConfigureComboBox (view, combobox, row);
                    break;
                }
            }

            // Setup view based on the column selected.
            switch (tableColumn.Title) {
            case NounColumnIdentifier:
                view.TextField.StringValue = noun.Name;
                view.TextField.Tag = row;
                break;
            case DescriptionColumnIdentifier:
                view.TextField.StringValue = noun.Description;
                view.TextField.Tag = row;
                break;
            case SuggestionsColumnIdentifier:
                foreach (NSView subview in view.Subviews) {
                    var combobox = subview as NSComboBox;
                    if (combobox != null) {
                        combobox.Tag = row;
                        var suggestions = noun.Suggestions.Select ((s) => s.Value).ToArray ();
                        combobox.DataSource = new DefineNounsComboBoxDataSource (suggestions);
                    }
                }
                break;
            case ConstraintsColumnIdentifier:
                break;
            }

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

