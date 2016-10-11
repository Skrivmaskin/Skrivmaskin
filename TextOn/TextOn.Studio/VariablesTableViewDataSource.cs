using System;
using System.Linq;
using System.Collections.Generic;
using AppKit;
using CoreGraphics;
using Foundation;
using TextOn.Compiler;
using TextOn.Nouns;

namespace TextOn.Studio
{
    // Data sources walk a given data source and respond to questions from AppKit to generate
    // the data used in your Delegate. In this example, we walk a simple tree.
    public class VariablesTableViewDataSource : NSTableViewDataSource
    {
        public List<Tuple<string, string>> Variables = new List<Tuple<string, string>> ();
        public Dictionary<string, string> VariableValues = new Dictionary<string, string> ();

        public VariablesTableViewDataSource (NounProfile variables)
        {
            foreach (var variable in variables.GetAllNouns ()) {
                Variables.Add (new Tuple<string, string> (variable.Name, variable.Description));
                VariableValues.Add (variable.Name, "");
            }
        }

        public override nint GetRowCount (NSTableView tableView)
        {
            return Variables.Count;
        }
    }
}
