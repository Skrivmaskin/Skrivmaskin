// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Linq;
using System.Collections.Generic;
using Foundation;
using AppKit;
using TextOn.Design;
using TextOn.Compiler;
using TextOn.Lexing;
using TextOn.Generation;
using TextOn.Nouns;

namespace TextOn.Studio
{
    public partial class DesignViewController : NSViewController
    {
        CentralViewController centralViewController = null;

        public DesignViewController (IntPtr handle) : base (handle)
        {
        }

        public void SetControllerLinks (CentralViewController cvc)
        {
            Console.Error.WriteLine ("Design SetControllerLinks");

            centralViewController = cvc;
        }

        public override void ViewDidLoad ()
        {
            Console.Error.WriteLine ("Design ViewDidLoad");

            base.ViewDidLoad ();
        }

        IDisposable disp;
        bool firstTimeAppearing = true;

        public override void ViewDidAppear ()
        {
            Console.Error.WriteLine ("Design ViewDidAppear");

            base.ViewDidAppear ();

            if (firstTimeAppearing) {
                Console.Error.WriteLine ("Design ViewDidAppear (firstTimeAppearing)");

                firstTimeAppearing = false;
                OutlineView.TreeController = TreeController;

                var windowController = NSApplication.SharedApplication.KeyWindow?.WindowController as TextOnWindowController;
                if (centralViewController == null)
                    throw new ApplicationException ("centralViewController has not been set");
                if ((windowController != null) && (windowController.IsInNew)) {
                    windowController.IsInNew = false;
                    PerformSegue (DesignViewDialogSegues.CreateTemplate, this);
                }

                if (PreviewSplitViewController == null)
                    throw new ApplicationException ("SplitViewController is null");
                if (TreeController == null)
                    throw new ApplicationException ("TreeController is null");

                previewSplitViewItem = PreviewSplitViewController.SplitViewItems [1];
                defineNounsSplitViewItem= NounsSplitViewController.SplitViewItems [1];

                disp = TreeController.AddObserver ("selectionIndexPaths", NSKeyValueObservingOptions.New, SelectionChanged);
            }       
        }

        private bool fiddlingWithSelection = false;
        public void SelectionChanged (NSObservedChange change)
        {
            Console.Error.WriteLine ("Design SelectionChanged");

            if (!fiddlingWithSelection && !loading) {
                UpdatePreview ();
            }
            disp.Dispose ();
            disp = TreeController.AddObserver ("selectionIndexPaths", NSKeyValueObservingOptions.New, SelectionChanged);
        }

        public void UpdatePreview ()
        {
            Console.Error.WriteLine ("Design UpdatePreview");

            if (!previewIsHidden && TreeController.SelectionIndexPaths.Length == 1) {
                List<PreviewPartialRouteChoiceNode> partialRoute = new List<PreviewPartialRouteChoiceNode> ();
                var node = centralViewController.Template.Definition;
                var indexPath = TreeController.SelectionIndexPaths [0];
                var indices = indexPath.GetIndexes ().Skip (1).Select ((n) => (int)n);
                var choicesMade = 0;
                foreach (var index in indices) {
                    if (node.Type == NodeType.Sequential)
                        node = ((SequentialNode)node).Sequential [index];
                    else if (node.Type == NodeType.Choice) {
                        var choiceNode = (ChoiceNode)node;
                        partialRoute.Add (new PreviewPartialRouteChoiceNode (choiceNode, index, choicesMade++, null));
                        node = choiceNode.Choices [index];
                    } else {
                        break;
                    }
                }
                partialRoute.Add (new PreviewPartialRouteChoiceNode (null, -1, choicesMade, node));
                centralViewController.GeneratePreview (partialRoute.ToArray ());
            }
        }

        #region Edits from the tree to a TextOnTemplate
        void DocumentEditedAction ()
        {
            Console.Error.WriteLine ("Design DocumentEditedAction");

            if (!loading) {
                // Reread project from the outline view
                var template = CreateTemplateFromOutlineView ();
                NSApplication.SharedApplication.KeyWindow.DocumentEdited = true;
                centralViewController.Template = template;
                centralViewController.CompiledTemplate = centralViewController.Compiler.Compile (centralViewController.Template); // has a caching layer so should be quick
            }

            centralViewController.MarkPreviewAsInvalid ();
        }

