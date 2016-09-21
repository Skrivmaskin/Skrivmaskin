#r "bin/Debug/Skrivmaskin.Lexing.dll"
#r "bin/Debug/Skrivmaskin.Core.dll"
#r "bin/Debug/Newtonsoft.Json.dll"
#r "bin/Debug/Irony.dll"
#r "bin/Debug/Xamarin.Forms.Core.dll"
#r "bin/Debug/Xamarin.Forms.Xaml.dll"
#r "bin/Debug/Xamarin.Forms.Platform.dll"

open System
open System.IO
open System.Collections.Generic
open System.Text.RegularExpressions
open Skrivmaskin.Core.Lexing
open Skrivmaskin.Core.Interfaces
open Skrivmaskin.Lexing
open Skrivmaskin.Core.Design
open Newtonsoft.Json
open Irony.Parsing

let res = SkrivmaskinGrammar.Parse("blah blah blah|Hello")
res.Tokens |> printfn "%A"

