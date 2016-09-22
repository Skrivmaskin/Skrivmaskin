﻿using System;
using System.Linq;
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
        readonly IRandomChooser randomChooser;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Skrivmaskin.Core.Generation.SkrivmaskinGenerator"/> class.
        /// </summary>
        /// <remarks>
        /// This takes ownership of the random chooser and will manage its lifetime. Access to the last seed should be via the generator.
        /// </remarks>
        /// <param name="project">Project.</param>
        /// <param name="randomChooser">Random chooser.</param>
        public SkrivmaskinGenerator (CompiledProject project, IRandomChooser randomChooser)
        {
            this.project = project;
            this.randomChooser = randomChooser;
        }

        private string GenerateText (ICompiledNode node, IVariableSubstituter variableSubstituter)
        {
            var textNode = node as TextCompiledNode;
            if (textNode != null) return textNode.Text;
            var variableNode = node as VariableCompiledNode;
            if (variableNode != null) return variableSubstituter.Substitute (variableNode.VariableFullName);
            var choiceNode = node as ChoiceCompiledNode;
            if (choiceNode != null) return GenerateText (randomChooser.Choose (choiceNode.Choices), variableSubstituter);
            var sequentialNode = node as SequentialCompiledNode;
            if (sequentialNode != null) return string.Concat (sequentialNode.Sequential.Select ((n) => GenerateText (n, variableSubstituter)));
            throw new ApplicationException ("Unrecognised compiled node type " + node.GetType ());
        }

        /// <summary>
        /// Gets the last seed used in making random choices.
        /// </summary>
        /// <remarks>
        /// Exposed to the user for logging and reproducability purposes.
        /// </remarks>
        /// <value>The last seed.</value>
        public int? LastSeed { get { return randomChooser.LastSeed; } }

        /// <summary>
        /// Determines if it is possible for this generator to regenerate.
        /// </summary>
        /// <remarks>
        /// May be used by a user interface to allow the user to activate and hide controls that Regenerate.
        /// </remarks>
        /// <returns><c>true</c>, if regenerate was caned, <c>false</c> otherwise.</returns>
        public bool CanRegenerate ()
        {
            return LastSeed != null;
        }

        /// <summary>
        /// Generate the text for a given set of variable substitutions.
        /// </summary>
        /// <param name="variableSubstituer">Variable substituer.</param>
        public string Generate (IVariableSubstituter variableSubstituer)
        {
            randomChooser.Begin ();
            var result = GenerateText (project.Definition, variableSubstituer);
            randomChooser.End ();
            return result;
        }

        /// <summary>
        /// Generate the text for a given set of variable substitutions, using the same random seed as before to get the same variable substitutions.
        /// </summary>
        /// <param name="variableSubstituter">Variable substituter.</param>
        public string Regenerate (IVariableSubstituter variableSubstituter)
        {
            var lastSeed = LastSeed;
            if (lastSeed == null) throw new ApplicationException ("Unable to regenerate when we haven't run before");
            randomChooser.BeginWithSeed (lastSeed.Value);
            var result = GenerateText (project.Definition, variableSubstituter);
            randomChooser.End ();
            return result;
        }
    
        /// <summary>
        /// Generate the text for a given set of variable substitutions, using a given random seed.
        /// </summary>
        /// <param name="variableSubstituter">Variable substituter.</param>
        public string GenerateWithSeed (int seed, IVariableSubstituter variableSubstituter)
        {
            randomChooser.BeginWithSeed (seed);
            var result = GenerateText (project.Definition, variableSubstituter);
            randomChooser.End ();
            return result;
        }
}
}
