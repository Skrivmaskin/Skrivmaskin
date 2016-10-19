using System;
using System.Collections.Generic;
using AppKit;

namespace TextOn.Studio
{
    public class ManageConstraintsTableViewDataSource : NSTableViewDataSource
    {
        readonly ManageConstraintsDialogController controller;
        readonly IDictionary<string, string> values;

        public ManageConstraintsTableViewDataSource (ManageConstraintsDialogController controller, IDictionary<string,string> values)
        {
            this.controller = controller;
            this.values = values;
        }

        public override nint GetRowCount (NSTableView tableView)
        {
            return values.Count;
        }
    }
}
