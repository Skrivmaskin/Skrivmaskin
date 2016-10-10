using System;
using AppKit;
using Foundation;
using TextOn.Design;
using System.Collections.Generic;
using TextOn.Compiler;
using TextOn.Lexing;
using TextOn.Interfaces;
using TextOn.Parsing;
using TextOn.Generation;

namespace TextOn.Studio
{
    /// <summary>
    /// TextOn design preview text view. A multi line text view that supports:
    /// - colouring of variables and variable brackets/dividers
    /// - colouring of choice brackets/dividers
    /// - auto-completion of variable names
    /// - reporting of parse errors.
    /// </summary>
    partial class DesignPreviewTextView : NSTextView
    {
        #region Automatic properties
        public ILexerSyntax LexerSyntax { get; set; } = new DefaultLexerSyntax ();
        // This is going to help me find errors and "word" boundaries in the partial text.
        public TextOnCompiler compiler { get; set; } = new TextOnCompiler (new DefaultLexerSyntax ());
        // I need the name of variables for auto completion.
        public CompiledTemplate CompiledTemplate { get; set; }
        public PreviewRouteNode [] Route { get; set; } = new PreviewRouteNode [0];
        #endregion

        #region Text view
        public DesignPreviewTextView (IntPtr handle) : base (handle)
        {
            Console.Error.WriteLine ("DesignPreview ctor");

            this.Delegate = new DesignPreviewTextViewDelegate (this);
        }
        #endregion

        public Func<PreviewRouteNode, bool> DoHighlightBackground { get; set; } = (n) => !n.ReachedTarget;

        public void SetValue (string value, PreviewRouteNode[] route, CompiledTemplate compiledTemplate)
        {
            Console.Error.WriteLine ("DesignPreview SetValue");

            Value = value;
            CompiledTemplate = compiledTemplate;
            Route = route;
            Highlight ();
        }

        #region Color setup
        public static NSColor GetSyntaxColorForToken (TextOnParseTokens token, int choiceDepth)
        {
            switch (token) {
            case TextOnParseTokens.ChoiceStart:
            case TextOnParseTokens.ChoiceEnd:
            case TextOnParseTokens.ChoiceDivide:
                return NSColor.Purple;
            case TextOnParseTokens.Error:
            case TextOnParseTokens.InvalidText:
            case TextOnParseTokens.InvalidCharacter:
                return NSColor.Red;
            case TextOnParseTokens.VarEnd:
            case TextOnParseTokens.VarName:
            case TextOnParseTokens.VarStart:
            case TextOnParseTokens.VarDivide:
            case TextOnParseTokens.VarFormName:
                return NSColor.Blue;
            default:
                return (choiceDepth > 0) ? NSColor.Gray : NSColor.Black;
            }
        }
        private static NSColor GetBackgroundColorForChoiceRoute ()
        {
            return NSColor.FromRgb (156, 210, 237);
        }
        private static readonly NSColor choiceRouteBackgroundColor = GetBackgroundColorForChoiceRoute ();
        #endregion

        //TODO Note I could do this much more efficiently, clean up if gets expensive.
        //TODO E.g. I could compile this line by line, and that might play well with editing?
        //TODO This relies on the text I chose for <pr/> compiling as text - yikes.
        public TextOnParseTokens Highlight ()
        {
            Console.Error.WriteLine ("DesignPreview Highlight");

            if (CompiledTemplate == null) return TextOnParseTokens.Text;
            var compiledText = compiler.CompileText (TextStorage.Value) as ICompiledText;
            var elements = compiledText.Elements;
            var lastToken = TextOnParseTokens.Error;
            var lines = TextStorage.Value.Split ('\n');
            var lineNumber = 0;
            var choiceRouteLastCharacterIndex = 0;
            while (lineNumber < lines.Length && lineNumber < Route.Length && DoHighlightBackground (Route [lineNumber])) {
                choiceRouteLastCharacterIndex += lines [lineNumber++].Length + 1; // EOL character I just knocked off?
            }
            foreach (var element in elements) {
                if (element.Range.EndCharacter < choiceRouteLastCharacterIndex) {
                    var attributes = new NSMutableDictionary ();
                    attributes.Add (NSStringAttributeKey.ForegroundColor, GetSyntaxColorForToken (element.Token, element.ChoiceDepth));
                    attributes.Add (NSStringAttributeKey.BackgroundColor, choiceRouteBackgroundColor);
                    var range = new NSRange (element.Range.StartCharacter, element.Range.EndCharacter - element.Range.StartCharacter + 1);
                    LayoutManager.SetTemporaryAttributes (attributes, range);
                } else if (element.Range.StartCharacter >= choiceRouteLastCharacterIndex) {
                    var attributes = new NSMutableDictionary ();
                    attributes.Add (NSStringAttributeKey.ForegroundColor, GetSyntaxColorForToken (element.Token, element.ChoiceDepth));
                    var range = new NSRange (element.Range.StartCharacter, element.Range.EndCharacter - element.Range.StartCharacter + 1);
                    LayoutManager.SetTemporaryAttributes (attributes, range);
                } else {
                    var attributes1 = new NSMutableDictionary ();
                    attributes1.Add (NSStringAttributeKey.ForegroundColor, GetSyntaxColorForToken (element.Token, element.ChoiceDepth));
                    attributes1.Add (NSStringAttributeKey.BackgroundColor, choiceRouteBackgroundColor);
                    var range1 = new NSRange (element.Range.StartCharacter, choiceRouteLastCharacterIndex - element.Range.StartCharacter + 1);
                    LayoutManager.SetTemporaryAttributes (attributes1, range1);
 
                    var attributes2 = new NSMutableDictionary ();
                    attributes2.Add (NSStringAttributeKey.ForegroundColor, GetSyntaxColorForToken (element.Token, element.ChoiceDepth));
                    var range2 = new NSRange (choiceRouteLastCharacterIndex, element.Range.EndCharacter - choiceRouteLastCharacterIndex + 1);
                    LayoutManager.SetTemporaryAttributes (attributes2, range2);
                }

                lastToken = element.Token;
            }
            return lastToken;
        }

