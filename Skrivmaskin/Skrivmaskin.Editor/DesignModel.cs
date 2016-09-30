using System;
using AppKit;
using Foundation;
using Skrivmaskin.Design;

namespace Skrivmaskin.Editor
{
    [Register ("DesignModel")]
    public class DesignModel : NSObject
    {
        private string name = "";
        [Export ("Name")]
        public string Name {
            get { return name; }
            set {
                WillChangeValue ("Name");
                name = value;
                DidChangeValue ("Name");
            }
        }

        [Export ("nameToolTip")]
        public string nameToolTip {
            get {
                switch (nodeType) {
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
                throw new ApplicationException ("Unknown nodde type " + nodeType);
            }
        }

        [Export ("detailsToolTip")]
        public string detailsToolTip {
            get {
                switch (nodeType) {
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
                throw new ApplicationException ("Unknown nodde type " + nodeType);
            }
        }

        private string details = "";
        [Export ("Details")]
        public string Details {
            get { return details; }
            set {
                WillChangeValue ("Details");
                details = value;
                DidChangeValue ("Details");
            }
        }

        private DesignModelType nodeType;
        public DesignModelType NodeType {
            get { return nodeType; }
            set {
                WillChangeValue ("Icon");
                WillChangeValue (nameof(enableConvertToChoice));
                WillChangeValue (nameof(enableConvertToSequential));
                nodeType = value;
                DidChangeValue ("Icon");
                DidChangeValue (nameof (enableConvertToChoice));
                DidChangeValue (nameof (enableConvertToSequential));
            }
        }

        private bool isActive = false;
        [Export ("IsActive")]
        public bool IsActive {
            get { return isActive; }
            set {
                WillChangeValue ("Icon");
                WillChangeValue ("IsActive");
                isActive = value;
                DidChangeValue ("Icon");
                DidChangeValue ("IsActive");
            }
        }

        public bool isLeaf {
            [Export ("isLeaf")]
            get { return designs.Count == 0; }
        }

        public bool enableAdd {
            [Export ("enableAdd")]
            get {
                switch (NodeType) {
                case DesignModelType.Choice:
                case DesignModelType.Sequential:
                    return true;
                default:
                    return false;
                }
            }
        }

        public bool enableAddVariant {
            [Export ("enableAddVariant")]
            get {
                return (NodeType == DesignModelType.Variable);
            }
        }

        [Export ("Icon")]
        public NSImage Icon {
            get {
                if (!isActive) {
                    return NSImage.ImageNamed (NSImageName.StatusUnavailable);
                } else {
                    switch (NodeType) {
                    case DesignModelType.VariableRoot:
                        return NSImage.ImageNamed (NSImageName.Folder);
                    case DesignModelType.Variable:
                    case DesignModelType.VariableForm:
                        return NSImage.ImageNamed (NSImageName.UserGuest);
                    case DesignModelType.Text:
                        return NSImage.ImageNamed (NSImageName.GoRightTemplate);
                    case DesignModelType.Choice:
                        return NSImage.ImageNamed (NSImageName.StatusPartiallyAvailable);
                    case DesignModelType.Sequential:
                        return NSImage.ImageNamed (NSImageName.StatusNone);
                    case DesignModelType.ParagraphBreak:
                        return NSImage.ImageNamed (NSImageName.QuickLookTemplate);
                    default:
                        return null;
                    }
                }
            }
        }

        [Export ("NumberOfDesigns")]
        public nint NumberOfDesigns {
            get { return (nint)designs.Count; }
        }

        private NSMutableArray designs = new NSMutableArray ();

        [Export ("designModelArray")]
        public NSArray Designs {
            get { return designs; }
        }

        private bool _isRoot;
        public bool isRoot {
            [Export ("isRoot")]
            get {
                return _isRoot;
            }
        }

        public bool isEditable {
            [Export ("isEditable")]
            get {
                return (NodeType != DesignModelType.VariableRoot);
            }
        }

        [Export ("addObject:")]
        public void AddDesign (DesignModel design)
        {
            WillChangeValue ("designModelArray");
            WillChangeValue ("isLeaf");
            designs.Add (design);
            DidChangeValue ("designModelArray");
            DidChangeValue ("isLeaf");
        }

        [Export ("insertObject:inDesignModelArrayAtIndex:")]
        public void InsertDesign (DesignModel design, nint index)
        {
            WillChangeValue ("designModelArray");
            WillChangeValue ("isLeaf");
            designs.Insert (design, index);
            DidChangeValue ("designModelArray");
            DidChangeValue ("isLeaf");
        }

        [Export ("removeObjectFromDesignModelArrayAtIndex:")]
        public void RemoveDesign (nint index)
        {
            WillChangeValue ("designModelArray");
            WillChangeValue ("isLeaf");
            designs.RemoveObject (index);
            DidChangeValue ("designModelArray");
            DidChangeValue ("isLeaf");
        }

        public bool enableConvertToChoice {
            [Export ("enableConvertToChoice")]
            get {
                return NodeType == DesignModelType.Sequential;
            }
        }

        public bool enableConvertToSequential {
            [Export ("enableConvertToSequential")]
            get {
                return NodeType == DesignModelType.Choice;
            }
        }

        [Export ("setDesignModelArray:")]
        public void SetDesigns (NSMutableArray array)
        {
            WillChangeValue ("designModelArray");
            WillChangeValue ("isLeaf");
            designs = array;
            DidChangeValue ("designModelArray");
            DidChangeValue ("isLeaf");
        }

        public DesignModel (string name)
        {
            isActive = true;
            NodeType = DesignModelType.VariableRoot;
            Name = name;
            Details = "";
            _isRoot = true;
        }

        public DesignModel (Variable variable)
        {
            isActive = true;
            NodeType = DesignModelType.Variable;
            Name = variable.Name;
            Details = variable.Description;
        }

        public DesignModel (VariableForm form)
        {
            isActive = true;
            NodeType = DesignModelType.VariableForm;
            Name = form.Name;
            Details = form.Suggestion;
        }

        public DesignModel (bool root, DesignModelType designNodeType, string name, string details)
        {
            isActive = true;
            NodeType = designNodeType;
            Name = name;
            Details = details;
            _isRoot = root;
        }

        public DesignModel (DesignModelType designNodeType, string name, string details)
            : this (false, designNodeType, name, details)
        {
        }
    }
}
