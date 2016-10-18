using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TextOn.Nouns;
using TextOn.Version0;

namespace TextOn.Design
{
    /// <summary>
    /// The user's design time project.
    /// </summary>
    public sealed class TextOnTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Design.TextOnTemplate"/> class.
        /// </summary>
        /// <param name="nouns">Nouns.</param>
        /// <param name="designTree">Definition.</param>
        public TextOnTemplate (NounProfile nouns, DesignNode designTree)
        {
            Nouns = nouns;
            DesignTree = designTree;
        }

        /// <summary>
        /// The user's noun profile.
        /// </summary>
        /// <value>The nouns.</value>
        public NounProfile Nouns { get; private set; }

        /// <summary>
        /// The definition of the template.
        /// </summary>
        /// <value>The definition.</value>
        public DesignNode DesignTree { get; private set; }
    }
}
