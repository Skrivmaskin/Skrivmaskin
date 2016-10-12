// This grammar is based on the ExpressionEvaluatorGrammar from Irony.Sample// Copyright (c) Roman Ivantsov
// Details at http://irony.codeplex.com
using Irony.Parsing;
using TextOn.Interfaces;

namespace TextOn.Lexing
{
    [Language ("TextOn", "1.0", "Basic grammar for single line TextOn input")]
    internal class TextOnGrammar : Grammar
    {
        public TextOnGrammar (ILexerSyntax lexerSyntax) : base()
        {
            this.GrammarComments = @"Basic grammar for single line TextOn input";

            // 1. Terminals
            var Text = new TextOnTextTerminal (nameof(TextOnParseNodes.Text), lexerSyntax, true);
            var Escape = new TextOnEscapeTerminal (nameof (TextOnParseNodes.Escape), lexerSyntax);
            var VarName = new TextOnTextTerminal (nameof (TextOnParseNodes.VarName), lexerSyntax, false);

            // 2. Non-terminals
            var CompoundText = new NonTerminal (nameof (TextOnParseNodes.CompoundText));
            var Phrase = new NonTerminal (nameof (TextOnParseNodes.Phrase));
            var Noun = new NonTerminal (nameof (TextOnParseNodes.Noun));
            var Sentence = new NonTerminal (nameof (TextOnParseNodes.Sentence));
            var SimpleChoice = new NonTerminal (nameof (TextOnParseNodes.SimpleChoice));
            var MultiChoice = new NonTerminal (nameof (TextOnParseNodes.MultiChoice));
            var Choice = new NonTerminal (nameof (TextOnParseNodes.Choice));
            var OrOp = new NonTerminal (nameof (TextOnParseNodes.OrOp));
            var Anything = new NonTerminal (nameof (TextOnParseNodes.Anything));

            // 3. BNF rules
            CompoundText.Rule = Text | Escape;
            Phrase.Rule = MakePlusRule (Phrase, CompoundText);
            Noun.Rule = lexerSyntax.VariableStartDelimiter.ToString () + VarName + lexerSyntax.VariableEndDelimiter.ToString ();

            SimpleChoice.Rule = MakeStarRule (SimpleChoice, Anything);
            OrOp.Rule = ToTerm (lexerSyntax.ChoiceAlternativeDelimiter.ToString ());
            Choice.Rule = MakePlusRule (Choice, OrOp, SimpleChoice);
            MultiChoice.Rule = lexerSyntax.ChoiceStartDelimiter.ToString () + Choice + lexerSyntax.ChoiceEndDelimiter.ToString();

            Anything.Rule = MultiChoice | Phrase | Noun;

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