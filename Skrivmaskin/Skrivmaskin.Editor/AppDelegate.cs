using System;
using CoreGraphics;
using Foundation;
using AppKit;
using ObjCRuntime;
using System.Collections.Generic;
using Skrivmaskin.Editor.Outline;

namespace Skrivmaskin.Editor
{
    public partial class AppDelegate : NSApplicationDelegate
    {
        MainWindowController mainWindowController;

        public override bool ApplicationShouldTerminateAfterLastWindowClosed (NSApplication sender)
        {
            return true;
        }

        public override void DidFinishLaunching (NSNotification notification)
        {
            // This is going to handle application logic for us.
            mainWindowController = new MainWindowController ();

            // This is where we setup our visual tree. These could be setup in MainWindow.xib, but
            // this example is showing programmatic creation.
            CGRect frame = mainWindowController.Window.ContentView.Frame;
            var outlineView = OutlineSetup.SetupOutlineView (frame);
            mainWindowController.Window.ContentView.AddSubview (outlineView);
            mainWindowController.Window.MakeKeyAndOrderFront (this);
        }
    }
}

