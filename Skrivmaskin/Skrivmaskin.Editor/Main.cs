using System;
using CoreGraphics;
using Foundation;
using AppKit;
using ObjCRuntime;

namespace Skrivmaskin.Editor
{
    class MainClass
    {
        static void Main (string[] args)
        {
            NSApplication.Init ();
            NSApplication.Main (args);
        }
    }
}

