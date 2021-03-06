using System;
using AppKit;
using Foundation;
using TextOn.Design;

namespace TextOn.Studio
{
    [Register ("DesignModel")]
    public class DesignModel : NSObject
    {
        private string _name = "";
        public string name {
            [Export (nameof (name))]
            get { return _name; }
            set {
                WillChangeValue (nameof (name));
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
                WillChangeValue (nameof (details));
                _details = value;
                DidChangeValue (nameof (details));
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

        private bool _canMoveUp = false;
        public bool canMoveUp {
            [Export (nameof (canMoveUp))]
            get {
                return _canMoveUp;
            }
            set {
                WillChangeValue (nameof (canMoveUp));
                _canMoveUp = value;
                DidChangeValue (nameof (canMoveUp));
            }
        }

        private bool _canMoveDown = false;
        public bool canMoveDown {
            [Export (nameof (canMoveDown))]
            get {
                return _canMoveDown;
            }
            set {
                WillChangeValue (nameof (canMoveDown));
                _canMoveDown = value;
                DidChangeValue (nameof (canMoveDown));
            }
        }

        private bool _isActive = false;
        public bool isActive {
            [Export (nameof (isActive))]
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
            [Export (nameof (isLeaf))]
            get { return designs.Count == 0; }
        }

        public bool enableAdd {
            [Export (nameof (enableAdd))]
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

        public NSImage icon {
            [Export (nameof (icon))]
            get {
                switch (modelType) {
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
            [Export (nameof (isRoot))]
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
                return true;
            }
        }

        public bool isDeletable {
            [Export (nameof (isDeletable))]
            get {
                return (!isRoot);
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
            if (designs.Count == 1) {
                design.canMoveUp = false;
                design.canMoveDown = false;
            } else {
                design.canMoveUp = true;
                design.canMoveDown = true;
                var f = designs.GetItem<DesignModel> (0);
                f.canMoveUp = false;
                f.canMoveDown = true;
                var l = designs.GetItem<DesignModel> (designs.Count - 1);
                l.canMoveUp = true;
                l.canMoveDown = false;
                var sf = designs.GetItem<DesignModel> (1);
                sf.canMoveUp = true;
                var sl = designs.GetItem<DesignModel> (designs.Count - 2);
                sl.canMoveDown = true;
            }
        }

        [Export ("insertObject:inDesignModelArrayAtIndex:")]
        public void InsertDesign (DesignModel design, nint index)
        {
            WillChangeValue ("designModelArray");
            WillChangeValue (nameof (isLeaf));
            designs.Insert (design, index);
            DidChangeValue ("designModelArray");
            DidChangeValue (nameof (isLeaf));
            if (designs.Count == 1) {
                design.canMoveUp = false;
                design.canMoveDown = false;
            } else {
                design.canMoveUp = true;
                design.canMoveDown = true;
                var f = designs.GetItem<DesignModel> (0);
                f.canMoveUp = false;
                f.canMoveDown = true;
                var l = designs.GetItem<DesignModel> (designs.Count - 1);
                l.canMoveUp = true;
                l.canMoveDown = false;
                var sf = designs.GetItem<DesignModel> (1);
                sf.canMoveUp = true;
                var sl = designs.GetItem<DesignModel> (designs.Count - 2);
                sl.canMoveDown = true;
            }
        }

        [Export ("removeObjectFromDesignModelArrayAtIndex:")]
        public void RemoveDesign (nint index)
        {
            WillChangeValue ("designModelArray");
            WillChangeValue (nameof (isLeaf));
            designs.RemoveObject (index);
            DidChangeValue ("designModelArray");
            DidChangeValue (nameof (isLeaf));
            if (designs.Count > 0) {
                designs.GetItem<DesignModel> (0).canMoveUp = false;
                designs.GetItem<DesignModel> (designs.Count - 1).canMoveDown = false;
            }
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

        public bool enableEncapsulate {
            [Export (nameof (enableEncapsulate))]
            get {
                switch (modelType) {
                case DesignModelType.Text:
                    return (!isRoot);
                default:
                    return true;
                }
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
            modelType = DesignModelType.Sequential;
            name = "";
            details = "";
            isRoot = false;
        }
    }
}
