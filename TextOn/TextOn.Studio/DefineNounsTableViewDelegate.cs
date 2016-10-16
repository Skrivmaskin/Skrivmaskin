using System;
using System.Linq;
using AppKit;
using CoreGraphics;
using Foundation;
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

        private void ConfigureComboBox (NSTableCellView view, NounProfile nounProfile, NSComboBox combobox, nint row)
        {
            // Add to view.
            view.AddSubview (combobox);

            // Configure
            combobox.UsesDataSource = true;
            combobox.Selectable = true;
            combobox.IgnoresMultiClick = false;
            combobox.Cell.Font = NSFont.SystemFontOfSize (10);

            // Listen to changes in suggestions for the relevant Noun
            nounProfile.SuggestionsChangedForNoun += (nounName) => {
                var nounIndex = (int)combobox.Tag;
                var currentNoun = (nounIndex < nounProfile.Count) ? nounProfile.GetNounByIndex (nounIndex) : null;
                if (currentNoun != null && currentNoun.Name == nounName) {
                    var suggestions = currentNoun.Suggestions.Select ((s) => s.Value).ToArray ();
                    combobox.StringValue = "";
                    combobox.DataSource = new DefineNounsComboBoxDataSource (suggestions);
                }
            };

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
                    view.TextField = new NSTextField (new CGRect (0, 0, 400, 20));
                    ConfigureTextField (view, row);
                    break;
                case SuggestionsColumnIdentifier:
                    var buttonStyle = NSBezelStyle.TexturedRounded;
                    var combobox = new NSComboBox (new CGRect (0, 0, 250, 20));
                    ConfigureComboBox (view, datasource.NounProfile, combobox, row);
                    var addButton = new NSButton (new CGRect (252, 0, 20, 20));
                    addButton.Image = NSImage.ImageNamed (NSImageName.AddTemplate);
                    view.AddSubview (addButton);
                    addButton.BezelStyle = buttonStyle;
                    addButton.Enabled = false;
                    addButton.Tag = row;
                    var removeButton = new NSButton (new CGRect (274, 0, 20, 20));
                    removeButton.Image = NSImage.ImageNamed (NSImageName.RemoveTemplate);
                    view.AddSubview (removeButton);
                    removeButton.BezelStyle = buttonStyle;
                    removeButton.Enabled = false;
                    removeButton.Tag = row;
                    var editConstraintsButton = new NSButton (new CGRect (296, 0, 104, 20));
                    editConstraintsButton.Title = "Constraints";
                    view.AddSubview (editConstraintsButton);
                    editConstraintsButton.BezelStyle = buttonStyle;
                    editConstraintsButton.Enabled = false;
                    editConstraintsButton.Tag = row;
                    // Begin complicated event hookups
                    // I'm guessing I need to be a bit careful here as I don't really own these things so guard for everything
                    combobox.SelectionChanged += (s, e) => {
                        var thisRow = (int)combobox.Tag;
                        var thisNoun = (thisRow < datasource.NounProfile.Count) ? datasource.NounProfile.GetNounByIndex (thisRow) : null;
                        if (thisNoun == null) {
                            addButton.Enabled = false;
                            removeButton.Enabled = false;
                            editConstraintsButton.Enabled = false;
                            return;
                        }
                        var selectedIndex = (int)combobox.SelectedIndex;
                        var stringValue = combobox.StringValue;
                        if (selectedIndex < 0) {
                            addButton.Enabled = (!String.IsNullOrWhiteSpace (stringValue));
                            removeButton.Enabled = false;
                            editConstraintsButton.Enabled = false;
                            return;
                        }
                        var selectedIndexValue = (selectedIndex < thisNoun.Suggestions.Count) ? thisNoun.Suggestions [selectedIndex].Value : "";
                        if (String.IsNullOrWhiteSpace (selectedIndexValue)) {
                            addButton.Enabled = false;
                            removeButton.Enabled = false;
                            editConstraintsButton.Enabled = false;
                            return;
                        }
                        var hasSelectedItem = stringValue == selectedIndexValue;
                        addButton.Enabled = !String.IsNullOrWhiteSpace(stringValue) && !hasSelectedItem;
                        removeButton.Enabled = hasSelectedItem;
                        editConstraintsButton.Enabled = hasSelectedItem;
                    };
                    combobox.Changed += (s, e) => {
                        var thisRow = (int)combobox.Tag;
                        var thisNoun = (thisRow < datasource.NounProfile.Count) ? datasource.NounProfile.GetNounByIndex (thisRow) : null;
                        if (thisNoun == null) {
                            addButton.Enabled = false;
                            removeButton.Enabled = false;
                            editConstraintsButton.Enabled = false;
                            return;
                        }
                        var selectedIndex = (int)combobox.SelectedIndex;
                        var stringValue = combobox.StringValue;
                        if (selectedIndex < 0) {
                            addButton.Enabled = (!String.IsNullOrWhiteSpace (stringValue));
                            removeButton.Enabled = false;
                            editConstraintsButton.Enabled = false;
                            return;
                        }
                        var selectedIndexValue = (selectedIndex < thisNoun.Suggestions.Count) ? thisNoun.Suggestions [selectedIndex].Value : "";
                        if (String.IsNullOrWhiteSpace (selectedIndexValue)) {
                            addButton.Enabled = false;
                            removeButton.Enabled = false;
                            editConstraintsButton.Enabled = false;
                            return;
                        }
                        var hasSelectedItem = stringValue == selectedIndexValue;
                        addButton.Enabled = !String.IsNullOrWhiteSpace (stringValue) && !hasSelectedItem;
                        removeButton.Enabled = hasSelectedItem;
                        editConstraintsButton.Enabled = hasSelectedItem;
                    };
                    addButton.Activated += (s, e) => {
                        var thisRow = (int)addButton.Tag;
                        var thisNoun = (thisRow < datasource.NounProfile.Count) ? datasource.NounProfile.GetNounByIndex (thisRow) : null;
                        if (thisNoun == null) {
                            addButton.Enabled = false;
                            removeButton.Enabled = false;
                            editConstraintsButton.Enabled = false;
                            return;
                        }
                        var thisSuggestionsPrior = thisNoun.Suggestions.Select ((sugg) => sugg.Value).ToArray ();
                        if (((combobox.SelectedIndex < 0) || thisSuggestionsPrior [combobox.SelectedIndex] != combobox.StringValue) && (!String.IsNullOrWhiteSpace (combobox.StringValue))) {
                            var thisValue = combobox.StringValue;
                            datasource.NounProfile.AddSuggestion (thisNoun.Name, thisValue, controller.GetCurrentDefaultDependenciesForThisNoun (thisNoun.Name));
                            var thisSuggestions = thisNoun.Suggestions.Select ((sugg) => sugg.Value).ToArray ();
                            combobox.DataSource = new DefineNounsComboBoxDataSource (thisSuggestions);
                            combobox.Select ((NSString)thisValue);
                            addButton.Enabled = false;
                            removeButton.Enabled = true;
                            editConstraintsButton.Enabled = true;
                        }
                    };
                    removeButton.Activated += (s, e) => {
                        var thisRow = (int)removeButton.Tag;
                        var thisNoun = (thisRow < datasource.NounProfile.Count) ? datasource.NounProfile.GetNounByIndex (thisRow) : null;
                        if (thisNoun == null) {
                            addButton.Enabled = false;
                            removeButton.Enabled = false;
                            editConstraintsButton.Enabled = false;
                            return;
                        }
                        var selectedIndex = combobox.SelectedIndex;
                        if (selectedIndex >= 0) {
                            var thisSuggestionValue = thisNoun.Suggestions.ElementAt ((int)selectedIndex).Value;
                            if (thisSuggestionValue == combobox.StringValue) {
                                controller.DeleteSuggestion (thisNoun.Name, thisSuggestionValue);
                            }
                        }
                        addButton.Enabled = false;
                        removeButton.Enabled = false;
                        editConstraintsButton.Enabled = false;
                    };
                    editConstraintsButton.Activated += (s, e) => {
                        var thisRow = (int)editConstraintsButton.Tag;
                        var thisNoun = (thisRow < datasource.NounProfile.Count) ? datasource.NounProfile.GetNounByIndex (thisRow) : null;
                        if (thisNoun == null) return;
                        var selectedIndex = combobox.SelectedIndex;
                        if (selectedIndex >= 0) {
                            var thisSuggestionValue = thisNoun.Suggestions.ElementAt ((int)selectedIndex).Value;
                            if (thisSuggestionValue == combobox.StringValue) {
                                controller.ManageConstraints (thisNoun.Name, thisSuggestionValue);
                            }
                        }
                    };
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

