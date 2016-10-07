#r "bin/Debug/Skrivmaskin.Lexing.dll"
#r "bin/Debug/Skrivmaskin.dll"
#r "bin/Debug/Newtonsoft.Json.dll"
#r "bin/Debug/Xamarin.Forms.Core.dll"
#r "bin/Debug/Xamarin.Forms.Xaml.dll"
#r "bin/Debug/Xamarin.Forms.Platform.dll"

open System
open System.IO
open System.Collections.Generic
open System.Text.RegularExpressions
open Skrivmaskin.Lexing
open Skrivmaskin.Design
open Skrivmaskin.Compiler
open Newtonsoft.Json

let output = "Hello. My name is Oliver...    What do you want to do today? Amazing! Let's do it then. OK? \nHow in the world are you anyway?\nGoodbye."
let project = OutputSplitter.Split(output)
let sn = project.Definition :?> SequentialNode
sn.Sequential

       

