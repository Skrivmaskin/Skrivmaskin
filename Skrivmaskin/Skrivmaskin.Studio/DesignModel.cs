using System;
using AppKit;
using Foundation;
using Skrivmaskin.Design;

namespace Skrivmaskin.Studio
{
    [Register ("DesignModel")]
    public class DesignModel : NSObject
    {
        private string _name = "";
        public string name {
            [Export (nameof (name))]
            get { return _name; }
            set {
                WillChangeValue (nameof(name));
                _name = value;
                DidChangeValue (nameof (name));
            }
        }

        public string nameToolTip {
            [Export (nameof (nameToolTip))]
            get {
                switch (_modelType) {
                case DesignModelType.Text:
                    return "Text to be inserted into the output.";
                case DesignModelType.Choice:
                    return "One of the subnodes will be randomly chosen for the output.";
                case DesignModelType.Sequential:
                    return "All of the subnodes will be included sequentially in the output.";
                case DesignModelType.VariableRoot:
                    return "Root node";
                case DesignModelType.Variable:
                    return "A variable to be substituted into the output, using the [VARNAME] syntax.";
                case DesignModelType.VariableForm:
                    return "A grammatical variant of the variable to be substituted into the output, using the [VARNAME|Variant] syntax.";
                case DesignModelType.ParagraphBreak:
                    return "A paragraph break";
                }
                throw new ApplicationException ("Unknown nodde type " + _modelType);
            }
        }

        public string detailsToolTip {
            [Export (nameof (detailsToolTip))]
            get {
                switch (_modelType) {
                case DesignModelType.Text:
                    return "Write text here.";
                case DesignModelType.Choice:
                case DesignModelType.Sequential:
                    return "Insert subnodes.";
                case DesignModelType.VariableRoot:
                    return "Root node";
                case DesignModelType.Variable:
                    return "Variable definition - used as a prompt for the user.";
                case DesignModelType.VariableForm:
                    return "Suggestion for the value of this variant.";
                case DesignModelType.ParagraphBreak:
                    return "A paragraph break";
                }
                throw new ApplicationException ("Unknown nodde type " + _modelType);
            }
        }

        private string _details = "";
        public string details {
            [Export (nameof (details))]
            get { return _details; }
            set {
                WillChangeValue (nameof(details));
                _details = value;
                DidChangeValue (nameof(details));
            }
        }

        private DesignModelType _modelType;
        public DesignModelType modelType {
            get { return _modelType; }
            set {
                WillChangeValue (nameof (icon));
                WillChangeValue (nameof (enableConvertToChoice));
                WillChangeValue (nameof (enableConvertToSequential));
                _modelType = value;
                DidChangeValue (nameof (icon));
                DidChangeValue (nameof (enableConvertToChoice));
                DidChangeValue (nameof (enableConvertToSequential));
            }
        }

        private bool _isActive = false;
        public bool isActive {
            [Export (nameof(isActive))]
            get { return _isActive; }
            set {
                WillChangeValue (nameof (icon));
                WillChangeValue (nameof (textColor));
                WillChangeValue (nameof (enableConvertToChoice));
                WillChangeValue (nameof (enableConvertToSequential));
                _isActive = value;
                DidChangeValue (nameof (icon));
                DidChangeValue (nameof (textColor));
                DidChangeValue (nameof (enableConvertToChoice));
                DidChangeValue (nameof (enableConvertToSequential));

                // set children's parents for the GUI
                for (nuint i = 0; i < designs.Count; i++) {
                    var model = designs.GetItem<DesignModel> (i);
                    model.isParentActive = isNodeActive;
                }
            }
        }

        private bool _isParentActive = true;
        public bool isParentActive {
            [Export (nameof (isParentActive))]
            get {
                return _isParentActive;
            }
            set {
                WillChangeValue (nameof (icon));
                WillChangeValue (nameof (textColor));
                WillChangeValue (nameof (enableConvertToChoice));
                WillChangeValue (nameof (enableConvertToSequential));
                _isParentActive = value;
                DidChangeValue (nameof (icon));
                DidChangeValue (nameof (textColor));
                DidChangeValue (nameof (isParentActive));
                DidChangeValue (nameof (isNodeActive));

                // set children's parents for the GUI
                for (nuint i = 0; i < designs.Count; i++) {
                    var model = designs.GetItem<DesignModel> (i);
                    model.isParentActive = isNodeActive;
                }
            }
        }

        public bool isNodeActive {
            [Export (nameof (isNodeActive))]
            get {
                return _isActive && _isParentActive;
            }
        }

        public bool isLeaf {
            [Export (nameof(isLeaf))]
            get { return designs.Count == 0; }
        }

