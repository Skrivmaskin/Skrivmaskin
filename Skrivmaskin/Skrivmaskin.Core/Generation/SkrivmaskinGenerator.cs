using System;
using Skrivmaskin.Core.Compiled;
using Skrivmaskin.Core.Interfaces;

namespace Skrivmaskin.Core.Generation
{
    /// <summary>
    /// Skrivmaskin generator. Takes a compiled project, variable replacements and a random number generator and produces output text.
    /// </summary>
    public sealed class SkrivmaskinGenerator
    {
        readonly CompiledProject project;
        readonly IVariableSubstituter variableSubstituer;
        readonly IRandomChooser randomChooser;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Skrivmaskin.Core.Generation.SkrivmaskinGenerator"/> class.
        /// </summary>
        /// <param name="project">Project.</param>
        /// <param name="variableSubstituter">Performs the variable substitutions based on user input.</param>
        /// <param name="randomChooser">Random chooser.</param>
        public SkrivmaskinGenerator (CompiledProject project, IVariableSubstituter variableSubstituter, IRandomChooser randomChooser)
        {
            this.project = project;
            this.variableSubstituer = variableSubstituter;
            this.randomChooser = randomChooser;
        }


    }
}
