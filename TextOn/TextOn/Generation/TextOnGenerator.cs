using System;
using System.Collections.Generic;
using System.Linq;
using TextOn.Compiler;
using TextOn.Interfaces;

namespace TextOn.Generation
{
    /// <summary>
    /// TextOn generator. Takes a compiled project, variable replacements and a random number generator and produces output text.
    /// </summary>
    public sealed class TextOnGenerator
    {
        readonly IRandomChooser randomChooser;
        readonly IGeneratorConfig generatorConfig;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Generation.TextOnGenerator"/> class.
        /// </summary>
        /// <remarks>
        /// This takes ownership of the random chooser and will manage its lifetime. Access to the last seed should be via the generator.
        /// </remarks>
        /// <param name="randomChooser">Random chooser.</param>
        public TextOnGenerator (IRandomChooser randomChooser, IGeneratorConfig generatorConfig)
        {
            this.randomChooser = randomChooser;
            this.generatorConfig = generatorConfig;
        }

        private IEnumerable<AnnotatedText> GenerateText (ICompiledNode node, IVariableSubstituter variableSubstituter)
        {
            switch (node.Type) {
            case CompiledNodeType.Text:
                yield return new AnnotatedText (node.Location, (node as TextCompiledNode).Text);
                break;
            case CompiledNodeType.Variable:
                yield return new AnnotatedText (node.Location, variableSubstituter.Substitute ((node as VariableCompiledNode).VariableFullName));
                break;
            case CompiledNodeType.Sequential:
                foreach (var n in (node as SequentialCompiledNode).Sequential) {
                    foreach (var text in GenerateText (n, variableSubstituter)) {
                        yield return text;
                    }
                }
                break;
            case CompiledNodeType.Choice:
                var choices = (node as ChoiceCompiledNode).Choices;
                if (choices.Count == 0) yield return AnnotatedText.Blank;
                else {
                    var n = randomChooser.Choose (choices.Count);
                    var choice = choices [n];
                    var li = GenerateText (choice, variableSubstituter);
                    foreach (var text in li) {
                        yield return text;
                    }
                }
                break;
            case CompiledNodeType.SentenceBreak:
                yield return new AnnotatedText (null, generatorConfig.Spacing);
                break;
            case CompiledNodeType.ParagraphBreak:
                yield return new AnnotatedText (null, generatorConfig.ParagraphBreak);
                break;
            case CompiledNodeType.Blank:
                yield return AnnotatedText.Blank;
                break;
            case CompiledNodeType.Success:
                foreach (var text in GenerateText ((node as SuccessCompiledNode).Node, variableSubstituter)) {
                    yield return text;
                }
                break;
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
        /// Is the template missing a noun definition?
        /// </summary>
        /// <returns><c>true</c>, if missing required noun definitions was ised, <c>false</c> otherwise.</returns>
        /// <param name="template">Template.</param>
        public bool IsMissingRequiredNounDefinitions (CompiledTemplate template)
        {
            var definedNouns = new HashSet<string> (template.Nouns.GetAllNouns ().Select ((n) => n.Name));
            return template.Definition.RequiredVariables.Where ((n) => !definedNouns.Contains (n)).Count () != 0;
        }

        /// <summary>
        /// Determines if it is possible for this generator to regenerate.
        /// </summary>
        /// <remarks>
        /// May be used by a user interface to allow the user to activate and hide controls that Regenerate.
        /// </remarks>
        /// <returns><c>true</c>, if regenerate was caned, <c>false</c> otherwise.</returns>
        public bool CanRegenerate (CompiledTemplate template)
        {
            return LastSeed != null && !template.Definition.HasErrors && !IsMissingRequiredNounDefinitions(template);
        }

        /// <summary>
        /// Determines if it is possible for this generator to generate.
        /// </summary>
        /// <remarks>
        /// May be used by a user interface to allow the user to activate and hide controls that Generate.
        /// </remarks>
        /// <returns><c>true</c>, if regenerate was caned, <c>false</c> otherwise.</returns>
        public bool CanGenerate (CompiledTemplate template)
        {
            return !template.Definition.HasErrors && !IsMissingRequiredNounDefinitions (template);
        }

        /// <summary>
        /// Generate the text for a given set of variable substitutions.
        /// </summary>
        /// <param name="variableSubstituer">Variable substituer.</param>
        public AnnotatedOutput Generate (CompiledTemplate project, IVariableSubstituter variableSubstituer)
        {
            randomChooser.Begin ();
            var result = GenerateText (project.Definition, variableSubstituer).ToList ();
            randomChooser.End ();
            return new AnnotatedOutput (result);
        }

        /// <summary>
        /// Generate the text for a given set of variable substitutions, using the same random seed as before to get the same choices.
        /// </summary>
        /// <param name="variableSubstituter">Variable substituter.</param>
        public AnnotatedOutput Regenerate (CompiledTemplate project, IVariableSubstituter variableSubstituter)
        {
            var lastSeed = LastSeed;
            if (lastSeed == null) throw new ApplicationException ("Unable to regenerate when we haven't run before");
            randomChooser.BeginWithSeed (lastSeed.Value);
            var result = GenerateText (project.Definition, variableSubstituter).ToList ();
            randomChooser.End ();
            return new AnnotatedOutput (result);
        }

        /// <summary>
        /// Generate the text for a given set of variable substitutions, using a given random seed.
        /// </summary>
        /// <param name="variableSubstituter">Variable substituter.</param>
        public AnnotatedOutput GenerateWithSeed (CompiledTemplate project, IVariableSubstituter variableSubstituter, int seed)
        {
            randomChooser.BeginWithSeed (seed);
            var result = GenerateText (project.Definition, variableSubstituter).ToList ();
            randomChooser.End ();
            return new AnnotatedOutput (result);
        }
    }
}
