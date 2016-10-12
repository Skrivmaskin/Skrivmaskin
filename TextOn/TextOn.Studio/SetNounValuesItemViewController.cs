using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace TextOn.Studio
{
    public partial class SetNounValuesItemViewController : AppKit.NSCollectionViewItem
    {
        #region Constructors

        // Called when created from unmanaged code
        public SetNounValuesItemViewController (IntPtr handle) : base (handle)
        {
            Initialize ();
        }

        // Called when created directly from a XIB file
        [Export ("initWithCoder:")]
        public SetNounValuesItemViewController (NSCoder coder) : base (coder)
        {
            Initialize ();
        }

        // Call to load from the XIB/NIB file
        public SetNounValuesItemViewController () : base (nameof(SetNounValuesItemView), NSBundle.MainBundle)
        {
            Initialize ();
        }

        // Shared initialization code
        void Initialize ()
        {
        }

        #endregion

        //strongly typed view accessor
        public new SetNounValuesItemView View {
            get {
                return (SetNounValuesItemView)base.View;
            }
        }

        internal string NounName {
            set {
                NounNameTextField.StringValue = value;
            }
        }
    }
}
