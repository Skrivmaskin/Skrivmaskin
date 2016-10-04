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

        /// <summary>
        /// This is the default mode stored on a per-file basis.
        /// </summary>
        /// <value>The per file modes.</value>
        public Dictionary<string, SkrivmaskinMode> PerFileModes { get; set; } = new Dictionary<string, SkrivmaskinMode> ();

        /// <summary>
        /// This is the last access date for files.
        /// </summary>
        /// <value>The last access date.</value>
        public Dictionary<string, DateTime> LastAccessDate { get; set; } = new Dictionary<string, DateTime> ();

        /// <summary>
        /// Register an opened file.
        /// </summary>
        /// <param name="file">File.</param>
        public void FileOpened (string file)
        {
            if (!PerFileModes.ContainsKey (file))
                PerFileModes.Add (file, DefaultMode);
            LastAccessDate [file] = DateTime.Today;
        }

        /// <summary>
        /// Register the user having changed settings.
        /// </summary>
        /// <param name="file">File.</param>
        /// <param name="mode">Mode.</param>
        public void ModeSet (string file, SkrivmaskinMode mode)
        {
            PerFileModes [file] = mode;
            LastAccessDate [file] = DateTime.Today;
        }
    }
}
