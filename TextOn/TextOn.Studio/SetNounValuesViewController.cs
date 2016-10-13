// This file has been autogenerated from a class added in the UI designer.

using System;
using System.Linq;
using Foundation;
using AppKit;
using System.Collections.Generic;
using TextOn.Compiler;
using CoreGraphics;
using TextOn.Nouns;

namespace TextOn.Studio
{
	public partial class SetNounValuesViewController : NSViewController
	{
		public SetNounValuesViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();

            this.datasource = new SetNounValuesCollectionViewDataSource ();
            SetNounValuesCollectionView.DataSource = this.datasource;
            SetNounValuesCollectionView.WantsLayer = true;
            SetNounValuesCollectionView.Layer.BackgroundColor = new CGColor (0.9f, 0.9f, 0.9f);
            ConfigureCollectionView ();
        }

        SetNounValuesCollectionViewDataSource datasource;

        /// <summary>
        /// When the view appears, the Template is locked, so we can make a session that lasts that long. We tear down on disappear.
        /// </summary>
        public override void ViewDidAppear ()
        {
            base.ViewDidAppear ();
            this.datasource = new SetNounValuesCollectionViewDataSource ();
			datasource.Session = parent.Template.Nouns.MakeSetValuesSession ();
            SetNounValuesCollectionView.DataSource = this.datasource;
            SetNounValuesCollectionView.WantsLayer = true;
            SetNounValuesCollectionView.Layer.BackgroundColor = new CGColor (0.9f, 0.9f, 0.9f);
            ConfigureCollectionView ();
        }

        /// <summary>
        /// Exposes the user's current values to the Generate page.
        /// </summary>
        /// <value>The noun values.</value>
        public IReadOnlyDictionary<string, string> NounValues {
            get {
                return datasource.Session.NounValues;
            }
        }

        public bool AllValuesAreSet {
            get {
                return datasource.Session.AllValuesAreSet;
            }
        }

        private CentralViewController parent = null;
        internal void SetControllerLinks (CentralViewController centralViewController)
        {
            Console.Error.WriteLine ("SetNounValues SetControllerLinks");

            this.parent = centralViewController;   
        }

        internal void SetCompiledTemplate ()
        {
            Console.Error.WriteLine ("SetNounValues SetCompiledTemplate");
        }

        private void ConfigureCollectionView ()
        {
            var flowLayout = new NSCollectionViewFlowLayout ();
            flowLayout.ItemSize = new CGSize (width: 250.0, height: 100.0);
            flowLayout.SectionInset = new NSEdgeInsets (top: (nfloat)20.0, left: (nfloat)10.0, bottom: (nfloat)20.0, right: (nfloat)10.0);
            flowLayout.MinimumInteritemSpacing = (nfloat)20.0;
            flowLayout.MinimumLineSpacing = (nfloat)20.0;
            SetNounValuesCollectionView.CollectionViewLayout = flowLayout;
            // 2
            View.WantsLayer = true;
            // 3
        }
	}
}