        public bool enableAdd {
            [Export (nameof(enableAdd))]
            get {
                switch (modelType) {
                case DesignModelType.Choice:
                case DesignModelType.Sequential:
                    return true;
                default:
                    return false;
                }
            }
        }

        public bool enableAddVariableVariant {
            [Export (nameof (enableAddVariableVariant))]
            get {
                return (modelType == DesignModelType.Variable);
            }
        }


        public bool enableAddVariable {
            [Export (nameof (enableAddVariable))]
            get {
                return (modelType == DesignModelType.VariableRoot);
            }
        }

        public NSImage icon {
            [Export (nameof (icon))]
            get {
                switch (modelType) {
                case DesignModelType.VariableRoot:
                    return NSImage.ImageNamed (NSImageName.Folder);
                case DesignModelType.Variable:
                case DesignModelType.VariableForm:
                    return NSImage.ImageNamed (NSImageName.UserGuest);
                case DesignModelType.Text:
                    return NSImage.ImageNamed (NSImageName.GoRightTemplate);
                case DesignModelType.Choice:
                    return (isNodeActive ? NSImage.ImageNamed (NSImageName.StatusPartiallyAvailable) : NSImage.ImageNamed (NSImageName.RemoveTemplate));
                case DesignModelType.Sequential:
                    return (isNodeActive ? NSImage.ImageNamed (NSImageName.StatusNone) : NSImage.ImageNamed (NSImageName.RemoveTemplate));
                case DesignModelType.ParagraphBreak:
                    return NSImage.ImageNamed (NSImageName.QuickLookTemplate);
                default:
                    return null;
                }
            }
        }

        public NSColor textColor {
            [Export (nameof (textColor))]
            get {
                return (isNodeActive ? NSColor.Black : NSColor.Gray);
            }
        }

        public nint numberOfDesigns {
            [Export (nameof (numberOfDesigns))]
            get { return (nint)designs.Count; }
        }

        private NSMutableArray designs = new NSMutableArray ();

        public NSArray Designs {
            [Export ("designModelArray")]
            get { return designs; }
        }

        private bool _isRoot;
        public bool isRoot {
            [Export (nameof(isRoot))]
            get {
                return _isRoot;
            }
            private set {
                _isRoot = value;
            }
        }

        public bool isEditable {
            [Export (nameof (isEditable))]
            get {
                return (modelType != DesignModelType.VariableRoot);
            }
        }

        [Export ("addObject:")]
        public void AddDesign (DesignModel design)
        {
            WillChangeValue ("designModelArray");
            WillChangeValue (nameof (isLeaf));
            designs.Add (design);
            DidChangeValue ("designModelArray");
            DidChangeValue (nameof (isLeaf));
        }

        [Export ("insertObject:inDesignModelArrayAtIndex:")]
        public void InsertDesign (DesignModel design, nint index)
        {
            WillChangeValue ("designModelArray");
            WillChangeValue (nameof (isLeaf));
            designs.Insert (design, index);
            DidChangeValue ("designModelArray");
            DidChangeValue (nameof (isLeaf));
        }

        [Export ("removeObjectFromDesignModelArrayAtIndex:")]
        public void RemoveDesign (nint index)
        {
            WillChangeValue ("designModelArray");
            WillChangeValue (nameof (isLeaf));
            designs.RemoveObject (index);
            DidChangeValue ("designModelArray");
            DidChangeValue (nameof (isLeaf));
        }

        public bool enableConvertToChoice {
            [Export (nameof (enableConvertToChoice))]
            get {
                return modelType == DesignModelType.Sequential;
            }
        }

        public bool enableConvertToSequential {
            [Export (nameof (enableConvertToSequential))]
            get {
                return modelType == DesignModelType.Choice;
            }
        }

        [Export ("setDesignModelArray:")]
        public void SetDesigns (NSMutableArray array)
        {
            WillChangeValue ("designModelArray");
            WillChangeValue (nameof (isLeaf));
            designs = array;
            DidChangeValue ("designModelArray");
            DidChangeValue (nameof (isLeaf));
        }

        public DesignModel (bool root, DesignModelType designModelType, string nm, string dt, bool ia, bool ipa)
        {
            modelType = designModelType;
            name = nm;
            details = dt;
            isRoot = root;
            isActive = ia;
            isParentActive = ipa;
        }

        public DesignModel (DesignModelType designNodeType, string name, string details, bool isActive, bool isParentActive)
            : this (false, designNodeType, name, details, isActive, isParentActive)
        {
        }

        public DesignModel (IntPtr handle) : base (handle)
        {
            isActive = true;
            isParentActive = true;
            modelType = DesignModelType.VariableRoot;
            name = "";
            details = "";
            isRoot = false;
        }
    }
}
