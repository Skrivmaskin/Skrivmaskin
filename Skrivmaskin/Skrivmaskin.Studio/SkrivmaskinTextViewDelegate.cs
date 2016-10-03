﻿using System;
using System.Collections.Generic;
using Foundation;
using AppKit;
using CoreGraphics;

namespace Skrivmaskin.Studio
{
    /// <summary>
    /// The <see cref="SkrivmaskinTextViewDelegate"/> is used to respond to events that occur on a <see cref="SkrivmaskinTextView"/>.
    /// </summary>
    public class SkrivmaskinTextViewDelegate : NSTextViewDelegate
    {
        #region Computed Properties
        /// <summary>
        /// Gets or sets the text view.
        /// </summary>
        /// <value>The <see cref="SkrivmaskinTextView"/> this delegate is attached to.</value>
        public SkrivmaskinTextView TextView { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SkrivmaskinTextViewDelegate"/> class.
        /// </summary>
        /// <param name="textView">Text view.</param>
        public SkrivmaskinTextViewDelegate (SkrivmaskinTextView textView)
        {
            // Initialize
            TextView = textView;
        }
        #endregion

        #region Override Methods
        /// <summary>
        /// Support auto completion of variable names within a text node.
        /// </summary>
        /// <returns>The list of variable name completions that will be presented to the user.</returns>
        /// <param name="textView">The source <see cref="SkrivmaskinTextView"/>.</param>
        /// <param name="words">A list of default words automatically provided by OS X in the user's language.</param>
        /// <param name="charRange">The cursor location where the partial word exists.</param>
        /// <param name="index">The word that should be selected when the list is displayed (usually 0 meaning
        /// the first item in the list). Pass -1 for no selected items.</param>
        public override string [] GetCompletions (NSTextView textView, string [] words, NSRange charRange, ref nint index)
        {
            List<string> completions = new List<string> ();

            // We only provide completion of variables. This means we only provide any completions if there has been a parse error.


            // Return results.
            return completions.ToArray ();
        }

        /// <summary>
        /// Called when the cell is clicked.
        /// </summary>
        /// <param name="textView">The <see cref="SkrivmaskinTextView"/>.</param>
        /// <param name="cell">The cell being acted upon.</param>
        /// <param name="cellFrame">The onscreen frame of the cell.</param>
        /// <param name="charIndex">The index of the character clicked.</param>
        /// <remarks>
        /// Because a custom <c>Delegate</c> has been attached to the <c>NSTextView</c>, the normal events
        /// will not work so we are using this method to call custom <see cref="SkrivmaskinTextView"/>
        /// events instead.
        /// </remarks>
        public override void CellClicked (NSTextView textView, NSTextAttachmentCell cell, CGRect cellFrame, nuint charIndex)
        {
            // Pass through to Text Editor event
            TextView.RaiseSourceCellClicked (TextView, new NSTextViewClickedEventArgs (cell, cellFrame, charIndex));
        }

        /// <summary>
        /// Called when the cell is double-clicked.
        /// </summary>
        /// <param name="textView">The <see cref="SkrivmaskinTextView"/>.</param>
        /// <param name="cell">The cell being acted upon.</param>
        /// <param name="cellFrame">The onscreen frame of the cell.</param>
        /// <param name="charIndex">The index of the character clicked.</param>
        /// <remarks>
        /// Because a custom <c>Delegate</c> has been attached to the <c>NSTextView</c>, the normal events
        /// will not work so we are using this method to call custom <see cref="SkrivmaskinTextView"/>
        /// events instead.
        /// </remarks>
        public override void CellDoubleClicked (NSTextView textView, NSTextAttachmentCell cell, CGRect cellFrame, nuint charIndex)
        {
            // Pass through to Text Editor event
            TextView.RaiseSourceCellDoubleClicked (TextView, new NSTextViewDoubleClickEventArgs (cell, cellFrame, charIndex));
        }

        /// <summary>
        /// Called when the cell is dragged.
        /// </summary>
        /// <param name="textView">The <see cref="SkrivmaskinTextView"/>.</param>
        /// <param name="cell">The cell being acted upon.</param>
        /// <param name="rect">The onscreen frame of the cell.</param>
        /// <param name="theevent">An event defining the drag operation.</param>
        /// <remarks>
        /// Because a custom <c>Delegate</c> has been attached to the <c>NSTextView</c>, the normal events
        /// will not work so we are using this method to call custom <see cref="SkrivmaskinTextView"/>
        /// events instead.
        /// </remarks>
        public override void DraggedCell (NSTextView textView, NSTextAttachmentCell cell, CGRect rect, NSEvent theevent)
        {
            // Pass through to Text Editor event
            TextView.RaiseSourceCellDragged (TextView, new NSTextViewDraggedCellEventArgs (cell, rect, theevent));
        }

        /// <summary>
        /// Called when the text selection has changed.
        /// </summary>
        /// <param name="notification">A notification defining the change.</param>
        /// <remarks>
        /// Because a custom <c>Delegate</c> has been attached to the <c>NSTextView</c>, the normal events
        /// will not work so we are using this method to call custom <see cref="SkrivmaskinTextView"/>
        /// events instead.
        /// </remarks>
        public override void DidChangeSelection (NSNotification notification)
        {
            // Pass through to Text Editor event
            TextView.RaiseSourceSelectionChanged (TextView, EventArgs.Empty);
        }

        /// <summary>
        /// Called when the typing attributes has changed.
        /// </summary>
        /// <param name="notification">A notification defining the change.</param>
        /// <remarks>
        /// Because a custom <c>Delegate</c> has been attached to the <c>NSTextView</c>, the normal events
        /// will not work so we are using this method to call custom <see cref="SkrivmaskinTextView"/>
        /// events instead.
        /// </remarks>
        public override void DidChangeTypingAttributes (NSNotification notification)
        {
            // Pass through to Text Editor event
            TextView.RaiseSourceTypingAttributesChanged (TextView, EventArgs.Empty);
        }
        #endregion
    }
}

