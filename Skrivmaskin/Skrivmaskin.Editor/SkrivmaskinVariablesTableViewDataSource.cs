using System;
using System.Linq;
using System.Collections.Generic;
using AppKit;
using CoreGraphics;
using Foundation;
using Skrivmaskin.Compiler;

namespace Skrivmaskin.Editor
{
    // Data sources walk a given data source and respond to questions from AppKit to generate
    // the data used in your Delegate. In this example, we walk a simple tree.
    public class SkrivmaskinVariablesTableViewDataSource : NSTableViewDataSource
    {
        public List<ICompiledVariable> Variables = new List<ICompiledVariable> ();
        public Dictionary<string, string> VariableValues = new Dictionary<string, string> ();

        public SkrivmaskinVariablesTableViewDataSource (IEnumerable<ICompiledVariable> variables)
        {
            foreach (var variable in variables) {
                Variables.Add (variable);
                VariableValues.Add (variable.FullName, variable.Suggestion);
            }
        }

        public override nint GetRowCount (NSTableView tableView)
        {
            return Variables.Count;
        }
    }
}