        #endregion

        #region Seque to dialog
        public override void PrepareForSegue (NSStoryboardSegue segue, NSObject sender)
        {
            Console.Error.WriteLine ("Design PrepareForSegue");

            base.PrepareForSegue (segue, sender);

            if (segue.DestinationController is CreateTemplateViewController) {
                var dlg = segue.DestinationController as CreateTemplateViewController;
                dlg.Presentor = this;
                switch (segue.Identifier) {
                case DesignViewDialogSegues.CreateTemplate:
                    dlg.TitleText = "Create Template";
                    dlg.DescriptionText = "Paste sample text below to create a new template outline.";
                    dlg.DialogAccepted += (s, e) => {
                        var project = new TextOnTemplate (new NounProfile (), OutputSplitter.Split (dlg.SampleText));
                        centralViewController.CreateTree (null, project);
                    };
                    break;
                case DesignViewDialogSegues.AddFromSample:
                    dlg.TitleText = "Add From Sample";
                    dlg.DescriptionText = "Paste sample text below to add the outline for a new subtree.";
                    dlg.DialogAccepted += (s, e) => {
                        var designNode = OutputSplitter.Split (dlg.SampleText);
                        var selected = (DesignModel)TreeController.SelectedObjects [0];
                        string errorText;
                        CreateDefinition (designNode, false, (n) => AddChild (n), selected.isNodeActive, out errorText);
                        DocumentEditedAction ();
                    };
                    break;
                default:
                    break;
                }
                return;
            }

            if (segue.DestinationController is MakeChoiceViewController) {
                var dlg = segue.DestinationController as MakeChoiceViewController;
                dlg.Presentor = this;
                dlg.CompiledTemplate = centralViewController.CompiledTemplate;
                var selected = (DesignModel)TreeController.SelectedObjects [0];
                var isChoice = segue.Identifier == DesignViewDialogSegues.MoveIntoNewChoice;
                dlg.isChoice = isChoice;
                dlg.TitleText = isChoice ? "Move Into New Choice" : "Move Into New Sequential";
                dlg.NameTextInput = isChoice ? "New Choice" : "New Sequential";
                if (selected.modelType == DesignModelType.Text) {
                    dlg.isTextNodeChoice = true;
                    dlg.DescriptionText = "Press the + button to add more text to the new " + (isChoice ? "choice" : "sequential") + " node.";
                    dlg.SampleText = selected.details;
                    dlg.DialogAccepted += (s, e) => {
                        fiddlingWithSelection = true;
                        var selectionIndexPath = TreeController.SelectionIndexPaths [0];
                        // Find parent, remove at index, add a new choice/sequential, add all the text nodes.
                        var lastIndex = selectionIndexPath.IndexAtPosition (selectionIndexPath.Length - 1);
                        var parentIndexPath = selectionIndexPath.IndexPathByRemovingLastIndex ();
                        TreeController.RemoveSelectionIndexPaths (new NSIndexPath [1] { selectionIndexPath });
                        TreeController.AddSelectionIndexPaths (new NSIndexPath [1] { parentIndexPath });
                        var parentNode = (DesignModel)TreeController.SelectedObjects [0];
                        parentNode.RemoveDesign ((nint)lastIndex);
                        var newChildModel = new DesignModel ((isChoice ? DesignModelType.Choice : DesignModelType.Sequential), dlg.NameText, "", true, parentNode.isNodeActive);
                        parentNode.InsertDesign (newChildModel, (nint)lastIndex);
                        foreach (var text in dlg.TextItems) {
                            TreeController.RemoveSelectionIndexPaths (TreeController.SelectionIndexPaths);
                            TreeController.AddSelectionIndexPaths (new NSIndexPath [1] { parentIndexPath.IndexPathByAddingIndex (lastIndex) });
                            AddChildModel (DesignModelType.Text, "", text, true);
                        }
                        TreeController.RemoveSelectionIndexPaths (TreeController.SelectionIndexPaths);
                        TreeController.AddSelectionIndexPaths (new NSIndexPath [1] { parentIndexPath.IndexPathByAddingIndex (lastIndex).IndexPathByAddingIndex (0) });
                        DocumentEditedAction ();
                        fiddlingWithSelection = false;
                        UpdatePreview ();
                    };
                } else {
                    dlg.isTextNodeChoice = false;
                    dlg.DescriptionText = "Are you sure? This will encapsulate the current node in a new " + (isChoice ? "choice" : "sequential") + " node.";
                    dlg.SampleText = "";
                    dlg.DialogAccepted += (s, e) => {
                        fiddlingWithSelection = true;
                        var selectionIndexPath = TreeController.SelectionIndexPaths [0];
                        if (selectionIndexPath.Length == 1) {
                            // Find parent, remove at index, add a new choice/sequential, re-add the old selected.
                            var childModel = (DesignModel)TreeController.SelectedObjects [0];
                            TreeController.RemoveSelectionIndexPaths (TreeController.SelectionIndexPaths);
                            TreeController.AddSelectionIndexPaths (new NSIndexPath [1] { NSIndexPath.Create (new nint [] { 0 }) });
                            RemoveDesign (1);
                            var newRootModel = new DesignModel (true, (isChoice ? DesignModelType.Choice : DesignModelType.Sequential), dlg.NameText, "", true, true);
                            AddDesign (newRootModel);
                            newRootModel.AddDesign (childModel);
                            TreeController.RemoveSelectionIndexPaths (TreeController.SelectionIndexPaths);
                            TreeController.AddSelectionIndexPaths (new NSIndexPath [1] { NSIndexPath.Create (new nint [] { 1, 0 }) });
                        } else {
                            // Find parent, remove at index, add a new choice/sequential, re-add the old selected.
                            var childModel = (DesignModel)TreeController.SelectedObjects [0];
                            var lastIndex = selectionIndexPath.IndexAtPosition (selectionIndexPath.Length - 1);
                            var parentIndexPath = selectionIndexPath.IndexPathByRemovingLastIndex ();
                            TreeController.RemoveSelectionIndexPaths (new NSIndexPath [1] { selectionIndexPath });
                            TreeController.AddSelectionIndexPaths (new NSIndexPath [1] { parentIndexPath });
                            var parentNode = (DesignModel)TreeController.SelectedObjects [0];
                            parentNode.RemoveDesign ((nint)lastIndex);
                            var newChildModel = new DesignModel ((isChoice ? DesignModelType.Choice : DesignModelType.Sequential), dlg.NameText, "", true, parentNode.isNodeActive);
                            parentNode.InsertDesign (newChildModel, (nint)lastIndex);
                            TreeController.RemoveSelectionIndexPaths (TreeController.SelectionIndexPaths);
                            TreeController.AddSelectionIndexPaths (new NSIndexPath [1] { parentIndexPath.IndexPathByAddingIndex (lastIndex) });
                            AddChild (childModel);
                            TreeController.RemoveSelectionIndexPaths (TreeController.SelectionIndexPaths);
                            TreeController.AddSelectionIndexPaths (new NSIndexPath [1] { parentIndexPath.IndexPathByAddingIndex (lastIndex).IndexPathByAddingIndex (0) });
                        }
                        DocumentEditedAction ();
                        fiddlingWithSelection = false;
                        UpdatePreview ();
                    };
                }
                return;
            }

            // All the segues I care about are dialog ones.
            var dialog = segue.DestinationController as GeneralPurposeDialogController;
            if (dialog == null) return;

            // So that the dialog can close.
            dialog.Presentor = this;

            // Set up.
            switch (segue.Identifier) {
            case DesignViewDialogSegues.Delete:
                dialog.titleText = "Delete";
                dialog.descriptionText = "Are you sure? This will delete all subnodes also.";
                dialog.showActive = false;
                dialog.showName = false;
                dialog.showDetails = false;
                dialog.DialogAccepted += (s, e) => {
                    TreeController.RemoveObjectsAtArrangedObjectIndexPaths (TreeController.SelectionIndexPaths);
                    DocumentEditedAction ();
                };
                break;
            case DesignViewDialogSegues.Edit:
                var selected = (DesignModel)TreeController.SelectedObjects [0];
                dialog.titleText = "Edit";
                dialog.descriptionText = "Edit this " + selected.modelType + ".";
                dialog.showActive = true;
                dialog.showName = (selected.modelType != DesignModelType.Text && selected.modelType != DesignModelType.ParagraphBreak);
                dialog.showDetails = (selected.modelType != DesignModelType.Choice && selected.modelType != DesignModelType.Sequential && selected.modelType != DesignModelType.ParagraphBreak);
                dialog.NameTextInput = selected.name;
                dialog.DetailsTextInput = selected.details;
                dialog.IsActiveInput = selected.isActive;
                if (selected.modelType == DesignModelType.Text) dialog.CompiledTemplate = centralViewController.CompiledTemplate;
                dialog.DialogAccepted += (s, e) => {
                    selected.name = dialog.NameTextOutput;
                    selected.details = dialog.DetailsTextOutput;
                    selected.isActive = dialog.IsActiveOutput;
                    DocumentEditedAction ();
                };
                break;
            case DesignViewDialogSegues.AddText:
                dialog.titleText = "Add Text";
                dialog.descriptionText = "Add new text.";
                dialog.showActive = true;
                dialog.showName = false;
                dialog.detailsText = "Text:";
                dialog.showDetails = true;
                dialog.DetailsTextInput = "";
                dialog.IsActiveInput = true;
                dialog.CompiledTemplate = centralViewController.CompiledTemplate;
                dialog.DialogAccepted += (s, e) => {
                    AddChildModel (DesignModelType.Text, "", dialog.DetailsTextOutput, dialog.IsActiveOutput);
                };
                break;
            case DesignViewDialogSegues.AddChoice:
                dialog.titleText = "Add Choice";
                dialog.descriptionText = "Add new choice.";
                dialog.showActive = true;
                dialog.showName = true;
                dialog.showDetails = false;
                dialog.NameTextInput = "Choice";
                dialog.IsActiveInput = true;
                dialog.DialogAccepted += (s, e) => {
                    AddChildModel (DesignModelType.Choice, dialog.NameTextOutput, "", dialog.IsActiveOutput);
                };
                break;
            case DesignViewDialogSegues.AddSequential:
                dialog.titleText = "Add Sequential";
                dialog.descriptionText = "Add new sequential.";
                dialog.showActive = true;
                dialog.showName = true;
                dialog.showDetails = false;
                dialog.NameTextInput = "Sequential";
                dialog.IsActiveInput = true;
                dialog.DialogAccepted += (s, e) => {
                    AddChildModel (DesignModelType.Sequential, dialog.NameTextOutput, "", dialog.IsActiveOutput);
                };
                break;
            default:
                throw new ApplicationException ("Unknown segue - " + segue.Identifier);
            }
        }

