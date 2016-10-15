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
                    var addButton = new NSButton ();
                    var removeButton = new NSButton ();
                    combobox.Tag = row;
                    addButton.Tag = row;
                    removeButton.Tag = row;
                    view.AddSubview (combobox);
                    view.AddSubview (addButton);
                    view.AddSubview (removeButton);
                    var suggestions = noun.Suggestions.Select ((s) => s.Value).ToArray();
                    combobox.UsesDataSource = true;
                    combobox.DataSource = new DefineNounsComboBoxDataSource (suggestions);
                    addButton.Activated += (s, e) => {
                        var thisRow = (int)addButton.Tag;
                        var thisView = tableView.GetView (2, thisRow, false);
                        var thisCbx = thisView.Subviews [0] as NSComboBox;
                        var value = thisCbx.StringValue;
                        // check there's some text
                        // add
                        // new datasource/delegate
                        // select it
                        // update an array of selection indices in the datasource
                        if (!String.IsNullOrWhiteSpace (value)) {
                            var thisNoun = datasource.NounProfile.GetNounByIndex (thisRow);
                            datasource.NounProfile.AddSuggestion (thisNoun.Name, value, new NounSuggestionDependency [0]);
                            var newSuggestions = thisNoun.Suggestions.Select ((sug) => sug.Value).ToArray ();
                            thisCbx.DataSource = new DefineNounsComboBoxDataSource (newSuggestions);
                            thisCbx.SelectItem (newSuggestions.Length - 1);
                            controller.SelectSuggestionIndex(thisNoun.Name, newSuggestions.Length - 1);
                        }
                    };
                    removeButton.Activated += (s, e) => {
                        // check there's a selection
                        // alert
                        // remove
                        // new datasource/delegate
                        // update an array of selection indices in the datasource (might be none left)
                        // or select another item (previous/0?)

                    };
                    combobox.SelectionChanged += (s, e) => {
                        // update an array of selection indices in the datasource
                        var thisNoun = datasource.NounProfile.GetNounByIndex ((int)combobox.Tag);
                        controller.SelectSuggestionIndex (thisNoun.Name, (int)combobox.SelectedIndex);
                    };
                    break;
                case ConstraintsColumnIdentifier:
                    var editConstraints = new NSButton ();
                    editConstraints.Tag = row;
                    view.AddSubview (editConstraints);
                    // update a value in the datasource
                    // trigger a segue on the controller
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
                var cbx = view.Subviews [0] as NSComboBox;
                var addButton = view.Subviews [1] as NSButton;
                var removeButton = view.Subviews [2] as NSButton;
                cbx.Tag = row;
                addButton.Tag = row;
                removeButton.Tag = row;
                var nounName = noun.Name;
                var suggestions = noun.Suggestions.Select ((s) => s.Value).ToArray ();
                cbx.Editable = true;
                cbx.DataSource = new DefineNounsComboBoxDataSource (suggestions);
                cbx.UsesDataSource = true;
                cbx.Selectable = true;
                cbx.AutoresizingMask = NSViewResizingMask.WidthSizable;
                cbx.IgnoresMultiClick = false;
                break;
            case ConstraintsColumnIdentifier:
                var editConstraints = view.Subviews [0] as NSButton;
                editConstraints.Tag = row;
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

