using System;
using System.Linq;
using Skrivmaskin.Core;

namespace Skrivmaskin.Core
{
    /// <summary>
    /// Skrivmaskin.
    /// </summary>
    public sealed class Skrivmaskin
    {
        readonly IRandomChooser randomChooser;
        readonly IVariableSubstituter variableSubstituter;
        readonly Project project;
        public Skrivmaskin (IRandomChooser randomChooser, IVariableSubstituter variableSubstituter, Project project)
        {
            this.randomChooser = randomChooser;
            this.variableSubstituter = variableSubstituter;
            this.project = project;
        }

        string Randomize<T, U> (Func<U, string> inner, T value) where T : Choice<U>
        {
            return inner (randomChooser.Choose (value));
        }

        string Concatenate<T, U> (Func<U, string> inner, T value, string separator) where T : Concat<U>
        {
            var s = "";
            foreach (var item in value.Elements) {
                if (String.IsNullOrEmpty (s)) s = inner (item);
                else s += separator + inner (item);
            }
            return s;
        }

        string Substitute (VariableForm variable)
        {
            return variableSubstituter.Substitute (variable);
        }

        string MakeBlock (IBlockOfText block)
        {
            var rawText = block as PhraseText;
            if (rawText != null) return rawText.Text;
            var variableSubstitution = block as PhraseVariableSubstitution;
            if (variableSubstitution != null) return Substitute (variableSubstitution.Variable);
            var wordChoice = block as PhraseWordChoice;
            if (wordChoice != null) return Randomize ((string s) => s, wordChoice);
            throw new InvalidOperationException ("Invalid block of text type " + block.GetType ());
        }

        string MakeSentenceChoice (SentenceChoice sc)
        {
            return Concatenate ((IBlockOfText bot) => MakeBlock(bot), sc, " ");
        }

        string MakeSentence (Sentence s)
        {
            return Randomize ((SentenceChoice sc) => MakeSentenceChoice(sc), s);
        }

        string MakeParagraphChoice (ParagraphChoice pc)
        {
            return Concatenate ((Sentence s) => MakeSentence (s), pc, "  ");
        }

        string MakeParagraph (Paragraph p)
        {
            return Randomize ((ParagraphChoice pc) => MakeParagraphChoice (pc), p);
        }

        /// <summary>
        /// Generate the text for the project.
        /// </summary>
        /// <returns>The project.</returns>
        public string Generate ()
        {
            //TODO Windows line endings for the Windows version?
            return Concatenate ((Paragraph p) => MakeParagraph (p), project, "\n\n");
        }
    }
}
