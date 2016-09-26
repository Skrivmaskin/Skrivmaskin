using System;
using AppKit;
using Foundation;
using Skrivmaskin.Design;

namespace Skrivmaskin.Editor
{
    [Register ("DesignModel")]
    public class DesignModel : NSObject
    {
        private readonly DesignViewController controller;

        private string name = "";
        [Export ("Name")]
        public string Name {
            get { return name; }
            set {
                WillChangeValue ("Name");
                name = value;
                DidChangeValue ("Name");
                controller.DocumentEditedAction ();
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
                controller.DocumentEditedAction ();
            }
        }

        public DesignModel Parent { get; set; } = null;

        private DesignModelType nodeType;
        public DesignModelType NodeType {
            get { return nodeType; }
            set {
                WillChangeValue ("Icon");
                WillChangeValue ("isNameEditable");
                WillChangeValue ("isDetailsEditable");
                nodeType = value;
                DidChangeValue ("Icon");
                DidChangeValue ("isNameEditable");
                DidChangeValue ("isDetailsEditable");
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

        [Export ("isLeaf")]
        public bool isLeaf {
            get { return designs.Count == 0; }
        }

        [Export ("isNameEditable")]
        public bool isNameEditable {
            get { return NodeType == DesignModelType.Choice || NodeType == DesignModelType.Sequential || NodeType == DesignModelType.Variable || NodeType == DesignModelType.VariableForm; }
        }

        [Export ("isDetailsEditable")]
        public bool isDetailsEditable {
            get { return NodeType == DesignModelType.Text || NodeType == DesignModelType.Variable || NodeType == DesignModelType.VariableForm; }
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

        [Export ("addObject:")]
        public void AddDesign (DesignModel design)
        {
            WillChangeValue ("designModelArray");
            WillChangeValue ("isLeaf");
            designs.Add (design);
            design.Parent = this;
            DidChangeValue ("designModelArray");
            DidChangeValue ("isLeaf");
        }

        [Export ("insertObject:inDesignModelArrayAtIndex:")]
        public void InsertDesign (DesignModel design, nint index)
        {
            WillChangeValue ("designModelArray");
            WillChangeValue ("isLeaf");
            designs.Insert (design, index);
            design.Parent = this;
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

        [Export ("setDesignModelArray:")]
        public void SetDesigns (NSMutableArray array)
        {
            WillChangeValue ("designModelArray");
            WillChangeValue ("isLeaf");
            designs = array;
            DidChangeValue ("designModelArray");
            DidChangeValue ("isLeaf");
        }

        public DesignModel (DesignViewController controller, string name)
        {
            this.controller = controller;
            isActive = true;
            NodeType = DesignModelType.VariableRoot;
            Name = name;
            Details = "";
        }

        public DesignModel (DesignViewController controller, Variable variable)
        {
            this.controller = controller;
            isActive = true;
            NodeType = DesignModelType.Variable;
            Name = variable.Name;
            Details = variable.Description;
        }

        public DesignModel (DesignViewController controller, VariableForm form)
        {
            this.controller = controller;
            isActive = true;
            NodeType = DesignModelType.VariableForm;
            Name = form.Name;
            Details = form.Suggestion;
        }

        public DesignModel (DesignViewController controller, DesignModelType designNodeType, string name, string details)
        {
            this.controller = controller;
            isActive = true;
            NodeType = designNodeType;
            Name = name;
            Details = details;
        }
    }
}
