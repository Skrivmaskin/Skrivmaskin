// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using AppKit;
using Skrivmaskin.Compiler;

namespace Skrivmaskin.Studio
{
	public partial class GeneralPurposeDialogController : NSViewController
	{
		public GeneralPurposeDialogController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            Title = titleText;
            NameTextField.StringValue = NameTextInput;
            DetailsTextView.CompiledProject = CompiledProject;
            DetailsTextView.Value = DetailsTextInput;
            IsActive.State = (IsActiveInput ? NSCellStateValue.On : NSCellStateValue.Off);
            if (CompiledProject != null) {
                DetailsTextView.Highlight ();
                DetailsTextView.SelectAll (this);
            }
        }

        public bool IsActiveInput { get; set; }

        public bool IsActiveOutput {
            get {
                return IsActive.State == NSCellStateValue.On;
            }
        }

        public CompiledProject CompiledProject { get; set; } = null;

        private string _titleText = "";
        public string titleText {
            [Export (nameof (titleText))]
            get {
                return _titleText;
            }
            set {
                _titleText = value;
            }
        }

        private string _descriptionText = "";
        public string descriptionText {
            [Export (nameof (descriptionText))]
            get {
                return _descriptionText;
            }
            set {
                _descriptionText = value;
            }
        }

        private string _detailsText = "";
        public string detailsText {
            [Export (nameof (detailsText))]
            get {
                return _detailsText;
            }
            set {
                _detailsText = value;
            }
        }

        private bool _showActive;
        public bool showActive {
            [Export (nameof (showActive))]
            get {
                return _showActive;
            }
            set {
                _showActive = value;
            }
        }

        public string NameTextOutput {
            get {
                return NameTextField.StringValue;
            }
        }

        public string DetailsTextOutput {
            get {
                return DetailsTextView.Value;
            }
        }

        public string SuggestionTextOutput {
            get {
                return SuggestionTextField.StringValue;
            }
        }

        public string NameTextInput { get; set; } = "";

        public string DetailsTextInput { get; set; } = "";

        private bool _showName;
        public bool showName {
            [Export (nameof (showName))]
            get {
                return _showName;
            }
            set {
                _showName = value;
            }
        }

        private bool _showDetails;
        public bool showDetails {
            [Export (nameof (showDetails))]
            get {
                return _showDetails;
            }
            set {
                _showDetails = value;
            }
        }

        private bool _showSuggestion;
        public bool showSuggestion {
            [Export (nameof (showSuggestion))]
            get {
                return _showSuggestion;
            }
            set {
                _showSuggestion = value;
            }
        }

        public NSViewController Presentor { get; set; }

        private void CloseDialog ()
        {
            Presentor.DismissViewController (this);
        }

        partial void Cancel_Clicked (Foundation.NSObject sender)
        {
            CloseDialog ();
        }

        partial void OK_Clicked (Foundation.NSObject sender)
        {
            RaiseDialogAccepted ();
            CloseDialog ();
        }

        public EventHandler DialogAccepted;

        internal void RaiseDialogAccepted ()
        {
            if (this.DialogAccepted != null)
                this.DialogAccepted (this, EventArgs.Empty);
        }
	}
}