        partial void Add_ParagraphBreak (Foundation.NSObject sender)
        {
            AddChildModel (DesignModelType.ParagraphBreak, "", "", true);
        }

        private void AddChildModel (DesignModelType modelType, string name, string details, bool isActive)
        {
            Console.Error.WriteLine ("Design AddChildModel");

            fiddlingWithSelection = true;
            var selected = ((DesignModel)TreeController.SelectedObjects [0]);
            var model = new DesignModel (modelType, name, details, isActive, selected.isNodeActive);
            selected.AddDesign (model);
            var selectionIndexPath = TreeController.SelectionIndexPaths [0];
            var newIndexPath = selectionIndexPath.IndexPathByAddingIndex ((nuint)(selected.numberOfDesigns - 1));
            TreeController.RemoveSelectionIndexPaths (TreeController.SelectionIndexPaths);
            TreeController.AddSelectionIndexPaths (new NSIndexPath [1] { newIndexPath });
            DocumentEditedAction ();
            fiddlingWithSelection = false;
            UpdatePreview ();
        }

        private void AddChild (DesignModel model)
        {
            Console.Error.WriteLine ("Design AddChild");

            var selected = ((DesignModel)TreeController.SelectedObjects [0]);
            selected.AddDesign (model);
            DocumentEditedAction ();
        }
        #endregion

