// This file has been autogenerated from a class added in the UI designer.

using System;

using Foundation;
using AppKit;
using System.Collections.Generic;
using TextOn.Compiler;
using TextOn.Lexing;
using TextOn.Design;
using TextOn.Generation;
using TextOn.Version0;
using TextOn.Nouns;

namespace TextOn.Studio
{
	public partial class CentralViewController : NSTabViewController
	{
        private const int DesignViewTabViewItemIndex = 0;

		public CentralViewController (IntPtr handle) : base (handle)
		{
		}

        //TODO can't keep using magic numbers here - get these numbers in a sensible way or else!
        internal void NavigateAndSelectDesignNode (INode designNode)
        {
            if (designViewController.SelectDesignNode (designNode))
                SelectedTabViewItemIndex = DesignViewTabViewItemIndex;
        }

        internal void Search ()
        {
            SelectedTabViewItemIndex = DesignViewTabViewItemIndex;
            designViewController.PerformSegue (DesignViewDialogSegues.Search, designViewController);
        }

        #region Design and Compiled templates
        internal TextOnCompiler Compiler = new TextOnCompiler (new DefaultLexerSyntax ());
        internal TextOnTemplate Template { get; set; } = new TextOnTemplate (new NounProfile (), new SequentialNode ("Sentences", true, new List<INode> ()));
        private CompiledTemplate compiledTemplate = null;
        internal IReadOnlyDictionary<string, string> NounValues {
            get {
                return setVariablesViewController.NounValues;
            }
        }
        internal CompiledTemplate CompiledTemplate {
            get {
                return compiledTemplate;
            }
            set {
                compiledTemplate = value;
                if (setVariablesViewController != null) {
                    setVariablesViewController.SetCompiledTemplate ();
                }
            }
        }
        #endregion

        private void DiscoverControllers (NSViewController grandpa, NSViewController dad, NSViewController controller)
        {
            if (controller is DesignViewController) {
                designViewController = (DesignViewController)controller;
                designViewController.NounsSplitViewController = dad as NSSplitViewController;
                designViewController.PreviewSplitViewController = grandpa as NSSplitViewController;
            } else if (controller is DesignPreviewViewController)
                designPreviewViewController = (DesignPreviewViewController)controller;
            else if (controller is SetNounValuesViewController)
                setVariablesViewController = (SetNounValuesViewController)controller;
            else if (controller is ResultsViewController)
                resultsViewController = (ResultsViewController)controller;
            else if (controller is DefineNounsViewController)
                defineNounsViewController = (DefineNounsViewController)controller;
            else {
                foreach (var child in controller.ChildViewControllers) {
                    DiscoverControllers (dad, controller, child);
                }
            }
        }

        public bool AllValuesAreSet { get { return setVariablesViewController.AllValuesAreSet; } }

        internal void DidChangeCanGenerate ()
		{
            resultsViewController.DidChangeCanGenerate ();
		}

		internal void WillChangeCanGenerate ()
		{
            resultsViewController.WillChangeCanGenerate ();
		}

		DesignViewController designViewController = null;
        DefineNounsViewController defineNounsViewController = null;
        DesignPreviewViewController designPreviewViewController = null;
        SetNounValuesViewController setVariablesViewController = null;
        ResultsViewController resultsViewController = null;
        public override void AwakeFromNib ()
        {
            Console.Error.WriteLine ("Central AwakeFromNib");

            base.AwakeFromNib ();

            // This is really cheesy - recurse through to find the controllers with fairly intimate knowledge of
            // how they link together, then tell them all about me so that they can ask me to do work for them.
            DiscoverControllers (null, null, this);
            designViewController.SetControllerLinks (this);
            setVariablesViewController.SetControllerLinks (this);
            resultsViewController.SetControllerLinks (this);
            designPreviewViewController.SetControllerLinks (this);
            defineNounsViewController.SetControllerLinks (this);

            Template = new TextOnTemplate (new NounProfile (), new SequentialNode ("Sentences", true, new List<INode> ()));
            CreateTree (null, Template);
            defineNounsViewController.TemplateUpdated ();
        }

        /// <summary>
        /// Gets or sets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath { get; set; } = null;

        public void CreateTree (string path, TextOnTemplate template)
        {
            Console.Error.WriteLine ("Central CreateTree");

            Template = template;
            FilePath = path;
            designViewController.CreateTree ();
            defineNounsViewController.TemplateUpdated ();
        }

        public void GeneratePreview (PreviewPartialRouteChoiceNode[] partialRoute)
        {
            designPreviewViewController.UpdatePreview (partialRoute);
        }

        internal void MarkPreviewAsInvalid ()
        {
            designPreviewViewController.MarkPreviewAsInvalid ();
        }
    }
}
