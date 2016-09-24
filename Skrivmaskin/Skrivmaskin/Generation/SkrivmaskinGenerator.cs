using System;
using System.Linq;
using Skrivmaskin.Compiler;
using Skrivmaskin.Interfaces;

namespace Skrivmaskin.Generation
{
    /// <summary>
    /// Skrivmaskin generator. Takes a compiled project, variable replacements and a random number generator and produces output text.
    /// </summary>
    public sealed class SkrivmaskinGenerator
    {
        readonly IRandomChooser randomChooser;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Skrivmaskin.Generation.SkrivmaskinGenerator"/> class.
        /// </summary>
        /// <remarks>
        /// This takes ownership of the random chooser and will manage its lifetime. Access to the last seed should be via the generator.
        /// </remarks>
        /// <param name="randomChooser">Random chooser.</param>
        public SkrivmaskinGenerator (IRandomChooser randomChooser)
        {
            this.randomChooser = randomChooser;
        }

        private string GenerateText (ICompiledNode node, IVariableSubstituter variableSubstituter)
        {
            switch (node.Type) {
            case CompiledNodeType.Text:
                return (node as TextCompiledNode).Text;
            case CompiledNodeType.Variable:
                return variableSubstituter.Substitute ((node as VariableCompiledNode).VariableFullName);
            case CompiledNodeType.Sequential:
                return string.Concat ((node as SequentialCompiledNode).Sequential.Select ((n) => GenerateText (n, variableSubstituter)));
            case CompiledNodeType.Choice:
                return GenerateText (randomChooser.Choose ((node as ChoiceCompiledNode).Choices), variableSubstituter);
            default:
                throw new ApplicationException ("Unrecognised to generate text when there were compiler errors " + node.GetType ());
            }
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
        public bool CanRegenerate (CompiledProject project)
        {
            return LastSeed != null && !project.Definition.HasErrors;
        }

        /// <summary>
        /// Determines if it is possible for this generator to generate.
        /// </summary>
        /// <remarks>
        /// May be used by a user interface to allow the user to activate and hide controls that Generate.
        /// </remarks>
        /// <returns><c>true</c>, if regenerate was caned, <c>false</c> otherwise.</returns>
        public bool CanGenerate (CompiledProject project)
        {
            return !project.Definition.HasErrors;
        }

        /// <summary>
        /// Generate the text for a given set of variable substitutions.
        /// </summary>
        /// <param name="variableSubstituer">Variable substituer.</param>
        public string Generate (CompiledProject project, IVariableSubstituter variableSubstituer)
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
        public string Regenerate (CompiledProject project, IVariableSubstituter variableSubstituter)
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
        public string GenerateWithSeed (CompiledProject project, IVariableSubstituter variableSubstituter, int seed)
        {
            randomChooser.BeginWithSeed (seed);
            var result = GenerateText (project.Definition, variableSubstituter);
            randomChooser.End ();
            return result;
        }
    }
}
