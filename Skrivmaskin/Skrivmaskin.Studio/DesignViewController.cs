// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Collections.Generic;
using Foundation;
using AppKit;
using Skrivmaskin.Design;
using Skrivmaskin.Compiler;
using Skrivmaskin.Lexing;

namespace Skrivmaskin.Studio
{
    public partial class DesignViewController : NSViewController
    {
        CentralViewController parent = null;

        public DesignViewController (IntPtr handle) : base (handle)
        {
        }

        public void SetControllerLinks (CentralViewController cvc)
        {
            parent = cvc;
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
        }

        public override void ViewDidAppear ()
        {
            base.ViewDidAppear ();

            var windowController = NSApplication.SharedApplication.KeyWindow.WindowController as SkrivmaskinWindowController;
            if (!parent.inGenerateOnlyMode && windowController.IsInNew) {
                windowController.IsInNew = false;
                PerformSegue ("CreateTemplate", this);
            }
        }

        #region Edits from the tree to a Project
        void DocumentEditedAction ()
        {
            if (!loading) {
                // Reread project from the outline view
                var project = CreateProjectFromOutlineView ();
                if (!project.Equals (parent.Project)) {
                    NSApplication.SharedApplication.KeyWindow.DocumentEdited = true;
                    parent.Project = project;
                    parent.CompiledProject = parent.Compiler.Compile (parent.Project); // has a caching layer so should be quick
                }
            }
        }

        #endregion

