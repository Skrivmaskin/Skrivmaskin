using System;

using AppKit;
using CoreGraphics;
using Foundation;

namespace Skrivmaskin.Editor
{
    public partial class ViewController : NSViewController
    {
        public string FilePath { get; set; } = null;
        public Node Node { get; private set; }

        public ViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            SetNode (new Node ("", ""));
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

        internal void SetNode (Node node)
        {
            Node = node;
            var datasource = new SkrivmaskinOutlineViewDataSource (node);
            SkrivmaskinOutline.DataSource = datasource;
            SkrivmaskinOutline.Delegate = new SkrivmaskinOutlineViewDelegate (datasource);
        }
    }
}
