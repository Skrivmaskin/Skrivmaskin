// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Linq;
using Foundation;
using AppKit;
using TextOn.Design;
using TextOn.Generation;
using TextOn.Services;
using System.Collections.Generic;
using TextOn.Compiler;

namespace TextOn.Studio
{
	public partial class DesignPreviewViewController : NSViewController
	{
		public DesignPreviewViewController (IntPtr handle) : base (handle)
		{
		}

        private string NodeString (INode node)
        {
            switch (node.Type) {
            case NodeType.Text:
                return ((TextNode)node).Text;
            case NodeType.ParagraphBreak:
                return "<pr/>";
            default:
                throw new ApplicationException ("Unexpected node type " + node.Type);
            }
        }

        public void UpdatePreview (INode node, CompiledTemplate compiledTemplate)
        {
            if (node == null) {
                nodes = new INode [0];
                TextView.SetValue ("", null);
            } else {
                nodes = generator.Generate (node);
                TextView.SetValue (string.Join ("\n", nodes.Select (NodeString)), compiledTemplate);
            }
        }

        private IEnumerable<INode> nodes;

        private readonly TextOnPreviewGenerator generator = new TextOnPreviewGenerator (new RandomChooser ());
	}
}