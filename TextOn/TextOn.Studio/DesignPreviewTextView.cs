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
        // Shared compiler with the Central.
        internal TextOnCompiler Compiler = null;
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

        public void SetValue (string value, PreviewRouteNode[] route, CompiledTemplate compiledTemplate)
        {
            Console.Error.WriteLine ("DesignPreview SetValue");

            Value = value;
            CompiledTemplate = compiledTemplate;
            Route = route;
            Highlight ();
        }

        #region Color setup
        private static NSDictionary GetAttributes (PreviewRouteState state, TextOnParseTokens token, bool isInChoice)
        {
            NSColor foregroundColor;
            var attributes = new NSMutableDictionary ();
            switch (token) {
            case TextOnParseTokens.ChoiceStart:
            case TextOnParseTokens.ChoiceEnd:
            case TextOnParseTokens.ChoiceDivide:
                foregroundColor = NSColor.Purple;
                break;
            case TextOnParseTokens.Error:
            case TextOnParseTokens.InvalidText:
            case TextOnParseTokens.InvalidCharacter:
                foregroundColor = NSColor.Red;
                break;
            case TextOnParseTokens.VarEnd:
            case TextOnParseTokens.VarName:
            case TextOnParseTokens.VarStart:
                foregroundColor = NSColor.Blue;
                break;
            default:
                foregroundColor = isInChoice ? NSColor.Gray : NSColor.Black;
                break;
            }
            attributes.Add (NSStringAttributeKey.ForegroundColor, foregroundColor);
            switch(state){
            case PreviewRouteState.BeforeTarget:
                attributes.Add (NSStringAttributeKey.BackgroundColor, choiceRouteBackgroundColor);
                break;
            case PreviewRouteState.AtTarget:
                attributes.Add (NSStringAttributeKey.BackgroundColor, atTargetBackgroundColor);
                break;
            case PreviewRouteState.WithinTarget:
                attributes.Add (NSStringAttributeKey.BackgroundColor, withinTargetBackgroundColor);
                break;
            }
            return attributes;
        }
        private static NSColor GetBackgroundColorForChoiceRoute ()
        {
            return NSColor.FromRgb (150, 240, 240);
        }
        private static NSColor GetAtTargetBackgroundColor ()
        {
            return NSColor.FromRgb (240, 150, 240);
        }
        private static NSColor GetWithinTargetBackgroundColor ()
        {
            return NSColor.FromRgb (240, 240, 150);
        }
        private static readonly NSColor choiceRouteBackgroundColor = GetBackgroundColorForChoiceRoute ();
        private static readonly NSColor atTargetBackgroundColor = GetAtTargetBackgroundColor ();
        private static readonly NSColor withinTargetBackgroundColor = GetWithinTargetBackgroundColor ();
        #endregion

        internal bool IsInvalid = true;

        //TODO Note I could do this much more efficiently, clean up if gets expensive.
        //TODO E.g. I could compile this line by line, and that might play well with editing?
        //TODO This relies on the text I chose for <pr/> compiling as text - yikes.
        public TextOnParseTokens Highlight ()
        {
            Console.Error.WriteLine ("DesignPreview Highlight");

            if (CompiledTemplate == null) return TextOnParseTokens.Text;
            if (IsInvalid) return TextOnParseTokens.Text;

            var lastToken = TextOnParseTokens.Error;
            var lines = TextStorage.Value.Split ('\n');
            var currentCharacter = 0;
            var lineNumber = 0;
            foreach (var line in lines) {
                // bail if this happens
                if (lineNumber >= Route.Length) return TextOnParseTokens.Text;
                var routeNode = Route [lineNumber];
                var node = routeNode.Node;
                IEnumerable<TextOnParseElement> elements;
                if (node.Type == NodeType.Text) {
                    var compiledText = Compiler.GetCompiledNode (node);
                    elements = (compiledText == null) ? new TextOnParseElement [1] { new TextOnParseElement (TextOnParseTokens.Text, 0, new TextOnParseRange (0, line.Length - 1)) } : compiledText.Elements;
                } else {
                    elements = new TextOnParseElement [1] { new TextOnParseElement (TextOnParseTokens.Text, 0, new TextOnParseRange (0, line.Length - 1)) };
                }
                foreach (var element in elements) {
                    var range = new NSRange (currentCharacter + element.Range.StartCharacter, element.Range.EndCharacter - element.Range.StartCharacter + 1);
                    var attributes = GetAttributes (Route[lineNumber].State, element.Token, element.ChoiceDepth > 0);
                    LayoutManager.SetTemporaryAttributes (attributes, range);
                    lastToken = element.Token;
                }
                currentCharacter += line.Length + 1;
                ++lineNumber;
            }
            return lastToken;
        }

        public event Action<DesignNode> ModifiedClick;

        #region Overrides
        /// <summary>
        /// If Alt is down, navigate the Design to the desired text node in the design tree.
        /// </summary>
        /// <param name="theEvent">The event.</param>
        public override void MouseDown (NSEvent theEvent)
        {
            if (CompiledTemplate == null) {
                base.MouseDown (theEvent);
                return;
            }
            var altPressed = (theEvent.ModifierFlags & NSEventModifierMask.AlternateKeyMask) == NSEventModifierMask.AlternateKeyMask;
            if (!altPressed) {
                base.MouseDown (theEvent);
                return;
            }
            var point = ConvertPointFromView (theEvent.LocationInWindow, null);
            var charIndex = CharacterIndex (point);
            var lines = TextStorage.Value.Split ('\n');
            var currentCharacter = 0;
            var lineNumber = 0;
            foreach (var line in lines) {
                // bail if this happens
                if (lineNumber >= Route.Length) {
                    base.MouseDown (theEvent);
                    return;
                }
                currentCharacter += line.Length + 1; // newline character
                if (currentCharacter > (int)charIndex) {
                    var routeNode = Route [lineNumber];
                    var node = routeNode.Node;
                    if (node != null) {
                        ModifiedClick?.Invoke (node);
                        return;
                    } else {
                        base.MouseDown (theEvent);
                        return;
                    }
                }
                ++lineNumber;
            }
            base.MouseDown (theEvent);
        }

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
            if (possibleComplete &&
                (CompiledTemplate != null) &&
                (CompiledTemplate.Nouns.Count > 0) &&
                (lastToken == TextOnParseTokens.VarName)) Complete (this);
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
