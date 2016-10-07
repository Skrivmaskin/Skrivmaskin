using System;
using AppKit;
using CoreGraphics;
using Foundation;

namespace Skrivmaskin.Studio
{
	class SkrivmaskinResultsTextViewDelegate : NSTextViewDelegate
	{
        internal SkrivmaskinResultsTextView TextView { get; private set;}

		public SkrivmaskinResultsTextViewDelegate (SkrivmaskinResultsTextView textView)
		{
            TextView = textView;
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
	}
}