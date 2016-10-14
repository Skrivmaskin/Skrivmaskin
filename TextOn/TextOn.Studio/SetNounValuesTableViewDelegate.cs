using System;
using TextOn.Nouns;
using AppKit;
using CoreGraphics;
using Foundation;

namespace TextOn.Studio
{
    public class SetNounValuesTableViewDelegate : NSTableViewDelegate
    {
        private readonly SetNounValuesTableViewDataSource datasource;
        private readonly SetNounValuesViewController controller;
        private const string CellIdentifier = "SetNounValuesCell";
        private const string NameColumnIdentifier = "Name";
        private const string DescriptionColumnIdentifier = "Description";
        private const string ValueColumnIdentifier = "Value";

        public SetNounValuesTableViewDelegate (SetNounValuesViewController controller, SetNounValuesTableViewDataSource datasource)
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

        public override NSView GetViewForItem (NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
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
                case NameColumnIdentifier:
                case DescriptionColumnIdentifier:
                    view.TextField = new NSTextField (new CGRect (0, 0, 400, 16));
                    ConfigureTextField (view, row);
                    break;
                }
            }

            // Setup view based on the column selected - titles match identifiers.
            switch (tableColumn.Title) {
            case NameColumnIdentifier:
                view.TextField.StringValue = datasource.Session.GetName ((int)row);
                view.TextField.Tag = row;
                break;
            case DescriptionColumnIdentifier:
                view.TextField.StringValue = datasource.Session.GetDescription (datasource.Session.GetName ((int)row));
                view.TextField.Tag = row;
                break;
            case ValueColumnIdentifier:
                foreach (NSView subview in view.Subviews) {
                    var cbx = subview as NSComboBox;
                    if (cbx != null) {
                        cbx.Tag = row;
                        var nounName = datasource.Session.GetName ((int)row);
                        var suggestions = datasource.Session.GetCurrentSuggestionsForNoun (nounName);
                        cbx.Editable = datasource.Session.GetAcceptsUserValue (nounName);
                        cbx.DataSource = new SetNounValuesSuggestionsComboBoxDataSource (suggestions);
                        cbx.UsesDataSource = true;
                        cbx.Selectable = true;
                        cbx.AutoresizingMask = NSViewResizingMask.WidthSizable;
                        cbx.IgnoresMultiClick = false;

                        datasource.Session.SetValue (nounName, cbx.StringValue);

                        // Listen for changes to suggestions.
                        datasource.Session.SuggestionsUpdated += (name) => {
                            var thisName = datasource.Session.GetName ((int)cbx.Tag);
                            if (name == thisName) {
                                // If the suggestion is set, it may need to be cleared after invalidation.
                                var newSuggestions = datasource.Session.GetCurrentSuggestionsForNoun (name);
                                if (cbx.SelectedIndex >= 0) {
                                    var value = cbx.StringValue;
                                    cbx.DeselectItem (cbx.SelectedIndex);
                                    var newIndex = 0;
                                    for (; newIndex < newSuggestions.Length; newIndex++) {
                                        if (newSuggestions [newIndex] == value)
                                            break;
                                    }
                                    cbx.DataSource = new SetNounValuesSuggestionsComboBoxDataSource (newSuggestions);
                                    if (newIndex < newSuggestions.Length) {
                                        cbx.SelectItem (newIndex);
                                    } else if (datasource.Session.GetAcceptsUserValue (thisName)) {
                                        cbx.StringValue = value;
                                    }
                                }
                                else
                                    cbx.DataSource = new SetNounValuesSuggestionsComboBoxDataSource (newSuggestions);
                            }
                        };

                        // Listen to the value getting set.
                        cbx.Changed += (s, e) => {
                            var thisName = datasource.Session.GetName ((int)cbx.Tag);
                            var value = cbx.StringValue;
                            datasource.Session.SetValue (thisName, value);
                        };
                        cbx.SelectionChanged += (s, e) => {
                            var thisName = datasource.Session.GetName ((int)cbx.Tag);
                            var index = cbx.SelectedIndex;
                            if (index >= 0) {
                                var value = datasource.Session.GetCurrentSuggestionsForNoun (nounName) [index];
                                datasource.Session.SetValue (thisName, value);
                            } else {
                                datasource.Session.SetValue (thisName, "");
                            }
                        };
                    }
                }
                break;
            }

            return view;
        }


        public override bool ShouldSelectRow (NSTableView tableView, nint row)
        {
            return false;
        }

        public override bool ShouldReorder (NSTableView tableView, nint columnIndex, nint newColumnIndex)
        {
            return false;
        }

    }
}
