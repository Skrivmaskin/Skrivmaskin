﻿using System;
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
            combobox.Editable = true;
            combobox.Bordered = true;
            combobox.IgnoresMultiClick = false;
            combobox.Cell.Font = NSFont.SystemFontOfSize (10);

            // Tag view
            combobox.Tag = row;
        }

        private EventHandler SuggestionsComboBox_SelectionChanged (NSComboBox combobox, NSButton addButton, NSButton removeButton, NSButton editConstraintsButton)
        {
            return (s, e) => {
                var row = (int)combobox.Tag;
                var noun = (row < datasource.NounProfile.Count) ? datasource.NounProfile.GetNounByIndex (row) : null;
                if (noun == null) {
                    addButton.Enabled = false;
                    removeButton.Enabled = false;
                    editConstraintsButton.Enabled = false;
                    return;
                }
                var selectedIndex = (int)combobox.SelectedIndex;
                if (selectedIndex < 0) {
                    addButton.Enabled = (!String.IsNullOrWhiteSpace (combobox.StringValue));
                    removeButton.Enabled = false;
                    editConstraintsButton.Enabled = false;
                    return;
                }
                var selectedIndexValue = (selectedIndex < noun.Suggestions.Count) ? noun.Suggestions [selectedIndex].Value : "";
                if (String.IsNullOrWhiteSpace (selectedIndexValue)) {
                    addButton.Enabled = false;
                    removeButton.Enabled = false;
                    editConstraintsButton.Enabled = false;
                    return;
                }
                addButton.Enabled = false;
                removeButton.Enabled = true;
                editConstraintsButton.Enabled = true;
            };
        }

        private EventHandler SuggestionsComboBox_Changed (NSComboBox combobox, NSButton addButton, NSButton removeButton, NSButton editConstraintsButton)
        {
            return (s, e) => {
                var row = (int)combobox.Tag;
                var noun = (row < datasource.NounProfile.Count) ? datasource.NounProfile.GetNounByIndex (row) : null;
                if (noun == null) {
                    addButton.Enabled = false;
                    removeButton.Enabled = false;
                    editConstraintsButton.Enabled = false;
                    return;
                }
                var selectedIndex = 0;
                var valueToFind = combobox.StringValue;
                //TODO this is well lame, I can't be this slow - replace this data structure to improve this
                for (; selectedIndex < noun.Suggestions.Count; ++selectedIndex) {
                    if (valueToFind == noun.Suggestions [selectedIndex].Value) break;
                }
                if (selectedIndex < noun.Suggestions.Count) {
                    addButton.Enabled = false;
                    removeButton.Enabled = true;
                    editConstraintsButton.Enabled = true;
                } else {
                    addButton.Enabled = (!String.IsNullOrWhiteSpace (valueToFind));
                    removeButton.Enabled = false;
                    editConstraintsButton.Enabled = false;
                }
            };
        }

        private EventHandler SuggestionsAddButton_Activated (NSComboBox combobox, NSButton addButton, NSButton removeButton, NSButton editConstraintsButton)
        {
            return (s, e) => {
                var row = (int)addButton.Tag;
                var noun = (row < datasource.NounProfile.Count) ? datasource.NounProfile.GetNounByIndex (row) : null;
                if (noun == null) {
                    addButton.Enabled = false;
                    removeButton.Enabled = false;
                    editConstraintsButton.Enabled = false;
                    return;
                }
                var thisSuggestionsPrior = noun.Suggestions.Select ((sugg) => sugg.Value).ToArray ();
                if (((combobox.SelectedIndex < 0) || thisSuggestionsPrior [combobox.SelectedIndex] != combobox.StringValue) && (!String.IsNullOrWhiteSpace (combobox.StringValue))) {
                    var thisValue = combobox.StringValue;
                    datasource.NounProfile.AddSuggestion (noun.Name, thisValue, controller.GetCurrentDefaultDependenciesForThisNoun (noun.Name));
                    var thisSuggestions = noun.Suggestions.Select ((sugg) => sugg.Value).ToArray ();
                    if (combobox.SelectedIndex >= 0) combobox.DeselectItem (combobox.SelectedIndex);
                    combobox.DataSource = new DefineNounsComboBoxDataSource (thisSuggestions);
                    addButton.Enabled = false;
                    removeButton.Enabled = true;
                    editConstraintsButton.Enabled = true;
                }
            };
        }

        private EventHandler SuggestionsRemoveButton_Activated (NSComboBox combobox, NSButton addButton, NSButton removeButton, NSButton editConstraintsButton)
        {
            return (s, e) => {
                var row = (int)removeButton.Tag;
                var noun = (row < datasource.NounProfile.Count) ? datasource.NounProfile.GetNounByIndex (row) : null;
                if (noun == null) {
                    addButton.Enabled = false;
                    removeButton.Enabled = false;
                    editConstraintsButton.Enabled = false;
                    return;
                }
                var valueToFind = combobox.StringValue;
                var selectedIndex = 0;
                for (; selectedIndex < noun.Suggestions.Count; ++selectedIndex) {
                    var suggestion = noun.Suggestions [selectedIndex];
                    if (suggestion.Value == valueToFind) break;
                }
                if (selectedIndex >= noun.Suggestions.Count) {
                    // this is an error - disable the buttons and ignore
                    addButton.Enabled = false;
                    removeButton.Enabled = false;
                    editConstraintsButton.Enabled = false;
                } else {
                    var thisSuggestionValue = noun.Suggestions.ElementAt (selectedIndex).Value;
                    if (thisSuggestionValue == combobox.StringValue) {
                        if (combobox.SelectedIndex >= 0) combobox.DeselectItem (combobox.SelectedIndex);
                        controller.DeleteSuggestion (noun.Name, thisSuggestionValue);
                    }
                    addButton.Enabled = false;
                    removeButton.Enabled = false;
                    editConstraintsButton.Enabled = false;
                }
            };
        }

        private EventHandler SuggestionsEditConstraintsButton_Activated (NSComboBox combobox, NSButton addButton, NSButton removeButton, NSButton editConstraintsButton)
        {
            return (s, e) => {
                var thisRow = (int)editConstraintsButton.Tag;
                var noun = (thisRow < datasource.NounProfile.Count) ? datasource.NounProfile.GetNounByIndex (thisRow) : null;
                if (noun == null) {
                    addButton.Enabled = false;
                    removeButton.Enabled = false;
                    editConstraintsButton.Enabled = false;
                    return;
                }
                var selectedIndex = (int)combobox.SelectedIndex;
                if (selectedIndex < 0) {
                    var valueToFind = combobox.StringValue;
                    selectedIndex = 0;
                    for (; selectedIndex < noun.Suggestions.Count; ++selectedIndex) {
                        var suggestion = noun.Suggestions [selectedIndex];
                        if (suggestion.Value == valueToFind) break;
                    }
                }
                if (selectedIndex >= noun.Suggestions.Count) {
                    // this is an error - disable the buttons and ignore
                    addButton.Enabled = false;
                    removeButton.Enabled = false;
                    editConstraintsButton.Enabled = false;
                } else {
                    var thisSuggestionValue = noun.Suggestions.ElementAt (selectedIndex).Value;
                    if (thisSuggestionValue == combobox.StringValue) {
                        controller.ManageConstraints (noun.Name, thisSuggestionValue);
                    }
                }
            };
        }

        public override NSView GetViewForItem (NSTableView tableView, NSTableColumn tableColumn, nint row)
        {
            // Get the noun for this row.
            var noun = datasource.NounProfile.GetNounByIndex ((int)row);

            // This pattern allows you reuse existing views when they are no-longer in use.
            // If the returned view is null, you instance up a new view
            // If a non-null view is returned, you modify it enough to reflect the new data
            NSTableCellView view = (NSTableCellView)tableView.MakeView ("Define" + tableColumn.Title, this);
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
                    combobox.SelectionChanged += SuggestionsComboBox_SelectionChanged (combobox, addButton, removeButton, editConstraintsButton);
                    combobox.Changed += SuggestionsComboBox_Changed (combobox, addButton, removeButton, editConstraintsButton);
                    addButton.Activated += SuggestionsAddButton_Activated (combobox, addButton, removeButton, editConstraintsButton);
                    removeButton.Activated += SuggestionsRemoveButton_Activated (combobox, addButton, removeButton, editConstraintsButton);
                    editConstraintsButton.Activated += SuggestionsEditConstraintsButton_Activated (combobox, addButton, removeButton, editConstraintsButton);
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
                if (view.Subviews.Length != 4)
                    throw new ApplicationException ("Supposed to have 4 subviews at this point (DefineNounsTable - Suggestions column) - have " + view.Subviews.Length);
                var combobox = view.Subviews[0] as NSComboBox;
                if (combobox != null) {
                    combobox.Tag = row;
                    var suggestions = noun.Suggestions.Select ((s) => s.Value).ToArray ();
                    combobox.DataSource = new DefineNounsComboBoxDataSource (suggestions);
                    if (combobox.SelectedIndex >= 0) {
                        combobox.DeselectItem (combobox.SelectedIndex);
                    }
                    combobox.StringValue = "";
                }
                var addButton = view.Subviews [1] as NSButton;
                addButton.Tag = row;
                addButton.Enabled = false;
                var removeButton = view.Subviews [2] as NSButton;
                removeButton.Tag = row;
                removeButton.Enabled = false;
                var editConstraintsButton = view.Subviews [3] as NSButton;
                editConstraintsButton.Tag = row;
                editConstraintsButton.Enabled = false;
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

