using System;
using System.Collections.Generic;
using Foundation;

namespace TextOn.Studio
{
    public class UserSettings
    {
        public UserSettings ()
        {
        }

        /// <summary>
        /// This is the default mode that the app starts in when opening new files.
        /// </summary>
        /// <value>The default mode.</value>
        public TextOnMode DefaultMode { get; set; } = TextOnMode.Design;
    }
}
