﻿// This grammar is based on the ExpressionEvaluatorGrammar from Irony.Samples
// Copyright (c) Roman Ivantsov
// Details at http://irony.codeplex.com
using Irony.Parsing;
using Skrivmaskin.Core.Interfaces;

namespace Skrivmaskin.Core.Lexing
{
    [Language ("Skrivmaskin", "1.0", "Basic grammar for single line Skrivmaskin input")]
    internal class SkrivmaskinGrammar : Grammar
    {
        public SkrivmaskinGrammar (ILexerSyntax lexerSyntax) : base()
        {
            this.GrammarComments = @"Basic grammar for single line Skrivmaskin input";

            // 1. Terminals
            var Text = new SkrivmaskinTextTerminal (nameof(SkrivmaskinTokens.Text), lexerSyntax);
            var Escape = new SkrivmaskinEscapeTerminal (nameof (SkrivmaskinTokens.Escape), lexerSyntax);
            var VarName = new RegexBasedTerminal (nameof (SkrivmaskinTokens.VarName), "[A-Za-z0-9_]+");
            var VarForm = new RegexBasedTerminal (nameof (SkrivmaskinTokens.VarForm), "[A-Za-z0-9_]+");

            // 2. Non-terminals
            var CompoundText = new NonTerminal (nameof (SkrivmaskinTokens.CompoundText));
            var Phrase = new NonTerminal (nameof (SkrivmaskinTokens.Phrase));
            var SimpleVariable = new NonTerminal (nameof (SkrivmaskinTokens.SimpleVariable));
            var CompoundVariable = new NonTerminal (nameof (SkrivmaskinTokens.CompoundVariable));
            var Variable = new NonTerminal (nameof (SkrivmaskinTokens.Variable));
            var Sentence = new NonTerminal (nameof (SkrivmaskinTokens.Sentence));
            var SimpleChoice = new NonTerminal (nameof (SkrivmaskinTokens.SimpleChoice));
            var MultiChoice = new NonTerminal (nameof (SkrivmaskinTokens.MultiChoice));
            var Choice = new NonTerminal (nameof (SkrivmaskinTokens.Choice));
            var OrOp = new NonTerminal (nameof (SkrivmaskinTokens.OrOp));
            var Anything = new NonTerminal (nameof (SkrivmaskinTokens.Anything));

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

            // 4. Operators precedence

            // 5. Transient stuff
            //TODO Mark stuff as transient to improve performance in compiler????
        }

        public override void SkipWhitespace (ISourceStream source)
        {
            // Whitespace is important for us. Don't skip it.
        }
    }
}