        #region Move Up/Down
        partial void MoveUp_Clicked (NSObject sender)
        {
            Console.Error.WriteLine ("Design MoveUp_Clicked");

            fiddlingWithSelection = true;
            // Find parent, remove at index, add at (index - 1), select.
            var selectionIndexPath = TreeController.SelectionIndexPaths [0];
            var childModel = (DesignModel)TreeController.SelectedObjects [0];
            var lastIndex = selectionIndexPath.IndexAtPosition (selectionIndexPath.Length - 1);
            var parentIndexPath = selectionIndexPath.IndexPathByRemovingLastIndex ();
            TreeController.RemoveSelectionIndexPaths (new NSIndexPath [1] { selectionIndexPath });
            TreeController.AddSelectionIndexPaths (new NSIndexPath [1] { parentIndexPath });
            var parentNode = (DesignModel)TreeController.SelectedObjects [0];
            parentNode.RemoveDesign ((nint)lastIndex);
            parentNode.InsertDesign (childModel, (nint)(lastIndex - 1));
            TreeController.RemoveSelectionIndexPaths (new NSIndexPath [1] { parentIndexPath });
            TreeController.AddSelectionIndexPaths (new NSIndexPath [1] { parentIndexPath.IndexPathByAddingIndex (lastIndex - 1) });
            DocumentEditedAction ();
            fiddlingWithSelection = false;
            UpdatePreview ();
        }

