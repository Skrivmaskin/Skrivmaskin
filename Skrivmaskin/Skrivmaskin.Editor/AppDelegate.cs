using System.IO;
using AppKit;
using Foundation;

namespace Skrivmaskin.Editor
{
    [Register ("AppDelegate")]
    public partial class AppDelegate : NSApplicationDelegate
    {
        public int NewWindowNumber { get; set; } = -1;

        public AppDelegate ()
        {
        }

        public override void DidFinishLaunching (NSNotification notification)
        {
            // Insert code here to initialize your application

        }

        public override void WillTerminate (NSNotification notification)
        {
            // Insert code here to tear down your application
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
                    var content = NSApplication.SharedApplication.Windows [n].ContentViewController as TabViewController;
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

                // Load the model into the window
                var viewController = controller.Window.ContentViewController as TabViewController;
                DesignViewController designViewController = null;
                foreach (var child in viewController.ChildViewControllers) {
                    if (child is DesignViewController) {
                        designViewController = child as DesignViewController;
                        break;
                    }
                }

                Node node;
                string errorText;
                if (Node.CreateTree (path, out node, out errorText)) {
                    designViewController.SetNode (node);
                }

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
        void OpenDialog (NSObject sender)
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

        /// <summary>
        /// The user has clicked Generate.
        /// </summary>
        /// <remarks>
        /// Responsiblities are:
        /// - parse the content of the design view (only if anything has changed) and update the compiled (or report a compilation error)
        /// - refresh the variables in the table, being careful not to lose the user's changes
        /// - randomly generate the output view.
        /// </remarks>
        /// <param name="sender">Sender.</param>
        partial void generateAction (NSObject sender)
        {
            
        }
    }
}
