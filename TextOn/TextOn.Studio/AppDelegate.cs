using System.IO;
using AppKit;
using Foundation;
using TextOn.Design;
using TextOn.Storage;

namespace TextOn.Studio
{
    [Register ("AppDelegate")]
    public partial class AppDelegate : NSApplicationDelegate
    {
        public AppDelegate ()
        {
//            UserSettingsContext.RegisterDefaults ();
        }

        public override void DidFinishLaunching (NSNotification notification)
        {
            // Insert code here to initialize your application
//            UserSettingsContext.LoadDefaults ();
        }

        public override void WillTerminate (NSNotification notification)
        {
            // Insert code here to tear down your application
//            UserSettingsContext.SaveDefaults ();
        }

        public override bool OpenFile (NSApplication sender, string filename)
        {
            // Trap all errors
            try {
                filename = filename.Replace (" ", "%20");
                var url = new NSUrl ("file://" + filename);
                return OpenFile (url);
            } catch {
                return false;
            }
        }

        private bool OpenFile (NSUrl url)
        {
            var good = false;

            // Trap all errors
            try {
                var path = url.Path;

                // Is the file already open?
                for (int n = 0; n < NSApplication.SharedApplication.Windows.Length; ++n) {
                    var content = NSApplication.SharedApplication.Windows [n].ContentViewController as CentralViewController;
                    if (content != null && path == content.FilePath) {
                        // Bring window to front
                        NSApplication.SharedApplication.Windows [n].MakeKeyAndOrderFront (this);
                        return true;
                    }
                }

                // Get new window
                var storyboard = NSStoryboard.FromName ("Main", null);
                var controller = storyboard.InstantiateControllerWithIdentifier ("MainWindow") as NSWindowController;

                // Display
                controller.ShowWindow (this);

                // Load the data into the controllers.
                var viewController = controller.Window.ContentViewController as CentralViewController;
                var fileInfo = new FileInfo (path);
                var template = TextOn.Storage.TemplateWriter.Read (fileInfo);
                viewController.CreateTree (path, template);

                //viewController.SetLanguageFromPath (path);
                viewController.View.Window.SetTitleWithRepresentedFilename (Path.GetFileName (path));
                viewController.View.Window.RepresentedUrl = url;

                // Add document to the Open Recent menu
                NSDocumentController.SharedDocumentController.NoteNewRecentDocumentURL (url);

                // Make as successful
                good = true;
            } catch {
                // Mark as bad file on error
                good = false;
            }

            // Return results
            return good;
        }

        [Export ("openDocument:")]
        public void OpenDialog (NSObject sender)
        {
            var dlg = NSOpenPanel.OpenPanel;
            dlg.CanChooseFiles = true;
            dlg.CanChooseDirectories = false;

            if (dlg.RunModal () == 1) {
                // Nab the first file
                var url = dlg.Urls [0];

                if (url != null) {
                    // Open the document in a new window
                    OpenFile (url);
                }
            }
        }

        int UntitledWindowCount = 0;

        [Export ("newDocument:")]
        public void NewDocument (NSObject sender)
        {
            // Get new window
            var storyboard = NSStoryboard.FromName ("Main", null);
            var controller = storyboard.InstantiateControllerWithIdentifier ("MainWindow") as TextOnWindowController;
            controller.IsInNew = true;

            // Display
            controller.ShowWindow (this);

            // Set the title
            controller.Window.Title = (++UntitledWindowCount == 1) ? "untitled" : string.Format ("untitled {0}", UntitledWindowCount);
        }

        /// <summary>
        /// Saves the document being edited in the current window. If the document hasn't been saved
        /// before, it presents a Save File Dialog and allows to specify the name and location of 
        /// the file.
        /// </summary>
        public void SaveDocument (bool saveAs)
        {
            var window = NSApplication.SharedApplication.KeyWindow;

            var viewController = window.ContentViewController as CentralViewController;

            // Already saved?
            if (!saveAs && window.RepresentedUrl != null) {
                var path = window.RepresentedUrl.Path;

                // Save changes to file
                TemplateWriter.Write (new FileInfo (path), viewController.Template);
                window.DocumentEdited = false;
            } else {
                var dlg = new NSSavePanel ();
                dlg.Title = "Save Template";
                dlg.RequiredFileType = "json";
                dlg.BeginSheet (window, (rslt) => {
                    // File selected?
                    if (rslt == 1) {
                        var path = dlg.Url.Path;
                        TemplateWriter.Write (new FileInfo (path), viewController.Template);
                        window.DocumentEdited = false;
                        window.SetTitleWithRepresentedFilename (Path.GetFileName (path));
                        window.RepresentedUrl = dlg.Url;

                        // Add document to the Open Recent menu
                        NSDocumentController.SharedDocumentController.NoteNewRecentDocumentURL (dlg.Url);
                    }
                });
            }
        }

        /// <summary>
        /// Allows the user to specify where to save the document.
        /// </summary>
        /// <param name="sender">The controller calling the method.</param>
        [Action ("saveDocumentAs:")]
        public void SaveDocumentAs (NSObject sender)
        {
            SaveDocument (true);
        }

        /// <summary>
        /// Saves the document to its last location or allows the user to select a 
        /// location if it has never been saved before.
        /// </summary>
        /// <param name="sender">The controller calling the method.</param>
        [Action ("saveDocument:")]
        public void SaveDocument (NSObject sender)
        {
            SaveDocument (false);
        }
    }
}