        #region Seque to dialog
        public override void PrepareForSegue (NSStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue (segue, sender);

            if (segue.DestinationController is CreateTemplateViewController) {
                var dlg = segue.DestinationController as CreateTemplateViewController;
                dlg.Presentor = this;
                switch (segue.Identifier) {
                case DesignViewDialogSegues.CreateTemplate:
                    dlg.DialogAccepted += (s, e) => {
                        var project = new Project (new List<Variable> (), OutputSplitter.Split (dlg.SampleText));
                        parent.CreateTree (null, project);
                    };
                    break;
                case DesignViewDialogSegues.AddFromSample:
                    dlg.DialogAccepted += (s, e) => {
                        var designNode = OutputSplitter.Split (dlg.SampleText);
                        var selected = (DesignModel)TreeController.SelectedObjects [0];
                        string errorText;
                        CreateDefinition (designNode, false, (n) => AddChild (n), selected.isNodeActive, out errorText);
                    };
                    break;
                default:
                    break;
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
                dialog.showSuggestion = false;
                dialog.DialogAccepted += (s, e) => {
                    TreeController.RemoveObjectsAtArrangedObjectIndexPaths (TreeController.SelectionIndexPaths);
                    DocumentEditedAction ();
                };
                break;
            case DesignViewDialogSegues.Edit:
                var selected = (DesignModel)TreeController.SelectedObjects [0];
                dialog.titleText = "Edit";
                dialog.descriptionText = "Edit this " + selected.modelType + ".";
                dialog.showActive = (selected.modelType != DesignModelType.VariableForm && selected.modelType != DesignModelType.Variable);
                dialog.showName = (selected.modelType != DesignModelType.Text && selected.modelType != DesignModelType.ParagraphBreak);
                dialog.showDetails = (selected.modelType != DesignModelType.Choice && selected.modelType != DesignModelType.Sequential && selected.modelType != DesignModelType.ParagraphBreak);
                dialog.showSuggestion = false;
                dialog.NameTextInput = selected.name;
                dialog.DetailsTextInput = selected.details;
                dialog.IsActiveInput = selected.isActive;
                if (selected.modelType == DesignModelType.Text) dialog.CompiledProject = parent.CompiledProject;
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
                dialog.showSuggestion = false;
                dialog.DetailsTextInput = "";
                dialog.IsActiveInput = true;
                dialog.CompiledProject = parent.CompiledProject;
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
                dialog.showSuggestion = false;
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
                dialog.showSuggestion = false;
                dialog.NameTextInput = "Sequential";
                dialog.IsActiveInput = true;
                dialog.DialogAccepted += (s, e) => {
                    AddChildModel (DesignModelType.Sequential, dialog.NameTextOutput, "", dialog.IsActiveOutput);
                };
                break;
            case DesignViewDialogSegues.AddVariable:
                dialog.titleText = "Add Variable";
                dialog.descriptionText = "Add new variable.";
                dialog.showActive = false;
                dialog.showName = true;
                dialog.showDetails = true;
                dialog.showSuggestion = true;
                dialog.NameTextInput = "VARNAME";
                dialog.detailsText = "Description:";
                dialog.DetailsTextInput = "Description for this variable";
                dialog.DialogAccepted += (s, e) => {
                    var variable = new DesignModel (DesignModelType.Variable, dialog.NameTextOutput, dialog.DetailsTextOutput, true, true);
                    variable.AddDesign (new DesignModel (DesignModelType.VariableForm, "", dialog.SuggestionTextOutput, true, true));
                    AddChild (variable);
                };
                break;
            case DesignViewDialogSegues.AddVariableVariant:
                dialog.titleText = "Add Variable variant";
                dialog.descriptionText = "Add new variant.";
                dialog.showActive = false;
                dialog.showName = true;
                dialog.showDetails = true;
                dialog.showSuggestion = false;
                dialog.NameTextInput = "";
                dialog.detailsText = "Suggestion:";
                dialog.DetailsTextInput = "Suggestion";
                dialog.DialogAccepted += (s, e) => {
                    AddChildModel (DesignModelType.VariableForm, dialog.NameTextOutput, dialog.DetailsTextOutput, true);
                };
                break;
            default:
                break;
            }
        }

        partial void Add_ParagraphBreak (Foundation.NSObject sender)
        {
            AddChildModel (DesignModelType.ParagraphBreak, "", "", true);
        }

        private void AddChildModel (DesignModelType modelType, string name, string details, bool isActive)
        {
            var selected = ((DesignModel)TreeController.SelectedObjects [0]);
            var model = new DesignModel (modelType, name, details, isActive, selected.isNodeActive);
            selected.AddDesign (model);
            var selectionIndexPath = TreeController.SelectionIndexPaths [0];
            var newIndexPath = selectionIndexPath.IndexPathByAddingIndex ((nuint)(selected.numberOfDesigns - 1));
            TreeController.RemoveSelectionIndexPaths (TreeController.SelectionIndexPaths);
            TreeController.AddSelectionIndexPaths (new NSIndexPath [1] { newIndexPath });
            DocumentEditedAction ();
        }

        private void AddChild (DesignModel model)
        {
            var selected = ((DesignModel)TreeController.SelectedObjects [0]);
            selected.AddDesign (model);
            DocumentEditedAction ();
        }
        #endregion

        #region Move Up/Down
        partial void MoveUp_Clicked (NSObject sender)
        {
            // Find parent, remove at index, add at (index - 1), select.
            var selectionIndexPath = TreeController.SelectionIndexPaths [0];
            var childModel = (DesignModel)TreeController.SelectedObjects [0];
            var lastIndex = selectionIndexPath.IndexAtPosition (selectionIndexPath.Length - 1);
            var parentIndexPath = selectionIndexPath.IndexPathByRemovingLastIndex();
            TreeController.RemoveSelectionIndexPaths (new NSIndexPath [1] { selectionIndexPath });
            TreeController.AddSelectionIndexPaths (new NSIndexPath [1] { parentIndexPath });
            var parentNode = (DesignModel)TreeController.SelectedObjects [0];
            parentNode.RemoveDesign ((nint)lastIndex);
            parentNode.InsertDesign (childModel, (nint)(lastIndex - 1));
            TreeController.RemoveSelectionIndexPaths (new NSIndexPath [1] { parentIndexPath });
            TreeController.AddSelectionIndexPaths (new NSIndexPath [1] { parentIndexPath.IndexPathByAddingIndex (lastIndex - 1) });
            DocumentEditedAction ();
        }

        partial void MoveDown_Clicked (NSObject sender)
        {
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
        Project CreateProjectFromOutlineView ()
        {
            var variablesNode = Designs.GetItem<DesignModel> ((nuint)0);
            var definitionNode = Designs.GetItem<DesignModel> ((nuint)1);
            var variables = new List<Variable> ();
            for (int i = 0; i < variablesNode.numberOfDesigns; i++) {
                var variableNode = variablesNode.Designs.GetItem<DesignModel> ((nuint)i);
                var variable = new Variable ();
                variable.Name = variableNode.name;
                variable.Description = variableNode.details;
                variable.Forms = new List<VariableForm> ();
                for (int j = 0; j < variableNode.numberOfDesigns; j++) {
                    var variableFormNode = variableNode.Designs.GetItem<DesignModel> ((nuint)j);
                    var variableForm = new VariableForm ();
                    variableForm.Name = variableFormNode.name;
                    variableForm.Suggestion = variableFormNode.details;
                    variable.Forms.Add (variableForm);
                }
                variables.Add (variable);
            }
            var root = CreateDesignNode (definitionNode);
            return new Project (variables, root);
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
            var variables = new DesignModel (true, DesignModelType.VariableRoot, "Variables", "", true, true);
            AddDesign (variables);
            if (this.CreateVariables (parent.Project, variables, out errorText)) {
                if (this.CreateDefinition (parent.Project.Definition, true, (d) => AddDesign (d), true, out errorText)) {
                    parent.CompiledProject = parent.Compiler.Compile (parent.Project);
                    loading = false;
                    return;
                }
            }
            loading = false;
        }

        public bool CreateVariables (Project project, DesignModel variables, out string errorText)
        {
            foreach (var variable in project.VariableDefinitions) {
                var model = new DesignModel (DesignModelType.Variable, variable.Name, variable.Description, true, true);
                variables.AddDesign (model);
                foreach (var form in variable.Forms) {
                    model.AddDesign (new DesignModel (DesignModelType.VariableForm, form.Name, form.Suggestion, true, true));
                }
            }
            errorText = "";
            return true;
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
            var indexPath = (new NSIndexPath ()).IndexPathByAddingIndex (1);
            var retVal = FindAndSelectDesignNode (designNode, parent.Project.Definition, ref indexPath);
            if (retVal) {
                TreeController.RemoveSelectionIndexPaths (TreeController.SelectionIndexPaths);
                TreeController.AddSelectionIndexPaths (new NSIndexPath [1] { indexPath });
            }
            return retVal;
        }
	}
}
