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
        private const string NounColumnIdentifier = "Noun";
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
            //view.TextField.AutoresizingMask = NSViewResizingMask.WidthSizable;
            view.AddSubview (view.TextField);

            // Configure
            view.TextField.BackgroundColor = NSColor.Clear;
            view.TextField.Bordered = false;
            view.TextField.Selectable = false;
            view.TextField.Editable = false;

            // Tag view
            view.TextField.Tag = row;
        }

        private void ConfigureComboBox (NSTableCellView view, NSComboBox combobox, nint row)
        {
            // Add to view
            view.AddSubview (combobox);

            // Configure
            combobox.UsesDataSource = true;
            combobox.Selectable = true;
            combobox.Editable = true;
            combobox.Bordered = true;
            combobox.IgnoresMultiClick = false;
            combobox.Cell.Font = NSFont.SystemFontOfSize (10);

//            // Listen for changes to suggestions.
//            //TODO So I think the moral problem with this is that I don't know if the combobox is actually active at this point.
//            datasource.Session.SuggestionsUpdated += (name) => {
//                var thisName = datasource.Session.GetName ((int)combobox.Tag);
//                if (name == thisName) {
//                    // If the suggestion is set, it may need to be cleared after invalidation.
//                    var newSuggestions = datasource.Session.GetCurrentSuggestionsForNoun (name);
//                    if (combobox.SelectedIndex >= 0) {
//                        var value = combobox.StringValue;
//                        combobox.DeselectItem (combobox.SelectedIndex);
//                        var newIndex = 0;
//                        for (; newIndex < newSuggestions.Length; newIndex++) {
//                            if (newSuggestions [newIndex] == value)
//                                break;
//                        }
//                        combobox.DataSource = new SetNounValuesSuggestionsComboBoxDataSource (newSuggestions);
//                        if (newIndex < newSuggestions.Length) {
//                            combobox.SelectItem (newIndex);
//                        } else if (datasource.Session.GetAcceptsUserValue (thisName)) {
//                            combobox.StringValue = value;
//                        }
//                    } else
//                        combobox.DataSource = new SetNounValuesSuggestionsComboBoxDataSource (newSuggestions);
//                }
//            };
            // Listen to the value getting set.
            //TODO I think this is over-eager at present - put a delay in or listen to end editing only. Somehow.
            // Delay might be better?
            combobox.Changed += (s, e) => {
                var thisName = datasource.Session.GetName ((int)combobox.Tag);
                var value = combobox.StringValue;
                controller.SetValue (thisName, value);
            };
            combobox.SelectionChanged += (s, e) => {
                var thisName = datasource.Session.GetName ((int)combobox.Tag);
                var index = combobox.SelectedIndex;
                if (index >= 0) {
                    var suggestions = datasource.Session.GetCurrentSuggestionsForNoun (thisName);
                    if (index < suggestions.Length) {
                        var value = suggestions [index];
                        controller.SetValue (thisName, value);
                    } else {
                        controller.SetValue (thisName, "");
                    }
                } else {
                    controller.SetValue (thisName, "");
                }
            };

            combobox.Tag = row;
        }

        public override NSView GetViewForItem (NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            // This pattern allows you reuse existing views when they are no-longer in use.
            // If the returned view is null, you instance up a new view
            // If a non-null view is returned, you modify it enough to reflect the new data
            NSTableCellView view = (NSTableCellView)tableView.MakeView ("SetNounValues" + tableColumn.Title, this);
            if (view == null) {
                view = new NSTableCellView ();

                // Configure the view
                view.Identifier = tableColumn.Title;

                // Take action based on title
                switch (tableColumn.Title) {
                case NounColumnIdentifier:
                case DescriptionColumnIdentifier:
                    view.TextField = new NSTextField (new CGRect (0, 0, 400, 20));
                    ConfigureTextField (view, row);
                    break;
                case ValueColumnIdentifier:
                    var combobox = new NSComboBox (new CGRect (0, 0, 200, 20));
                    ConfigureComboBox (view, combobox, row);
                    break;
                }
            }

            // Setup view based on the column selected - titles match identifiers.
            switch (tableColumn.Title) {
            case NounColumnIdentifier:
                view.TextField.StringValue = datasource.Session.GetName ((int)row);
                view.TextField.Tag = row;
                break;
            case DescriptionColumnIdentifier:
                view.TextField.StringValue = datasource.Session.GetDescription (datasource.Session.GetName ((int)row));
                view.TextField.Tag = row;
                break;
            case ValueColumnIdentifier:
                if (view.Subviews.Length != 1)
                    throw new ApplicationException ("Expected 1 subview, but found " + view.Subviews.Length);
                var cbx = view.Subviews [0] as NSComboBox;
                if (cbx != null) {
                    cbx.Tag = row;
                    var nounName = datasource.Session.GetName ((int)row);
                    cbx.DataSource = new SetNounValuesSuggestionsComboBoxDataSource (nounName, datasource.Session);
                    cbx.Editable = datasource.Session.GetAcceptsUserValue (nounName);
                    controller.SetValue (nounName, cbx.StringValue);
                }
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

        internal void SuggestionsUpdatedForName (string nounName, NSTableView setNounValuesTableView)
        {
            if (datasource != null && datasource.Session != null && setNounValuesTableView != null) {
                var nounIndex = datasource.Session.GetIndex (nounName);
                var rowView = setNounValuesTableView.GetRowView (nounIndex, false);
                if (rowView != null && rowView.Subviews.Length >= 3) {
                    var comboboxCellView = rowView.Subviews [2];
                    var combobox = comboboxCellView.Subviews [0] as NSComboBox;
                    if (combobox != null) {
                        if (combobox.SelectedIndex >= 0) {
                            combobox.DeselectItem (combobox.SelectedIndex);
                        }
                        combobox.ReloadData ();
                    }
                }
            }
        }
    }
}