        partial void MoveDown_Clicked (NSObject sender)
        {
            Console.Error.WriteLine ("Design MoveDown_Clicked");

            fiddlingWithSelection = true;
            // Find parent, remove at index, add at (index + 1), select.
            var selectionIndexPath = TreeController.SelectionIndexPaths [0];
            var childModel = (DesignModel)TreeController.SelectedObjects [0];
            var lastIndex = selectionIndexPath.IndexAtPosition (selectionIndexPath.Length - 1);
            var parentIndexPath = selectionIndexPath.IndexPathByRemovingLastIndex ();
            TreeController.RemoveSelectionIndexPaths (new NSIndexPath [1] { selectionIndexPath });
            TreeController.AddSelectionIndexPaths (new NSIndexPath [1] { parentIndexPath });
            var parentNode = (DesignModel)TreeController.SelectedObjects [0];
            parentNode.RemoveDesign ((nint)lastIndex);
            parentNode.InsertDesign (childModel, (nint)(lastIndex + 1));
            TreeController.RemoveSelectionIndexPaths (new NSIndexPath [1] { parentIndexPath });
            TreeController.AddSelectionIndexPaths (new NSIndexPath [1] { parentIndexPath.IndexPathByAddingIndex (lastIndex + 1) });
            DocumentEditedAction ();
            fiddlingWithSelection = false;
            UpdatePreview ();
        }
        #endregion

        private NSMutableArray designs = new NSMutableArray ();
        public NSArray Designs {
            [Export ("designModelArray")]
            get { return designs; }
        }

        [Export ("addObject:")]
        public void AddDesign (DesignModel design)
        {
            WillChangeValue ("designModelArray");
            designs.Add (design);
            DidChangeValue ("designModelArray");
        }

        [Export ("insertObject:inDesignModelArrayAtIndex:")]
        public void InsertDesign (DesignModel design, nint index)
        {
            WillChangeValue ("designModelArray");
            designs.Insert (design, index);
            DidChangeValue ("designModelArray");
        }

        [Export ("removeObjectFromDesignModelArrayAtIndex:")]
        public void RemoveDesign (nint index)
        {
            WillChangeValue ("designModelArray");
            designs.RemoveObject (index);
            DidChangeValue ("designModelArray");
        }

        [Export ("setDesignModelArray:")]
        public void SetDesigns (NSMutableArray array)
        {
            WillChangeValue ("designModelArray");
            designs = array;
            DidChangeValue ("designModelArray");
        }

