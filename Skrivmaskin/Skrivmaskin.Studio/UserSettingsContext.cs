using System;
using System.Collections.Generic;
using Foundation;

namespace Skrivmaskin.Studio
{
    /// <summary>
    /// User settings context.
    /// </summary>
    /// <remarks>
    /// We store a dictionary of filenames to mode.
    /// </remarks>
    public static class UserSettingsContext
    {
        // Modes.
        private static readonly NSNumber generateOnlyMode = NSNumber.FromBoolean (true);
        private static readonly NSNumber designMode = NSNumber.FromBoolean (false);

        // Keys.
        private const string kDefaultMode = "DefaultMode";

        public static void RegisterDefaults ()
        {
//            var dict = new NSMutableDictionary ();
//            dict [kDefaultMode] = designMode;
//            NSUserDefaults.StandardUserDefaults.RegisterDefaults (dict);
        }

        public static UserSettings Settings = new UserSettings ();

        public static void LoadDefaults ()
        {
//            var defaults = NSUserDefaults.StandardUserDefaults;
//            NSNumber nsDefaultMode = (NSNumber)defaults [kDefaultMode];
//            var defaultMode = (nsDefaultMode == generateOnlyMode) ? SkrivmaskinMode.GenerateOnly : SkrivmaskinMode.Design;
//            Settings =
//                new UserSettings () {
//                DefaultMode = defaultMode
//                };
        }

        public static void SaveDefaults ()
        {
//            var defaults = NSUserDefaults.StandardUserDefaults;
//            defaults [kDefaultMode] = (Settings.DefaultMode == SkrivmaskinMode.GenerateOnly) ? generateOnlyMode : designMode;
        }
    }
}
