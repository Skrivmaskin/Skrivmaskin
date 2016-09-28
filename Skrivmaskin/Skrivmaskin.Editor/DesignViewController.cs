// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using AppKit;
using Skrivmaskin.Compiler;
using Skrivmaskin.Lexing;
using Skrivmaskin.Design;
using System.Collections.Generic;

namespace Skrivmaskin.Editor
{
    public partial class DesignViewController : NSViewController
    {
        private SkrivmaskinCompiler compiler = new SkrivmaskinCompiler (new DefaultLexerSyntax ());

        public DesignViewController (IntPtr handle) : base (handle)
        {
        }

        bool loading = false;

        internal void SelectionChanged (NSObservedChange change)
        {
            WillChangeValue ("hideConvertToChoice");
            WillChangeValue ("hideConvertToSequential");
            WillChangeValue ("enableDelete");
            WillChangeValue ("enableAdd");
            WillChangeValue ("enableAddVariant");
            DidChangeValue ("hideConvertToChoice");
            DidChangeValue ("hideConvertToSequential");
            DidChangeValue ("enableDelete");
            DidChangeValue ("enableAdd");
            DidChangeValue ("enableAddVariant");
        }

        internal void DocumentEditedAction ()
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

        Project CreateProjectFromOutlineView ()
        {
            var variablesNode = Designs.GetItem<DesignModel> ((nuint)0);
            var definitionNode = Designs.GetItem<DesignModel> ((nuint)1);
            var variables = new List<Variable> ();
            for (int i = 0; i < variablesNode.NumberOfDesigns; i++) {
                var variableNode = variablesNode.Designs.GetItem<DesignModel> ((nuint)i);
                var variable = new Variable ();
                variable.Name = variableNode.Name;
                variable.Description = variableNode.Details;
                variable.Forms = new List<VariableForm> ();
                for (int j = 0; j < variableNode.NumberOfDesigns; j++) {
                    var variableFormNode = variableNode.Designs.GetItem<DesignModel> ((nuint)j);
                    var variableForm = new VariableForm ();
                    variableForm.Name = variableFormNode.Name;
                    variableForm.Suggestion = variableFormNode.Details;
                    variable.Forms.Add (variableForm);
                }
                variables.Add (variable);
            }
            var root = CreateDesignNode (definitionNode);
            return new Project (variables, root);
        }

        INode CreateDesignNode (DesignModel designModel)
        {
            switch (designModel.NodeType) {
            case DesignModelType.Text:
                return new TextNode (designModel.Details, designModel.IsActive);
            case DesignModelType.Choice:
                var li = new List<INode> ();
                for (int i = 0; i < designModel.NumberOfDesigns; i++) {
                    li.Add (CreateDesignNode (designModel.Designs.GetItem<DesignModel> ((nuint)i)));
                }
                return new ChoiceNode (designModel.Name, designModel.IsActive, li);
            case DesignModelType.Sequential:
                var li2 = new List<INode> ();
                for (int i = 0; i < designModel.NumberOfDesigns; i++) {
                    li2.Add (CreateDesignNode (designModel.Designs.GetItem<DesignModel> ((nuint)i)));
                }
                return new SequentialNode (designModel.Name, designModel.IsActive, li2);
            case DesignModelType.ParagraphBreak:
                return new ParagraphBreakNode (designModel.IsActive);
            default:
                break;
            }
            throw new ApplicationException ("Unrecognised node type " + designModel.NodeType);
        }

        private void DocumentEdited (NSObservedChange sender)
        {
            DocumentEditedAction ();
        }

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

        public bool CreateVariables (Project project, DesignModel variables, out string errorText)
        {
            foreach (var variable in project.VariableDefinitions) {
                var model = new DesignModel (variable);
                variables.AddDesign (model);
                foreach (var form in variable.Forms) {
                    model.AddDesign (new DesignModel (form));
                }
            }
            errorText = "";
            return true;
        }