        #region Mapping to and from Project
        /// <summary>
        /// Following an edit, recurse through the view and generate the design time project.
        /// </summary>
        /// <returns>The project from outline view.</returns>
        TextOnTemplate CreateTemplateFromOutlineView ()
        {
            Console.Error.WriteLine ("Design CreateTemplateFromOutlineView");

            var definitionNode = Designs.GetItem<DesignModel> ((nuint)0);
            var nounProfile = centralViewController.Template?.Nouns;
            var root = CreateDesignNode (definitionNode);
            return new TextOnTemplate (nounProfile, root);
        }

        INode CreateDesignNode (DesignModel designModel)
        {
            switch (designModel.modelType) {
            case DesignModelType.Text:
                return new TextNode (designModel.details, designModel.isActive);
            case DesignModelType.Choice:
                var li = new List<INode> ();
                for (int i = 0; i < designModel.numberOfDesigns; i++) {
                    li.Add (CreateDesignNode (designModel.Designs.GetItem<DesignModel> ((nuint)i)));
                }
                return new ChoiceNode (designModel.name, designModel.isActive, li);
            case DesignModelType.Sequential:
                var li2 = new List<INode> ();
                for (int i = 0; i < designModel.numberOfDesigns; i++) {
                    li2.Add (CreateDesignNode (designModel.Designs.GetItem<DesignModel> ((nuint)i)));
                }
                return new SequentialNode (designModel.name, designModel.isActive, li2);
            case DesignModelType.ParagraphBreak:
                return new ParagraphBreakNode (designModel.isActive);
            default:
                break;
            }
            throw new ApplicationException ("Unrecognised node type " + designModel.modelType);
        }

        private bool loading = false;

        public void CreateTree ()
        {
            string errorText;
            loading = true;
            var array = new NSMutableArray ();
            SetDesigns (array);
            if (this.CreateDefinition (centralViewController.Template.Definition, true, (d) => AddDesign (d), true, out errorText)) {
                centralViewController.CompiledTemplate = centralViewController.Compiler.Compile (centralViewController.Template);
                loading = false;
                return;
            }
            loading = false;
        }

        public bool CreateDefinition (INode designNode, bool root, Action<DesignModel> addDefn, bool isParentActive, out string errorText)
        {
            IEnumerable<INode> children;
            DesignModel design;
            switch (designNode.Type) {
            case NodeType.Choice:
                children = (designNode as ChoiceNode).Choices;
                design = new DesignModel (root, DesignModelType.Choice, (designNode as ChoiceNode).ChoiceName, "", designNode.IsActive, isParentActive);
                break;
            case NodeType.Sequential:
                children = (designNode as SequentialNode).Sequential;
                design = new DesignModel (root, DesignModelType.Sequential, (designNode as SequentialNode).SequentialName, "", designNode.IsActive, isParentActive);
                break;
            case NodeType.ParagraphBreak:
                children = new INode [0];
                design = new DesignModel (root, DesignModelType.ParagraphBreak, "Paragraph Break", "", designNode.IsActive, isParentActive);
                break;
            case NodeType.Text:
                children = new INode [0];
                design = new DesignModel (root, DesignModelType.Text, "", (designNode as TextNode).Text, designNode.IsActive, isParentActive);
                break;
            default:
                throw new ApplicationException ("Unrecognised design node type " + designNode.Type);
            }
            addDefn (design);
            foreach (var child in children) {
                if (!CreateDefinition (child, false, (d) => design.AddDesign (d), design.isNodeActive, out errorText))
                    return false;
            }
            errorText = "";
            return true;
        }

        #endregion

