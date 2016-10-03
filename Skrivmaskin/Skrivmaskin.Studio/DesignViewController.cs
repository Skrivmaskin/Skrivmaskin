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
        public DesignViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            Project = new Project (new List<Variable> (), new SequentialNode ("Sentences", true, new List<INode> ()));
            CreateTree (Project);
            CompiledProject = compiler.Compile (Project);
        }

        #region Edits from the tree to a Project
        void DocumentEditedAction ()
        {
            if (!loading) {
                // Reread project from the outline view
                var project = CreateProjectFromOutlineView ();
                if (!project.Equals (Project)) {
                    NSApplication.SharedApplication.KeyWindow.DocumentEdited = true;
                    Project = project;
                    CompiledProject = compiler.Compile (Project); // has a caching layer so should be quick
                }
            }
        }
        #endregion

        #region Seque to dialog
        public override void PrepareForSegue (NSStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue (segue, sender);

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
                dialog.showActive = false;
                dialog.showName = (selected.modelType != DesignModelType.Text);
                dialog.showDetails = (selected.modelType != DesignModelType.Choice && selected.modelType != DesignModelType.Sequential);
                dialog.showSuggestion = false;
                dialog.NameTextInput = selected.name;
                dialog.DetailsTextInput = selected.details;
                if (selected.modelType == DesignModelType.Text) dialog.CompiledProject = CompiledProject;
                dialog.DialogAccepted += (s, e) => {
                    selected.name = dialog.NameTextOutput;
                    selected.details = dialog.DetailsTextOutput;
                    DocumentEditedAction ();
                };
                break;
            case DesignViewDialogSegues.AddText:
                dialog.titleText = "Add Text";
                dialog.descriptionText = "Add new text.";
                dialog.showActive = false;
                dialog.showName = false;
                dialog.detailsText = "Text:";
                dialog.showDetails = true;
                dialog.showSuggestion = false;
                dialog.DetailsTextInput = "";
                dialog.CompiledProject = CompiledProject;
                dialog.DialogAccepted += (s, e) => {
                    AddChildModel (new DesignModel (DesignModelType.Text, "", dialog.DetailsTextOutput));
                };
                break;
            case DesignViewDialogSegues.AddChoice:
                dialog.titleText = "Add Choice";
                dialog.descriptionText = "Add new choice.";
                dialog.showActive = false;
                dialog.showName = true;
                dialog.showDetails = false;
                dialog.showSuggestion = false;
                dialog.NameTextInput = "Choice";
                dialog.DialogAccepted += (s, e) => {
                    AddChildModel (new DesignModel (DesignModelType.Choice, dialog.NameTextOutput, ""));
                };
                break;
            case DesignViewDialogSegues.AddSequential:
                dialog.titleText = "Add Sequential";
                dialog.descriptionText = "Add new sequential.";
                dialog.showActive = false;
                dialog.showName = true;
                dialog.showDetails = false;
                dialog.showSuggestion = false;
                dialog.NameTextInput = "Sequential";
                dialog.DialogAccepted += (s, e) => {
                    AddChildModel (new DesignModel (DesignModelType.Sequential, dialog.NameTextOutput, ""));
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
                    var variable = new DesignModel (DesignModelType.Variable, dialog.NameTextOutput, dialog.DetailsTextOutput);
                    variable.AddDesign (new DesignModel (DesignModelType.VariableForm, "", dialog.SuggestionTextOutput));
                    AddChildModel (variable);
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
                    AddChildModel (new DesignModel (DesignModelType.VariableForm, dialog.NameTextOutput, dialog.DetailsTextOutput));
                };
                break;
            default:
                break;
            }
        }

        partial void Add_ParagraphBreak (Foundation.NSObject sender)
        {
            AddChildModel (new DesignModel (DesignModelType.ParagraphBreak, "", ""));
        }

        private void AddChildModel (DesignModel model)
        {
            ((DesignModel)TreeController.SelectedObjects [0]).AddDesign (model);
            DocumentEditedAction ();
        }
        #endregion

        #region Relationships with other ViewControllers
        SetVariablesViewController setVariablesViewController = null;
        ResultsViewController resultsViewController = null;
        internal void SetControllerLinks (SetVariablesViewController svvc, ResultsViewController rvc)
        {
            setVariablesViewController = svvc;
            resultsViewController = rvc;
        }
        #endregion

        #region Design and Compiled projects
        private SkrivmaskinCompiler compiler = new SkrivmaskinCompiler (new DefaultLexerSyntax ());
        internal Project Project { get; private set; } = new Project (new List<Variable> (), new SequentialNode ("Sentences", true, new List<INode> ()));
        private CompiledProject compiledProject = null;
        private CompiledProject CompiledProject {
            get {
                return compiledProject;
            }
            set {
                compiledProject = value;
                if (setVariablesViewController != null) {
                    setVariablesViewController.SetCompiledProject (compiledProject);
                    resultsViewController.SetCompiledProject (setVariablesViewController.VariableValues, compiledProject);
                }
            }
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

        bool loading = false;

        public bool CreateTree (Project project)
        {
            string errorText;
            loading = true;
            Project = project; // no edits yet so no need to inform Apple about it
            var array = new NSMutableArray ();
            SetDesigns (array);
            var variables = new DesignModel (true, DesignModelType.VariableRoot, "Variables", "");
            AddDesign (variables);
            if (this.CreateVariables (project, variables, out errorText)) {
                if (this.CreateDefinition (project.Definition, true, (d) => AddDesign (d), out errorText)) {
                    CompiledProject = compiler.Compile (project);
                    loading = false;
                    return true;
                }
            }
            loading = false;
            return false;
        }

        public bool CreateVariables (Project project, DesignModel variables, out string errorText)
        {
            foreach (var variable in project.VariableDefinitions) {
                var model = new DesignModel (DesignModelType.Variable, variable.Name, variable.Description);
                variables.AddDesign (model);
                foreach (var form in variable.Forms) {
                    model.AddDesign (new DesignModel (DesignModelType.VariableForm, form.Name, form.Suggestion));
                }
            }
            errorText = "";
            return true;
        }

        public bool CreateDefinition (INode designNode, bool root, Action<DesignModel> addDefn, out string errorText)
        {
            IEnumerable<INode> children;
            DesignModel design;
            switch (designNode.Type) {
            case NodeType.Choice:
                children = (designNode as ChoiceNode).Choices;
                design = new DesignModel (root, DesignModelType.Choice, (designNode as ChoiceNode).ChoiceName, "");
                break;
            case NodeType.Sequential:
                children = (designNode as SequentialNode).Sequential;
                design = new DesignModel (root, DesignModelType.Sequential, (designNode as SequentialNode).SequentialName, "");
                break;
            case NodeType.ParagraphBreak:
                children = new INode [0];
                design = new DesignModel (root, DesignModelType.ParagraphBreak, "Paragraph Break", "");
                break;
            case NodeType.Text:
                children = new INode [0];
                design = new DesignModel (root, DesignModelType.Text, "", (designNode as TextNode).Text);
                break;
            default:
                throw new ApplicationException ("Unrecognised design node type " + designNode.Type);
            }
            addDefn (design);
            foreach (var child in children) {
                if (!CreateDefinition (child, false, (d) => design.AddDesign (d), out errorText))
                    return false;
            }
            errorText = "";
            return true;
        }

        #endregion
	}
}
