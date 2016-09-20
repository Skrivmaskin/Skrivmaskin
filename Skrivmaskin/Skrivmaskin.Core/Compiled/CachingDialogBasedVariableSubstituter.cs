using System;
using System.Collections.Generic;
namespace Skrivmaskin.Core.Compiled
{
    public sealed class CachingDialogBasedVariableSubstituter : IVariableSubstituter
    {
        readonly IDialogService dialogService;
        readonly IDictionary<VariableForm, string> substitutions = new Dictionary<VariableForm, string> ();
        public CachingDialogBasedVariableSubstituter (IDialogService dialogService)
        {
            this.dialogService = dialogService;
        }

        /// <summary>
        /// Substitute the specified variable.
        /// </summary>
        /// <param name="variable">Variable.</param>
        public string Substitute (VariableForm variable)
        {
            if (substitutions.ContainsKey (variable)) return substitutions [variable];
            var substitution = dialogService.GetAnswer (variable.Variable.Description + ((String.IsNullOrEmpty (variable.Name)) ? "" : " (" + variable.Name + ")"), variable.Variable.Suggestion);
            substitutions.Add (variable, substitution);
            return substitution;
        }
    }
}
