using System;
namespace TextOn.Nouns
{
    /// <summary>
    /// The user's current session for setting Noun values.
    /// </summary>
    public sealed class NounSetValuesSession
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:TextOn.Nouns.NounSetValuesSession"/> class.
        /// </summary>
        /// <remarks>
        /// Edits are not allowed while there is a session open - this is guaranteed by the profile.
        /// </remarks>
        /// <param name="nouns">The noun profile from the design template.</param>
        public NounSetValuesSession (NounProfile nouns)
        {
            
        }
    }
}
