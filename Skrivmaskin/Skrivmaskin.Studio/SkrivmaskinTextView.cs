using System;
using AppKit;
using Foundation;
using Skrivmaskin.Design;
using System.Collections.Generic;
using Skrivmaskin.Compiler;
using Skrivmaskin.Lexing;
using Skrivmaskin.Interfaces;
using Skrivmaskin.Parsing;

namespace Skrivmaskin.Studio
{
    /// <summary>
    /// Skrivmaskin text view. A single line text view (this is to be set in the Interface Builder using "Text Field" property, but is assumed here),
    /// that supports:
    /// - colouring of variables and variable brackets/dividers
    /// - colouring of choice brackets/dividers
    /// - auto-completion of variable names
    /// - reporting of parse errors.
    /// </summary>
    [Register (nameof (SkrivmaskinTextView))]
    public class SkrivmaskinTextView : NSTextView
    {
        #region Automatic properties
        public ILexerSyntax LexerSyntax { get; set; } = new DefaultLexerSyntax ();
        // This is going to help me find errors and "word" boundaries in the partial text.
        public SkrivmaskinCompiler compiler { get; set; } = new SkrivmaskinCompiler (new DefaultLexerSyntax ());
        // I need the name of variables for auto completion.
        public CompiledProject CompiledProject { get; set; }
        #endregion

        #region Text view
        public SkrivmaskinTextView (IntPtr handle) : base (handle)
        {
            this.Delegate = new SkrivmaskinTextViewDelegate (this);
        }
        #endregion

        #region Color setup
        public static NSColor GetColor (SkrivmaskinParseTokens token)
        {
            switch (token) {
            case SkrivmaskinParseTokens.ChoiceStart:
            case SkrivmaskinParseTokens.ChoiceEnd:
            case SkrivmaskinParseTokens.ChoiceDivide:
                return NSColor.Orange;
            case SkrivmaskinParseTokens.Error:
            case SkrivmaskinParseTokens.InvalidText:
            case SkrivmaskinParseTokens.InvalidCharacter:
                return NSColor.Red;
            case SkrivmaskinParseTokens.VarEnd:
            case SkrivmaskinParseTokens.VarName:
            case SkrivmaskinParseTokens.VarStart:
            case SkrivmaskinParseTokens.VarDivide:
            case SkrivmaskinParseTokens.VarFormName:
                return NSColor.Blue;
            default:
                return NSColor.Black;
            }
        }
        #endregion

        public SkrivmaskinParseTokens Highlight ()
        {
            if (CompiledProject == null) return SkrivmaskinParseTokens.Text;
            var compiledText = compiler.CompileText (TextStorage.Value) as ICompiledText;
            var elements = compiledText.Elements;
            var lastToken = SkrivmaskinParseTokens.Error;
            foreach (var element in elements) {
                LayoutManager.SetTemporaryAttributes (new NSDictionary (NSStringAttributeKey.ForegroundColor, GetColor (element.Token)), new NSRange (element.Range.StartCharacter, element.Range.EndCharacter));
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
            var lastToken = Highlight ();
            if (possibleComplete && (CompiledProject != null) && (CompiledProject.VariableDefinitions.Count > 0) && (lastToken == SkrivmaskinParseTokens.VarName) || (lastToken == SkrivmaskinParseTokens.VarFormName)) Complete (this);
        }

        public override NSRange RangeForUserCompletion ()
        {
            var range = base.RangeForUserCompletion ();
            // either the character before the range is a '[' or a '|'
            var start = range.Location;
            var characters = TextStorage.Value.ToCharArray();
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
