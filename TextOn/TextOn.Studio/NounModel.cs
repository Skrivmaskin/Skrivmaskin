using System;
using Foundation;

namespace TextOn.Studio
{
    [Register ("NounModel")]
    public class NounModel : NSObject
    {
        private string _name;
        public string name {
            [Export (nameof (name))]
            get {
                return _name;
            }
            set {
                WillChangeValue (nameof (name));
                _name = value;
                DidChangeValue (nameof (name));
            }
        }

        private string _description;
        public string description {
            [Export (nameof (description))]
            get {
                return _description;
            }
            set {
                WillChangeValue (nameof (description));
                _description = value;
                DidChangeValue (nameof (description));
            }
        }

        private bool _allowsUserEdits;
        public bool allowsUserEdits {
            [Export (nameof (allowsUserEdits))]
            get {
                return _allowsUserEdits;
            }
            set {
                WillChangeValue (nameof (allowsUserEdits));
                _allowsUserEdits = value;
                DidChangeValue (nameof (allowsUserEdits));
            }
        }


        private NSMutableArray suggestions = new NSMutableArray ();

        public NSArray Suggestions {
            [Export ("suggestionModelArray")]
            get { return suggestions; }
        }


        [Export ("setSuggestionModelArray:")]
        public void SetSuggestions (NSMutableArray array)
        {
            WillChangeValue ("suggestionModelArray");
            suggestions = array;
            DidChangeValue ("suggestionModelArray");
        }
    }
}
