using System;
using AppKit;
using TextOn.Nouns;

namespace TextOn.Studio
{
    public class SetNounValuesTableViewDataSource : NSTableViewDataSource
    {
        internal readonly NounSetValuesSession Session;

        public SetNounValuesTableViewDataSource (NounSetValuesSession nounProfileSession)
        {
            Session = nounProfileSession;
        }

        public override nint GetRowCount (NSTableView tableView)
        {
            return Session.Count;
        }
    }
}
