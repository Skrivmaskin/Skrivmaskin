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
                case DesignNodeType.Text:
                    return "Text to be inserted into the output.";
                case DesignNodeType.Choice:
                    return "One of the subnodes will be randomly chosen for the output.";
                case DesignNodeType.Sequential:
                    return "All of the subnodes will be included sequentially in the output.";
                case DesignNodeType.Root:
                    return "Root node";
                case DesignNodeType.Variable:
                    return "A variable to be substituted into the output, using the [VARNAME] syntax.";
                case DesignNodeType.VariableForm:
                    return "A grammatical variant of the variable to be substituted into the output, using the [VARNAME|Variant] syntax.";
                case DesignNodeType.ParagraphBreak:
                    return "A paragraph break";
                case DesignNodeType.Comment:
                    return "A free form comment, excluded from the output.";
                }
                throw new ApplicationException ("Unknown nodde type " + nodeType);
            }
        }

        [Export ("detailsToolTip")]
        public string detailsToolTip {
            get {
                switch (nodeType) {
                case DesignNodeType.Text:
                case DesignNodeType.Comment:
                    return "Write text here.";
                case DesignNodeType.Choice:
                case DesignNodeType.Sequential:
                    return "Insert subnodes.";
                case DesignNodeType.Root:
                    return "Root node";
                case DesignNodeType.Variable:
                    return "Variable definition - used as a prompt for the user.";
                case DesignNodeType.VariableForm:
                    return "Suggestion for the value of this variant.";
                case DesignNodeType.ParagraphBreak:
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

        private DesignNodeType nodeType;
        public DesignNodeType NodeType {
            get { return nodeType; }
            set {
                WillChangeValue ("Icon");
                nodeType = value;
                DidChangeValue ("Icon");
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
            get { return NodeType == DesignNodeType.Choice || NodeType == DesignNodeType.Sequential || NodeType == DesignNodeType.Variable || NodeType == DesignNodeType.VariableForm; }
        }

        [Export ("isDetailsEditable")]
        public bool isDetailsEditable {
            get { return NodeType == DesignNodeType.Comment || NodeType == DesignNodeType.Text || NodeType == DesignNodeType.Variable || NodeType == DesignNodeType.VariableForm; }
        }

        [Export ("Icon")]
        public NSImage Icon {
            get {
                if (!isActive) {
                    return NSImage.ImageNamed (NSImageName.StatusUnavailable);
                } else {
                    switch (NodeType) {
                    case DesignNodeType.Root:
                        return NSImage.ImageNamed (NSImageName.Folder);
                    case DesignNodeType.Variable:
                    case DesignNodeType.VariableForm:
                        return NSImage.ImageNamed (NSImageName.UserGuest);
                    case DesignNodeType.Text:
                        return NSImage.ImageNamed (NSImageName.GoRightTemplate);
                    case DesignNodeType.Comment:
                        return NSImage.ImageNamed (NSImageName.UserGuest);
                    case DesignNodeType.Choice:
                        return NSImage.ImageNamed (NSImageName.StatusPartiallyAvailable);
                    case DesignNodeType.Sequential:
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

        public DesignModel (string name)
        {
            isActive = true;
            NodeType = DesignNodeType.Root;
            Name = name;
            Details = "";
        }

        public DesignModel (Variable variable)
        {
            isActive = true;
            NodeType = DesignNodeType.Variable;
            Name = variable.Name;
            Details = variable.Description;
        }

        public DesignModel (VariableForm form)
        {
            isActive = true;
            NodeType = DesignNodeType.VariableForm;
            Name = form.Name;
            Details = form.Suggestion;
        }

        public DesignModel (DesignNodeType designNodeType, string name, string details)
        {
            isActive = true;
            NodeType = designNodeType;
            Name = name;
            Details = details;
        }
    }
}
