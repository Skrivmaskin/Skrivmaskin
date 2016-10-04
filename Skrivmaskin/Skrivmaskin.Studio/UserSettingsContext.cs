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
        private const string kPerFileMode = "PerFileMode";
        private const string kLastAccessDate = "LastAccessDate";

        public static void RegisterDefaults ()
        {
            var dict = new NSMutableDictionary ();
            dict [kDefaultMode] = designMode;
            dict [kPerFileMode] = new NSDictionary ();
            dict [kLastAccessDate] = new NSDictionary ();
            NSUserDefaults.StandardUserDefaults.RegisterDefaults (dict);
        }

        public static UserSettings Settings;

        public static void LoadDefaults ()
        {
            var defaults = NSUserDefaults.StandardUserDefaults;
            NSNumber nsDefaultMode = (NSNumber)defaults [kDefaultMode];
            NSDictionary nsPerFileMode = (NSDictionary)defaults [kPerFileMode];
            NSDictionary nsLastAccessDate = (NSDictionary)defaults [kLastAccessDate];
            var defaultMode = (nsDefaultMode == generateOnlyMode) ? SkrivmaskinMode.GenerateOnly : SkrivmaskinMode.Design;
            var perFileMode = new Dictionary<string, SkrivmaskinMode> ();
            var lastAccessDate = new Dictionary<string, DateTime> ();
            foreach (var key in nsPerFileMode.Keys) {
                var mode = (((NSNumber)nsPerFileMode [key]) == generateOnlyMode) ? SkrivmaskinMode.GenerateOnly : SkrivmaskinMode.Design;
                perFileMode.Add (((NSString)key).ToString (), mode);
                var dateString = (NSString)nsLastAccessDate [key];
                var date = DateTime.ParseExact (dateString.ToString (), "yyyyMMdd", null);
                lastAccessDate.Add (((NSString)key).ToString (), date);
            }
            Settings = new UserSettings () { DefaultMode = defaultMode, PerFileModes = perFileMode, LastAccessDate = lastAccessDate };
        }

        public static void SaveDefaults ()
        {
            var defaults = NSUserDefaults.StandardUserDefaults;
            defaults [kDefaultMode] = (Settings.DefaultMode == SkrivmaskinMode.GenerateOnly) ? generateOnlyMode : designMode;
            var perFileMode = new NSMutableDictionary ();
            var lastAccessDate = new NSMutableDictionary ();
            foreach (var kvp in Settings.PerFileModes) {
                var key = (NSString)kvp.Key;
                var mode = (kvp.Value == SkrivmaskinMode.GenerateOnly) ? generateOnlyMode : designMode;
                var date = (NSString)(Settings.LastAccessDate [key].ToString ("yyyyMMdd"));
                perFileMode.Add (key, mode);
                lastAccessDate.Add (key, date);
            }
            defaults [kPerFileMode] = perFileMode;
            defaults [kLastAccessDate] = lastAccessDate;
        }
    }
}
