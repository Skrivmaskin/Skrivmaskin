using System;
using AppKit;
using Foundation;

namespace TextOn.Studio
{
    class DefineNounsComboBoxDataSource : NSComboBoxDataSource
    {
        string [] suggestions;

        public DefineNounsComboBoxDataSource (string [] suggestions)
        {
            this.suggestions = suggestions;
        }

        public override nint ItemCount (NSComboBox comboBox)
        {
            Console.Error.WriteLine ("ComboBox ItemCount {0}", suggestions.Length);
            return suggestions.Length;
        }

        public override NSObject ObjectValueForItem (NSComboBox comboBox, nint index)
        {
            Console.Error.WriteLine ("ObjectValueForItem {0} {1}", index, suggestions.Length);
            if (index >= suggestions.Length) return new NSString ("");
            return new NSString (suggestions [index]);
        }

        public override nint IndexOfItem (NSComboBox comboBox, string value)
        {
            Console.Error.WriteLine ("ComboBox IndexOfItem {0}", value);
            int i = 0;
            foreach (var suggestion in suggestions) {
                Console.Error.WriteLine ("ComboBox Item {0} is {1} looking for {2}", i, suggestion, value);
                if (suggestion == value) return i;
                ++i;
            }
            if (i > suggestions.Length) return -1;
            Console.Error.WriteLine ("ComboBox IndexOfItem {0} Exit {1}", value, i);
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