using System;
using AppKit;
using Foundation;
using TextOn.Nouns;

namespace TextOn.Studio
{
	class SetNounValuesSuggestionsComboBoxDataSource : NSComboBoxDataSource
	{
        readonly string name;
        readonly NounSetValuesSession session;

		public SetNounValuesSuggestionsComboBoxDataSource (string name, NounSetValuesSession session)
		{
            this.name = name;
            this.session = session;
		}

        public override nint ItemCount (NSComboBox comboBox)
        {
            return session.GetCurrentSuggestionsForNoun (name).Length;
        }

        private string [] GetSuggestions ()
        {
            return (session.IsActive) ? session.GetCurrentSuggestionsForNoun (name) : new string [0];

        }

        public override NSObject ObjectValueForItem (NSComboBox comboBox, nint index)
        {
            var suggestions = GetSuggestions ();
            if (index >= suggestions.Length) return new NSString ("");
            return new NSString (suggestions [index]);
        }

        public override nint IndexOfItem (NSComboBox comboBox, string value)
        {
            var suggestions = GetSuggestions ();
            int i = 0;
            foreach (var suggestion in suggestions) {
                if (suggestion == value) return i;
                ++i;
            }
            return i;
        }

        public override string CompletedString (NSComboBox comboBox, string uncompletedString)
        {
            foreach (var suggestion in GetSuggestions ()) {
                if (suggestion.StartsWith (uncompletedString, StringComparison.InvariantCultureIgnoreCase)) return suggestion;
            }
            return uncompletedString;
        }
	}
}