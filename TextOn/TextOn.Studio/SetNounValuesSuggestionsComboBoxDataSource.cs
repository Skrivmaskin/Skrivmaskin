using System;
using AppKit;
using Foundation;

namespace TextOn.Studio
{
	class SetNounValuesSuggestionsComboBoxDataSource : NSComboBoxDataSource
	{
        string [] suggestions;

		public SetNounValuesSuggestionsComboBoxDataSource (string [] suggestions)
		{
            this.suggestions = suggestions;
		}

        public override nint ItemCount (NSComboBox comboBox)
        {
            return suggestions.Length;
        }

        public override NSObject ObjectValueForItem (NSComboBox comboBox, nint index)
        {
            return new NSString (suggestions [index]);
        }

        public override nint IndexOfItem (NSComboBox comboBox, string value)
        {
            int i = 0;
            foreach (var suggestion in suggestions) {
                if (suggestion == value) return i;
                ++i;
            }
            return i;
        }

        public override string CompletedString (NSComboBox comboBox, string uncompletedString)
        {
            foreach (var suggestion in suggestions) {
                if (suggestion.StartsWith (uncompletedString, StringComparison.InvariantCultureIgnoreCase)) return suggestion;
            }
            return uncompletedString;
        }
	}
}