        SetVariablesViewController setVariablesViewController = null;
        ResultsViewController resultsViewController = null;
        internal void SetControllerLinks (SetVariablesViewController svvc, ResultsViewController rvc)
        {
            setVariablesViewController = svvc;
            resultsViewController = rvc;
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

        public bool CreateTree (Project project)
        {
            string errorText;
            loading = true;
            Project = project; // no edits yet so no need to inform Apple about it
            var array = new NSMutableArray ();
            SetDesigns (array);
            var variables = new DesignModel ("Variables");
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

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            this.AddObserver ("designModelArray", NSKeyValueObservingOptions.New, DocumentEdited);
            this.TreeController.AddObserver ("selectedObjects", NSKeyValueObservingOptions.New, SelectionChanged);

            loading = true;
            var array = new NSMutableArray ();
            SetDesigns (array);
            var variables = new DesignModel ("Variables");
            AddDesign (variables);
            var definition = new DesignModel (true, DesignModelType.Sequential, "Definition", "");
            AddDesign (definition);
            loading = false;
        }

        public override NSObject RepresentedObject {
            get {
                return base.RepresentedObject;
            }
            set {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        partial void Add_ParagraphBreak (Foundation.NSObject sender)
        {
            var model = TreeController.SelectedObjects [0] as DesignModel;
            model.AddDesign (new DesignModel (DesignModelType.ParagraphBreak, "", ""));
        }

        partial void Delete_Item (NSObject sender)
        {
        }

        public override void PrepareForSegue (NSStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue (segue, sender);
            DesignModel selected = null;
            if (TreeController.SelectedObjects.Length == 1)
                selected = (DesignModel)TreeController.SelectedObjects [0];
            var dialog = segue.DestinationController as GeneralPurposeDialogController;
            if (dialog == null) return;

            // Take action based on the segue name
            switch (segue.Identifier) {
            case DesignViewDialogSegues.AddChoice:
                dialog.DialogTitle = "Add New Choice";
                dialog.DialogMessage = "Add a new node that will randomly choose between a number of options to insert into the output.";
                dialog.NameLabelInput = "Name:";
                dialog.DialogAccepted += (s, e) => {
                    selected.AddDesign (new DesignModel (DesignModelType.Choice, dialog.NameTextOutput, ""));
                    DocumentEditedAction ();
                };
                dialog.showNameEditField = true;
                dialog.showDetailsEditField = false;
                dialog.showSuggestionEditField = false;
                dialog.showActive = true;
                break;
            case DesignViewDialogSegues.AddSequential:
                dialog.DialogTitle = "Add New Sequential";
                dialog.DialogMessage = "Add a new node that will include all of the elements in its sub tree sequentially.";
                dialog.NameLabelInput = "Name:";
                dialog.DialogAccepted += (s, e) => {
                    selected.AddDesign (new DesignModel (DesignModelType.Sequential, dialog.NameTextOutput, ""));
                    DocumentEditedAction ();
                };
                dialog.showNameEditField = true;
                dialog.showDetailsEditField = false;
                dialog.showSuggestionEditField = false;
                dialog.showActive = true;
                break;
            case DesignViewDialogSegues.AddText:
                dialog.DialogTitle = "Add New Text";
                dialog.DialogMessage = "Add a new node that will be parsed to generate a sentence or phrase.";
                dialog.DetailsLabelInput = "Text:";
                dialog.DialogAccepted += (s, e) => {
                    selected.AddDesign (new DesignModel (DesignModelType.Text, "", dialog.DetailsTextOutput));
                    DocumentEditedAction ();
                };
                dialog.showNameEditField = false;
                dialog.showDetailsEditField = true;
                dialog.showSuggestionEditField = false;
                dialog.showActive = true;
                break;
            case DesignViewDialogSegues.AddVariable:
                dialog.DialogTitle = "Add New Variable";
                dialog.DialogMessage = "Add a new variable that can be used for a user replacement in the text.";
                dialog.NameLabelInput = "Name:";
                dialog.DetailsLabelInput = "Description:";
                dialog.DialogAccepted += (s, e) => {
                    var variable = new DesignModel (new Variable () { Name = dialog.NameTextOutput, Description = dialog.DetailsTextOutput });
                    ((DesignModel)Designs.GetItem<DesignModel> ((nuint)0)).AddDesign (variable);
                    variable.AddDesign (new DesignModel (new VariableForm () { Name = "", Suggestion = dialog.SuggestionTextOutput }));
                    DocumentEditedAction ();
                };
                dialog.showNameEditField = true;
                dialog.showDetailsEditField = true;
                dialog.showSuggestionEditField = true;
                dialog.showActive = false;
                break;
            case DesignViewDialogSegues.AddVariableVariant:
                dialog.DialogTitle = "Add New Variable Variant";
                dialog.DialogMessage = "Add a new variant for " + selected.Name;
                dialog.NameLabelInput = "Name:";
                dialog.DialogAccepted += (s, e) => {
                    selected.AddDesign (new DesignModel (new VariableForm () { Name = dialog.NameTextOutput, Suggestion = dialog.SuggestionTextOutput }));
                    DocumentEditedAction ();
                };
                dialog.showNameEditField = true;
                dialog.showDetailsEditField = false;
                dialog.showSuggestionEditField = true;
                dialog.showActive = false;
                break;
            case DesignViewDialogSegues.Edit:
                dialog.DialogTitle = "Edit " + selected.NodeType;
                dialog.DialogMessage = "Edit this " + selected.NodeType + ".";
                dialog.NameLabelInput = "Name:";
                dialog.DetailsLabelInput = (selected.NodeType == DesignModelType.Text) ? "Text:" : "Description";
                dialog.NameTextInput = selected.Name;
                dialog.DetailsTextInput = selected.Details;
                dialog.SuggestionTextInput = selected.Details;
                dialog.DialogAccepted += (s, e) => {
                    switch (selected.NodeType) {
                    case DesignModelType.Text:
                        selected.Details = dialog.DetailsTextOutput;
                        break;
                    case DesignModelType.Choice:
                    case DesignModelType.Sequential:
                        selected.Name = dialog.NameTextOutput;
                        break;
                    case DesignModelType.Variable:
                        selected.Name = dialog.NameTextOutput;
                        selected.Details = dialog.DetailsTextOutput;
                        break;
                    case DesignModelType.VariableForm:
                        selected.Name = dialog.NameTextOutput;
                        selected.Details = dialog.SuggestionTextOutput;
                        break;
                    default:
                        break;
                    }
                    DocumentEditedAction ();
                };
                dialog.showNameEditField = (selected.NodeType == DesignModelType.Choice || selected.NodeType == DesignModelType.Sequential || selected.NodeType == DesignModelType.Variable || selected.NodeType == DesignModelType.VariableForm);
                dialog.showDetailsEditField = (selected.NodeType == DesignModelType.Text || selected.NodeType == DesignModelType.Variable);
                dialog.showSuggestionEditField = (selected.NodeType == DesignModelType.VariableForm);
                dialog.showActive = (selected.NodeType != DesignModelType.Variable && selected.NodeType != DesignModelType.VariableForm);
                break;
            case DesignViewDialogSegues.Delete:
                dialog.DialogTitle = "Delete " + selected.NodeType;
                dialog.DialogMessage = "Are you sure? This will delete any sub nodes also.";
                dialog.DialogAccepted += (s, e) => {
                    foreach (var indexPath in TreeController.SelectionIndexPaths) {
                        TreeController.RemoveObjectAtArrangedObjectIndexPath (indexPath);
                    }
                    DocumentEditedAction ();
                };
                dialog.showNameEditField =false;
                dialog.showDetailsEditField = false;
                dialog.showSuggestionEditField = false;
                dialog.showActive = false;
                break;
}

            dialog.Presentor = this;
        }

        public bool EnableDelete {
            [Export ("enableDelete")]
            get {
                return false;
            }
        }

        public bool EnableAdd {
            [Export ("enableAdd")]
            get {
                if (TreeController.SelectedObjects.Length != 1) return false;
                var selected = (DesignModel)TreeController.SelectedObjects [0];
                if (selected.NodeType == DesignModelType.Sequential || selected.NodeType == DesignModelType.Choice) return true;
                return false;
            }
        }

        public bool EnableRename {
            [Export ("enableRename")]
            get {
                if (TreeController.SelectedObjects.Length != 1) return false;
                var selected = (DesignModel)TreeController.SelectedObjects [0];
                if (selected.NodeType == DesignModelType.Sequential || selected.NodeType == DesignModelType.Choice) return true;
                return false;
            }
        }

        public bool EnableAddVariant {
            [Export ("enableAddVariant")]
            get {
                if (TreeController.SelectedObjects.Length != 1) return false;
                var selected = (DesignModel)TreeController.SelectedObjects [0];
                if (selected.NodeType == DesignModelType.Variable) return true;
                return false;
            }
        }

        public bool HideConvertToChoice {
            [Export ("hideConvertToChoice")]
            get {
                if (TreeController.SelectedObjects.Length != 1) return true;
                var selected = (DesignModel)TreeController.SelectedObjects [0];
                if (selected.NodeType == DesignModelType.Sequential) return false;
                return true;
            }
        }

        public bool HideConvertToSequential {
            [Export ("hideConvertToSequential")]
            get {
                if (TreeController.SelectedObjects.Length != 1) return true;
                var selected = (DesignModel)TreeController.SelectedObjects [0];
                if (selected.NodeType == DesignModelType.Choice) return false;
                return true;
            }
        }

        partial void ConvertToChoice (Foundation.NSObject sender)
        {
            if (TreeController.SelectedObjects.Length != 1) return;
            var selected = (DesignModel)TreeController.SelectedObjects [0];
            if (selected.NodeType == DesignModelType.Sequential) {
                WillChangeValue ("hideConvertToChoice");
                WillChangeValue ("hideConvertToSequential");
                WillChangeValue ("designModelArray");
                selected.NodeType = DesignModelType.Choice;
                DidChangeValue ("hideConvertToChoice");
                DidChangeValue ("hideConvertToSequential");
                DidChangeValue ("designModelArray");
            }
        }

        partial void ConvertToSequential (Foundation.NSObject sender)
        {
            if (TreeController.SelectedObjects.Length != 1) return;
            var selected = (DesignModel)TreeController.SelectedObjects [0];
            if (selected.NodeType == DesignModelType.Choice) {
                WillChangeValue ("hideConvertToChoice");
                WillChangeValue ("hideConvertToSequential");
                WillChangeValue ("designModelArray");
                selected.NodeType = DesignModelType.Sequential;
                DidChangeValue ("designModelArray");
                DidChangeValue ("hideConvertToChoice");
                DidChangeValue ("hideConvertToSequential");
            }
        }
    }
}
