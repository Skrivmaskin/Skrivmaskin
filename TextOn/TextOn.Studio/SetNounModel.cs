using System;
using AppKit;
using Foundation;

namespace TextOn.Studio
{
    [Register (nameof (SetNounModel))]
    public class SetNounModel : NSObject
    {
        private static NSImage unsetImage = NSImage.ImageNamed ("unset");
        private static NSImage validImage = NSImage.ImageNamed ("valid");
        private static NSImage invalidImage = NSImage.ImageNamed ("invalid");

        private string _name;
        public string nounName {
            [Export (nameof (nounName))]
            get {
                return _name;
            }
        }

        private string _description;
        public string nounDescription {
            [Export (nameof (nounDescription))]
            get {
                return _description;
            }
        }

        private NSMutableArray _suggestionValues = new NSMutableArray ();
        public NSArray suggestionValues {
            [Export (nameof (suggestionValues))]
            get {
                return _suggestionValues;
            }
        }

        public void SetSuggestions (string [] suggestions)
        {
            var arr = new NSMutableArray ();
            foreach (var sugg in suggestions) {
                arr.Add ((NSString)sugg);
            }
            WillChangeValue (nameof (suggestionValues));
            _suggestionValues = arr;
            DidChangeValue (nameof (_suggestionValues));
        }

        private NSImage _statusImage = unsetImage;
        public NSImage statusImage {
            [Export (nameof (statusImage))]
            get {
                return _statusImage;
            }
            set {
                WillChangeValue (nameof (statusImage));
                _statusImage = value;
                DidChangeValue (nameof (statusImage));
            }
        }

        private string _nounValue = "";
        public string nounValue {
            [Export (nameof (nounValue))]
            get {
                return _nounValue;
            }
        }

        private bool _acceptsUserValue = true;
        public bool acceptsUserValue {
            [Export (nameof (acceptsUserValue))]
            get {
                return _acceptsUserValue;
            }
        }

        public SetNounModel (string name, string description, bool nounAcceptsUserValue)
        {
            _name = name;
            _description = description;
            _acceptsUserValue = nounAcceptsUserValue;
        }

        public event Action<string, string> NounValueSet;
    }
}