        private bool FindAndSelectDesignNode (INode designNode, INode reachedNode, ref NSIndexPath indexPath)
        {
            if (designNode == reachedNode) return true;
            IEnumerable<INode> children;
            switch (reachedNode.Type) {
            case NodeType.Choice:
                children = ((ChoiceNode)reachedNode).Choices;
                break;
            case NodeType.Sequential:
                children = ((SequentialNode)reachedNode).Sequential;
                break;
            default:
                children = new INode [0];
                break;
            }
            nuint i = 0;
            foreach (var childNode in children) {
                NSIndexPath innerIndexPath = indexPath.IndexPathByAddingIndex (i);
                if (FindAndSelectDesignNode (designNode, childNode, ref innerIndexPath)) {
                    indexPath = innerIndexPath;
                    return true;
                }
                ++i;
            }
            return false;
        }

        internal bool SelectDesignNode (INode designNode)
        {
            var indexPath = (new NSIndexPath ()).IndexPathByAddingIndex (0);
            var retVal = FindAndSelectDesignNode (designNode, centralViewController.Template.Definition, ref indexPath);
            if (retVal) {
                TreeController.RemoveSelectionIndexPaths (TreeController.SelectionIndexPaths);
                TreeController.AddSelectionIndexPaths (new NSIndexPath [1] { indexPath });
            }
            return retVal;
        }

        #region Split View management
        internal NSSplitViewController PreviewSplitViewController { get; set; } = null;
        private NSSplitViewItem previewSplitViewItem = null;

        private bool previewIsHidden = false;
        public bool PreviewIsHidden {
            get {
                return previewIsHidden;
            }
            set {
                WillChangeValue (nameof (hideShowPreviewTitle));
                WillChangeValue (nameof (hideShowPreviewTooltip));
                if (PreviewSplitViewController != null) {
                    if (value && !previewIsHidden) {
                        PreviewSplitViewController.RemoveSplitViewItem (previewSplitViewItem);
                    } else if (!value && previewIsHidden) {
                        PreviewSplitViewController.AddSplitViewItem (previewSplitViewItem);
                        UpdatePreview ();
                    }
                    previewIsHidden = value;
                }
                DidChangeValue (nameof (hideShowPreviewTitle));
                DidChangeValue (nameof (hideShowPreviewTooltip));
            }
        }

        private bool defineNounsIsHidden = false;
        public bool DefineNounsIsHidden {
            get {
                return defineNounsIsHidden;
            }
            set {
                WillChangeValue (nameof (hideShowDefineNounsTitle));
                WillChangeValue (nameof (hideShowDefineNounsTooltip));
                if (NounsSplitViewController != null) {
                    if (value && !defineNounsIsHidden) {
                        NounsSplitViewController.RemoveSplitViewItem (defineNounsSplitViewItem);
                    } else if (!value && defineNounsIsHidden) {
                        NounsSplitViewController.AddSplitViewItem (defineNounsSplitViewItem);
                    }
                    defineNounsIsHidden = value;
                }
                DidChangeValue (nameof (hideShowDefineNounsTitle));
                DidChangeValue (nameof (hideShowDefineNounsTooltip));
            }
        }

        public string hideShowPreviewTitle {
            [Export (nameof (hideShowPreviewTitle))]
            get {
                return previewIsHidden ? "Show Preview" : "Hide Preview";
            }
        }

        public string hideShowPreviewTooltip {
            [Export (nameof (hideShowPreviewTooltip))]
            get {
                return previewIsHidden ? "Show the Preview pane." : "Hide the Preview pane.";
            }
        }

        public string hideShowDefineNounsTitle {
            [Export (nameof (hideShowDefineNounsTitle))]
            get {
                return defineNounsIsHidden ? "Show Nouns" : "Hide Nouns";
            }
        }

        public string hideShowDefineNounsTooltip {
            [Export (nameof (hideShowDefineNounsTooltip))]
            get {
                return defineNounsIsHidden ? "Show the Nouns pane." : "Hide the Nouns pane.";
            }
        }

        public NSSplitViewController NounsSplitViewController { get; internal set; }
        private NSSplitViewItem defineNounsSplitViewItem = null;
        #endregion

		partial void HideShowPreview_Clicked (NSObject sender)
        {
            PreviewIsHidden = !PreviewIsHidden;
        }

        partial void HideShowDefineNouns_Clicked (NSObject sender)
        {
            DefineNounsIsHidden = !DefineNounsIsHidden;
        }
    }
}
