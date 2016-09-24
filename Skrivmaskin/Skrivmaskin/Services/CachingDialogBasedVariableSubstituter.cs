using System;
using System.Collections.Generic;
using Skrivmaskin.Compiler;
using Skrivmaskin.Interfaces;

namespace Skrivmaskin.Services
{
    /// <summary>
    /// An implementation of a variable substituter that asks a dialog service for the result when needed and caches the answer.
    /// </summary>
    public sealed class CachingDialogBasedVariableSubstituter : IVariableSubstituter
    {
        readonly IDialogService dialogService;
        readonly IDictionary<string, string> substitutions = new Dictionary<string, string> ();
        readonly IDictionary<string, ICompiledVariable> variableDefinitions;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Skrivmaskin.Services.CachingDialogBasedVariableSubstituter"/> class.
        /// </summary>
        /// <param name="dialogService">Dialog service.</param>
        /// <param name="variableDefinitions">Variable definitions.</param>
        public CachingDialogBasedVariableSubstituter (IDialogService dialogService, IDictionary<string, ICompiledVariable> variableDefinitions)
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
