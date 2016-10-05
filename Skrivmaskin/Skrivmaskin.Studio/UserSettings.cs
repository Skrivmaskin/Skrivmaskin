using System;
using System.Collections.Generic;
using Foundation;

namespace Skrivmaskin.Studio
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
        public SkrivmaskinMode DefaultMode { get; set; } = SkrivmaskinMode.Design;
    }
}
