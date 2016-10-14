using System;
using AppKit;
using TextOn.Nouns;

namespace TextOn.Studio
{
    public class DefineNounsTableViewDataSource : NSTableViewDataSource
    {
        internal readonly NounProfile NounProfile;
        public DefineNounsTableViewDataSource (NounProfile nounProfile)
        {
            this.NounProfile = nounProfile;
        }

        public override nint GetRowCount (NSTableView tableView)
        {
            return NounProfile.Count;
        }
    }
}
