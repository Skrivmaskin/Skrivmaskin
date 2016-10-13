using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using TextOn.Nouns;

namespace TextOn.Studio
{
    public partial class SetNounValuesItemViewController : AppKit.NSCollectionViewItem
    {
        private static NSImage unsetImage = NSImage.ImageNamed ("unset");
        private static NSImage validImage = NSImage.ImageNamed ("valid");
        private static NSImage invalidImage = NSImage.ImageNamed ("invalid");

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
        public SetNounValuesItemViewController () : base (nameof (SetNounValuesItemView), NSBundle.MainBundle)
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

        string nounName;
        NounSetValuesSession session;
        public void Activate (NounSetValuesSession session, string nounName)
        {
            this.session = session;
            this.nounName = nounName;
            NounNameTextField.StringValue = nounName;
            DescriptionTextView.Value = session.GetDescription (nounName);
            StatusImage.Image = unsetImage;
            SuggestionsComboBox.Editable = session.GetAcceptsUserValue (nounName);
            session.SuggestionsUpdated += Session_SuggestionsUpdated;
            SuggestionsComboBox.EditingEnded += SuggestionsComboBox_Changed;
            SuggestionsComboBox.SelectionChanged += SuggestionsComboBox_Changed;
            SuggestionsComboBox.StringValue = "";
            Session_SuggestionsUpdated (nounName);
            session.Deactivating += Deactivate;
        }

        public void Deactivate ()
        {
            session.SuggestionsUpdated -= Session_SuggestionsUpdated;
            session.Deactivating -= Deactivate;
            SuggestionsComboBox.EditingEnded -= SuggestionsComboBox_Changed;
            SuggestionsComboBox.SelectionChanged -= SuggestionsComboBox_Changed;
            session = null;
        }

        void SuggestionsComboBox_Changed (object sender, EventArgs e)
        {
            session.SetValue (nounName, SuggestionsComboBox.StringValue);
        }

        private void Session_SuggestionsUpdated (string name)
        {
            if (nounName != name) return;
            SuggestionsComboBox.DataSource = new SetNounValuesItemSuggestionsComboBoxDataSource (session.GetCurrentSuggestionsForNoun (name));
        }
    }
}
