using System;
using System.Collections.Generic;
using TextOn.Interfaces;

namespace TextOn.Services
{
    public class DictionaryBackedVariableSubstituter : IVariableSubstituter
    {
        readonly IReadOnlyDictionary<string, string> values;
        public DictionaryBackedVariableSubstituter (IReadOnlyDictionary<string,string> values)
        {
            this.values = values;
        }

        public string Substitute (string variableFullName)
        {
            return values [variableFullName];
        }
    }
}
