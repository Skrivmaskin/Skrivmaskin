// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using AppKit;
using CoreGraphics;
using System.Collections.Generic;
using TextOn.Nouns;
using log4net;

namespace TextOn.Studio
{
    public partial class DefineNounsViewController : NSViewController
    {
        private static readonly ILog Log = LogManager.GetLogger (nameof (DefineNounsViewController));

        public DefineNounsViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
        }

        bool apparent = false;

        public override void ViewDidAppear ()
        {
            Log.Debug ("Appeared");

            base.ViewDidAppear ();

            apparent = true;
            TemplateUpdated ();
        }

        public override void ViewDidDisappear ()
        {
            Log.Debug ("Disappeared");
            
            base.ViewDidDisappear ();

            DefineNounsTableView.DataSource = null;
            DefineNounsTableView.Delegate = null;
            apparent = false;
        }

        internal void TemplateUpdated ()
        {
            Log.Debug ("TemplateUpdated");

            centralViewController.Template.Nouns.NounsInOrderChanged += Refresh;
            centralViewController.Template.Nouns.SuggestionsChangedForNoun += (s) => Refresh ();
            if (apparent) {
                var datasource = new DefineNounsTableViewDataSource (centralViewController.Template.Nouns);
                DefineNounsTableView.DataSource = datasource;
                DefineNounsTableView.Delegate = new DefineNounsTableViewDelegate (this, datasource);
            }
        }

        private void Refresh ()
        {
            Log.Debug ("Refresh");
            
            DefineNounsTableView.ReloadData ();
        }

        private CentralViewController centralViewController = null;
        internal void SetControllerLinks (CentralViewController cvc)
        {
            centralViewController = cvc;
        }

        public override void PrepareForSegue (NSStoryboardSegue segue, NSObject sender)
        {
            Log.DebugFormat ("PrepareForSegue,{0}", segue.Identifier);
            
            base.PrepareForSegue (segue, sender);

            switch (segue.Identifier) {
            case DesignViewDialogSegues.AddNewNoun:
                var dlg = segue.DestinationController as AddNewNounViewController;
                dlg.Presentor = this;
                dlg.DialogAccepted += (s, e) => {
                    centralViewController.Template.Nouns.AddNewNoun (dlg.NameText, dlg.DescriptionText, dlg.AcceptsUserValue);
                };
                break;
            case DesignViewDialogSegues.DeleteSuggestion:
                var ddlg = segue.DestinationController as DeleteSuggestionViewController;
                ddlg.Presentor = this;
                ddlg.NounName = nounNameToDelete;
                ddlg.SuggestionValue = suggestionValueToDelete;
                ddlg.DialogAccepted += (s, e) => {
                    centralViewController.Template.Nouns.DeleteSuggestion (nounNameToDelete, suggestionValueToDelete);
                };
                break;
            case DesignViewDialogSegues.ManageConstraints:
                var mdlg = segue.DestinationController as ManageConstraintsDialogController;
                mdlg.Presentor = this;
                mdlg.NounName = nounNameToManage;
                mdlg.SuggestionValue = suggestionValueToManage;
                mdlg.Profile = centralViewController.Template.Nouns;
                mdlg.DialogAccepted += (s, e) => {
                    var newDependencies = mdlg.NewDependencies;
                    centralViewController.Template.Nouns.SetDependenciesForSuggestion (nounNameToManage, suggestionValueToManage, newDependencies);
                    if (mdlg.ApplyToFutureSuggestions) {
                        currentDefaultDependencies [nounNameToManage] = newDependencies;
                    } else {
                        currentDefaultDependencies [nounNameToManage] = new NounSuggestionDependency [0];
                    }
                };
                break;
            default:
                break;
            }
        }

        private readonly Dictionary<string, IEnumerable<NounSuggestionDependency>> currentDefaultDependencies = new Dictionary<string, IEnumerable<NounSuggestionDependency>> ();
        internal IEnumerable<NounSuggestionDependency> GetCurrentDefaultDependenciesForThisNoun (string name)
        {
            if (currentDefaultDependencies.ContainsKey (name)) return currentDefaultDependencies [name];
            return new NounSuggestionDependency [0];
        }

        //TODO Everything below is horribly un-thread-safe, surely? Fix this!!! Or just hope I get away with it?
        private string nounNameToDelete;
        private string suggestionValueToDelete;
        public void DeleteSuggestion (string nounName, string suggestionValue)
        {
            nounNameToDelete = nounName;
            suggestionValueToDelete = suggestionValue;
            PerformSegue (DesignViewDialogSegues.DeleteSuggestion, this);
        }

        private string nounNameToManage;
        private string suggestionValueToManage;
        public void ManageConstraints (string nounName, string suggestionValue)
        {
            nounNameToManage = nounName;
            suggestionValueToManage = suggestionValue;
            PerformSegue (DesignViewDialogSegues.ManageConstraints, this);
        }

   }
}
