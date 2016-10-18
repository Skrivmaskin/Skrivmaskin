using System;
using System.Linq;
using System.Collections.Generic;

namespace TextOn.Design
{
    /// <summary>
    /// Index path. Represents a direct route to a node from the root of the Design tree.
    /// </summary>
    public sealed class IndexPath
    {        
        internal IndexPath (IEnumerable<int> indexes)
        {
            Indexes = indexes.ToArray ();
        }

        /// <summary>
        /// Gets the indexes.
        /// </summary>
        /// <value>The indexes.</value>
        public int [] Indexes { get; private set; }

        /// <summary>
        /// Get a new index path that includes one more level (steps into a Choice or Sequential).
        /// </summary>
        /// <param name="nextIndex">Next index.</param>
        public IndexPath With (int nextIndex)
        {
            return new IndexPath (Indexes.Concat (new int [1] { nextIndex }));
        }
    }
}
