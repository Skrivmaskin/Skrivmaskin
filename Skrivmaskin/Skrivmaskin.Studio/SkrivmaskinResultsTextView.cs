﻿using System;
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
    /// Skrivmaskin results text view. A single line text view (this is to be set in the Interface Builder using "Text Field" property, but is assumed here),
    /// that supports:
    /// - colouring of variables and variable brackets/dividers
    /// - colouring of choice brackets/dividers
    /// - auto-completion of variable names
    /// - reporting of parse errors.
    /// </summary>
    [Register (nameof (SkrivmaskinResultsTextView))]
    public class SkrivmaskinResultsTextView : NSTextView
    {
        #region Automatic properties
        #endregion

        #region Text view
        public SkrivmaskinResultsTextView (IntPtr handle) : base (handle)
        {
            this.Delegate = new SkrivmaskinResultsTextViewDelegate (this);
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
            var charIndex = e.CharIndex;
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

        public override void MouseDown (NSEvent theEvent)
        {
            var altPressed = (theEvent.ModifierFlags & NSEventModifierMask.AlternateKeyMask) == NSEventModifierMask.AlternateKeyMask;
            // check if we're in GenerateOnly
            if (!altPressed) base.MouseDown (theEvent);
            else {
                var point = ConvertPointFromView (theEvent.LocationInWindow, null);
                var charIndex = CharacterIndex (point);
                // find the annotated text
                // get the design node
                // if null, base.MouseDown
                // otherwise:
                // raise an event, passing the design node back
                // this will allow the CentralViewController to change tab back to design
                // and find the appropriate selected index path
                // and tell the tree about it
                // then it's job jobbed innit?
                // don't forget to disable editing in the text view and change its source from a binding to programmatic I guess?
                // unless don't bother?
                base.MouseDown (theEvent);
            }
        }
    }
}