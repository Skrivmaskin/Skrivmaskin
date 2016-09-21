using System;
using System.Collections.Generic;
using Skrivmaskin.Core.Compiled;
using Skrivmaskin.Core.Interfaces;

namespace Skrivmaskin.Core.Services
{
    public sealed class CachingDialogBasedVariableSubstituter : IVariableSubstituter
    {
        readonly IDialogService dialogService;
        readonly IDictionary<string, string> substitutions = new Dictionary<string, string> ();
        readonly IDictionary<string, CompiledVariable> variableDefinitions;
        public CachingDialogBasedVariableSubstituter (IDialogService dialogService, IDictionary<string, CompiledVariable> variableDefinitions)
        {
            this.dialogService = dialogService;
            this.variableDefinitions = variableDefinitions;
        }

        /// <summary>
        /// Substitute the specified variable.
        /// </summary>
        /// <param name="variableFullName">Variable full name.</param>
        public string Substitute (string variableFullName)
        {
            if (substitutions.ContainsKey (variableFullName)) return substitutions [variableFullName];
            var variable = variableDefinitions [variableFullName];
            var substitution = dialogService.GetAnswer (variable.Description + ((String.IsNullOrEmpty (variable.FormName)) ? "" : " (" + variable.FormName + ")"), variable.Suggestion);
            substitutions.Add (variableFullName, substitution);
            return substitution;
        }
    }
}