        #region Overrides
        /// <summary>
        /// Look for special keys being pressed and does specific processing based on the key.
        /// </summary>
        /// <param name="theEvent">The event.</param>
        public override void KeyDown (NSEvent theEvent)
        {
            base.KeyDown (theEvent);
            var possibleComplete = Char.IsLetterOrDigit (theEvent.Characters [0]);
            //TODO Note, not ideal, because the user's position should be taken into account.
            var lastToken = Highlight ();
            if (possibleComplete && (CompiledTemplate != null) && (CompiledTemplate.VariableDefinitions.Count > 0) && (lastToken == TextOnParseTokens.VarName) || (lastToken == TextOnParseTokens.VarFormName)) Complete (this);
        }

        public override NSRange RangeForUserCompletion ()
        {
            var range = base.RangeForUserCompletion ();
            // either the character before the range is a '[' or a '|'
            var start = range.Location;
            var characters = TextStorage.Value.ToCharArray ();
            while ((start > 0) && (characters [start - 1] != LexerSyntax.VariableStartDelimiter)) --start;
            return new NSRange (start - 1, range.Length + range.Location - start + 1);
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when source cell clicked. 
        /// </summary>
        /// <remarks>NOTE: This replaces the built-in <c>CellClicked</c> event because we
        /// are providing a custom <c>NSTextViewDelegate</c> and it is unavailable.</remarks>
        public event EventHandler<NSTextViewClickedEventArgs> SourceCellClicked;

        /// <summary>
        /// Raises the source cell clicked event.
        /// </summary>
        /// <param name="sender">The controller raising the event.</param>
        /// <param name="e">Arguments defining the event.</param>
        internal void RaiseSourceCellClicked (object sender, NSTextViewClickedEventArgs e)
        {
            SourceCellClicked?.Invoke (sender, e);
        }

        /// <summary>
        /// Occurs when source cell double clicked.
        /// </summary>
        /// <remarks>NOTE: This replaces the built-in <c>CellDoubleClicked</c> event because we
        /// are providing a custom <c>NSTextViewDelegate</c> and it is unavailable.</remarks>
        public event EventHandler<NSTextViewDoubleClickEventArgs> SourceCellDoubleClicked;

        /// <summary>
        /// Raises the source cell double clicked event.
        /// </summary>
        /// <param name="sender">The controller raising the event.</param>
        /// <param name="e">Arguments defining the event.</param>
        internal void RaiseSourceCellDoubleClicked (object sender, NSTextViewDoubleClickEventArgs e)
        {
            SourceCellDoubleClicked?.Invoke (sender, e);
        }

        /// <summary>
        /// Occurs when source cell dragged.
        /// </summary>
        /// <remarks>NOTE: This replaces the built-in <c>DragCell</c> event because we
        /// are providing a custom <c>NSTextViewDelegate</c> and it is unavailable.</remarks>
        public event EventHandler<NSTextViewDraggedCellEventArgs> SourceCellDragged;

        /// <summary>
        /// Raises the source cell dragged event.
        /// </summary>
        /// <param name="sender">The controller raising the event.</param>
        /// <param name="e">Arguments defining the event.</param>
        internal void RaiseSourceCellDragged (object sender, NSTextViewDraggedCellEventArgs e)
        {
            SourceCellDragged?.Invoke (sender, e);
        }

        /// <summary>
        /// Occurs when source selection changed.
        /// </summary>
        /// <remarks>NOTE: This replaces the built-in <c>DidChangeSelection</c> event because we
        /// are providing a custom <c>NSTextViewDelegate</c> and it is unavailable.</remarks>
        public event EventHandler SourceSelectionChanged;

        /// <summary>
        /// Raises the source selection changed event.
        /// </summary>
        /// <param name="sender">The controller raising the event.</param>
        /// <param name="e">Arguments defining the event.</param>
        internal void RaiseSourceSelectionChanged (object sender, EventArgs e)
        {
            SourceSelectionChanged?.Invoke (sender, e);
        }

        /// <summary>
        /// Occurs when source typing attributes changed.
        /// </summary>
        /// <remarks>NOTE: This replaces the built-in <c>DidChangeTypingAttributes</c> event because we
        /// are providing a custom <c>NSTextViewDelegate</c> and it is unavailable.</remarks>
        public event EventHandler SourceTypingAttributesChanged;

        /// <summary>
        /// Raises the source typing attributes changed event.
        /// </summary>
        /// <param name="sender">The controller raising the event.</param>
        /// <param name="e">Arguments defining the event.</param>
        internal void RaiseSourceTypingAttributesChanged (object sender, EventArgs e)
        {
            SourceTypingAttributesChanged?.Invoke (sender, e);
        }
        #endregion
    }
}
