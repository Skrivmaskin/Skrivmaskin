using System;
using System.Collections.Generic;
using AppKit;
using Foundation;
using TextOn.Nouns;

namespace TextOn.Studio
{
    public class SetNounValuesCollectionViewDataSource : NSCollectionViewDataSource
    {
        public NounSetValuesSession Session { get; set; } = null;

        public SetNounValuesCollectionViewDataSource ()
        {
        }

        public override nint GetNumberOfSections (NSCollectionView collectionView)
        {
            return 1;
        }

        public override NSCollectionViewItem GetItem (NSCollectionView collectionView, NSIndexPath indexPath)
        {
            // should all be like (0, a) where a is 0 - 4
            var item = collectionView.MakeItem (nameof (SetNounValuesItemView), indexPath) as SetNounValuesItemViewController;
            var indexes = indexPath.GetIndexes ();
            var lastIndex = indexes [indexes.Length - 1];
            var name = Session.GetName ((int)lastIndex);
            item.Activate (Session, name);
            return item;
        }

        public override nint GetNumberofItems (NSCollectionView collectionView, nint section)
        {
            return (Session == null) ? 0 : Session.Count;
        }
    }
}
