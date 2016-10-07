﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Skrivmaskin.Design;

namespace Skrivmaskin.Generation
{
    /// <summary>
    /// Annotated output from the generator.
    /// </summary>
    public sealed class AnnotatedOutput
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Skrivmaskin.Generation.AnnotatedOutput"/> class.
        /// </summary>
        /// <param name="text">Text.</param>
        internal AnnotatedOutput (IReadOnlyList<AnnotatedText> text)
        {
            Text = text;
        }

        /// <summary>
        /// Gets the annotated text components.
        /// </summary>
        /// <value>The text.</value>
        public IReadOnlyList<AnnotatedText> Text { get; private set; }

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:Skrivmaskin.Generation.AnnotatedOutput"/>.
        /// </summary>
        /// <returns>A <see cref="T:System.String"/> that represents the current <see cref="T:Skrivmaskin.Generation.AnnotatedOutput"/>.</returns>
        public override string ToString ()
        {
            var builder = new StringBuilder ();
            foreach (var text in Text) {
                builder.Append (text.Text);
            }
            return builder.ToString ();
        }

        /// <summary>
        /// Gets the index of the design node for character.
        /// </summary>
        /// <remarks>
        /// Returns null if the index does not match to a node.
        /// </remarks>
        /// <returns>The design node for character index.</returns>
        /// <param name="characterIndex">Character index.</param>
        public INode GetDesignNodeForCharacterIndex (int characterIndex)
        {
            int currentCharacterIndex = 0;
            foreach (var text in Text) {
                currentCharacterIndex += text.Text.Length;
                if (currentCharacterIndex >= characterIndex)
                    return text.DesignNode;
            }
            return null;
        }
    }
}
