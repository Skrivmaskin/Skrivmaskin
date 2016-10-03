// This grammar is based on the ExpressionEvaluatorGrammar from Irony.Sample// Copyright (c) Roman Ivantsov
// Details at http://irony.codeplex.com
using Irony.Parsing;
using Skrivmaskin.Interfaces;

namespace Skrivmaskin.Lexing
{
    [Language ("Skrivmaskin", "1.0", "Basic grammar for single line Skrivmaskin input")]
    internal class SkrivmaskinGrammar : Grammar
    {
        public SkrivmaskinGrammar (ILexerSyntax lexerSyntax) : base()
        {
            this.GrammarComments = @"Basic grammar for single line Skrivmaskin input";

            // 1. Terminals
            var Text = new SkrivmaskinTextTerminal (nameof(SkrivmaskinParseNodes.Text), lexerSyntax, true);
            var Escape = new SkrivmaskinEscapeTerminal (nameof (SkrivmaskinParseNodes.Escape), lexerSyntax);
            var VarName = new SkrivmaskinTextTerminal (nameof (SkrivmaskinParseNodes.VarName), lexerSyntax, false);
            var VarForm = new SkrivmaskinTextTerminal (nameof (SkrivmaskinParseNodes.VarForm), lexerSyntax, false);

            // 2. Non-terminals
            var CompoundText = new NonTerminal (nameof (SkrivmaskinParseNodes.CompoundText));
            var Phrase = new NonTerminal (nameof (SkrivmaskinParseNodes.Phrase));
            var SimpleVariable = new NonTerminal (nameof (SkrivmaskinParseNodes.SimpleVariable));
            var CompoundVariable = new NonTerminal (nameof (SkrivmaskinParseNodes.CompoundVariable));
            var Variable = new NonTerminal (nameof (SkrivmaskinParseNodes.Variable));
            var Sentence = new NonTerminal (nameof (SkrivmaskinParseNodes.Sentence));
            var SimpleChoice = new NonTerminal (nameof (SkrivmaskinParseNodes.SimpleChoice));
            var MultiChoice = new NonTerminal (nameof (SkrivmaskinParseNodes.MultiChoice));
            var Choice = new NonTerminal (nameof (SkrivmaskinParseNodes.Choice));
            var OrOp = new NonTerminal (nameof (SkrivmaskinParseNodes.OrOp));
            var Anything = new NonTerminal (nameof (SkrivmaskinParseNodes.Anything));

            // 3. BNF rules
            CompoundText.Rule = Text | Escape;
            Phrase.Rule = MakePlusRule (Phrase, CompoundText);
            SimpleVariable.Rule = lexerSyntax.VariableStartDelimiter.ToString () + VarName + lexerSyntax.VariableEndDelimiter.ToString ();
            CompoundVariable.Rule = lexerSyntax.VariableStartDelimiter.ToString () + VarName + lexerSyntax.VariableFormDelimiter.ToString() + VarForm + lexerSyntax.VariableEndDelimiter.ToString ();
            Variable.Rule = SimpleVariable | CompoundVariable;

            SimpleChoice.Rule = MakeStarRule (SimpleChoice, Anything);
            OrOp.Rule = ToTerm (lexerSyntax.ChoiceAlternativeDelimiter.ToString ());
            Choice.Rule = MakePlusRule (Choice, OrOp, SimpleChoice);
            MultiChoice.Rule = lexerSyntax.ChoiceStartDelimiter.ToString () + Choice + lexerSyntax.ChoiceEndDelimiter.ToString();

            Anything.Rule = MultiChoice | Phrase | Variable;

            Sentence.Rule = MakeStarRule(Sentence, Anything);

            this.Root = Sentence;

            // 4. Operators precedenc
            // 5. Transient stuf            //TODO Mark stuff as transient to improve performance in compiler????
        }

        public override void SkipWhitespace (ISourceStream source)
        {
            // Whitespace is important for us. Don't skip it.
        }
    }
}