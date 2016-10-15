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

        private void ConfigureComboBox (NSTableCellView view,NSComboBox combobox, nint row)
        {
            view.AddSubview (combobox);

            combobox.UsesDataSource = true;
            combobox.Selectable = true;
            combobox.IgnoresMultiClick = false;
            combobox.Cell.Font = NSFont.SystemFontOfSize (10);

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
                    ConfigureComboBox (view, combobox, row);
                    var addButton = new NSButton (new CGRect (252, 0, 20, 20));
                    addButton.Image = NSImage.ImageNamed (NSImageName.AddTemplate);
                    view.AddSubview (addButton);
                    addButton.BezelStyle = buttonStyle;
                    addButton.Tag = row;
                    addButton.Activated += (s, e) => {
                        var thisNoun = datasource.NounProfile.GetNounByIndex ((int)addButton.Tag);
                        var thisSuggestionsPrior = thisNoun.Suggestions.Select ((sugg) => sugg.Value).ToArray ();
                        if (((combobox.SelectedIndex < 0) || thisSuggestionsPrior[combobox.SelectedIndex] != combobox.StringValue) && (!String.IsNullOrWhiteSpace (combobox.StringValue))) {
                            var thisValue = combobox.StringValue;
                            datasource.NounProfile.AddSuggestion (thisNoun.Name, thisValue, new NounSuggestionDependency [0]);
                            var thisSuggestions = thisNoun.Suggestions.Select ((sugg) => sugg.Value).ToArray ();
                            combobox.DataSource = new DefineNounsComboBoxDataSource (thisSuggestions);
                            combobox.Select ((NSString)thisValue);
                        }
                    };
                    var removeButton = new NSButton (new CGRect (274, 0, 20, 20));
                    removeButton.Image = NSImage.ImageNamed (NSImageName.RemoveTemplate);
                    view.AddSubview (removeButton);
                    removeButton.BezelStyle = buttonStyle;
                    removeButton.Tag = row;
                    var editConstraintsButton = new NSButton (new CGRect (296, 0, 104, 20));
                    editConstraintsButton.Title = "Constraints";
                    view.AddSubview (editConstraintsButton);
                    editConstraintsButton.BezelStyle = buttonStyle;
                    editConstraintsButton.Tag = row;
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

