using System;
namespace Skrivmaskin.Core.Interfaces
{
    /// <summary>
    /// Dialog service.
    /// </summary>
    public interface IDialogService
    {
        /// <summary>
        /// Gets the answer to a given question.
        /// </summary>
        /// <returns>The answer.</returns>
        /// <param name="question">Question.</param>
        /// <param name="suggestion">Suggested answer.</param>
        string GetAnswer (string question, string suggestion);
    }
}
