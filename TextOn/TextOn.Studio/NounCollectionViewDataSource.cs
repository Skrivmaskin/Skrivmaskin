using System;
using AppKit;
using Foundation;

namespace TextOn.Studio
{
    public class NounCollectionViewDataSource : NSCollectionViewDataSource
    {
        public NounCollectionViewDataSource ()
        {
        }

        public override nint GetNumberOfSections (NSCollectionView collectionView)
        {
            return 1;
        }

        public override NSCollectionViewItem GetItem (NSCollectionView collectionView, NSIndexPath indexPath)
        {
            // should all be like (0, a) where a is 0 - 4
            var item = collectionView.MakeItem (nameof(NounItemView), indexPath) as NounItemViewController;
            return item;
        }

        public override nint GetNumberofItems (NSCollectionView collectionView, nint section)
        {
            return 5;
        }
    }
}
