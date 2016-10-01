// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using AppKit;

namespace Skrivmaskin.Studio
{
	public partial class CentralViewController : NSTabViewController
	{
		public CentralViewController (IntPtr handle) : base (handle)
		{
		}

        public override void AwakeFromNib ()
        {
            base.AwakeFromNib ();

            DesignViewController dvc = null;
            SetVariablesViewController svvc = null;
            ResultsViewController rvc = null;
            foreach (var child in ChildViewControllers) {
                if (child is DesignViewController)
                    dvc = (DesignViewController)child;
                else {
                    foreach (var subchild in child.ChildViewControllers) {
                        if (subchild is SetVariablesViewController)
                            svvc = (SetVariablesViewController)subchild;
                        else if (subchild is ResultsViewController)
                            rvc = (ResultsViewController)subchild;
                    }
                }
            }
            dvc.SetControllerLinks (svvc, rvc);
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; set; } = null;
    }
}