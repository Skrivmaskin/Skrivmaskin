// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using AppKit;

namespace Skrivmaskin.Editor
{
	public partial class DesignViewController : NSViewController
    {
        public Node Node { get; private set; }
        private Node lastCompiledNode { get; set; }

        public DesignViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
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
            SkrivmaskinOutlineView.DataSource = datasource;
            SkrivmaskinOutlineView.Delegate = new SkrivmaskinOutlineViewDelegate (datasource);
        }

        //TODO farm this out to threads??
        private void Compile ()
        {
            if (lastCompiledNode != Node) {
                
            }
        }
	}
}
