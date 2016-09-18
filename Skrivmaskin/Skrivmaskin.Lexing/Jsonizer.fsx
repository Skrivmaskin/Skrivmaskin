#r "bin/Debug/Skrivmaskin.Lexing.dll"
#r "bin/Debug/Newtonsoft.Json.dll"
#r "bin/Debug/Xamarin.Forms.Core.dll"
#r "bin/Debug/Xamarin.Forms.Xaml.dll"
#r "bin/Debug/Xamarin.Forms.Platform.dll"

open System
open System.IO
open System.Text.RegularExpressions
open Skrivmaskin.Lexing
open Newtonsoft.Json

let variableRegex = new Regex("(.*)\[([^\[\]]+)\](.*)")

/// Transform a single line into "new" toy syntax.
let rec transformLine n (line:string) =
    let variableMatch = variableRegex.Match(line)
    if n >= 10 then
        failwithf "%s" line
    else if variableMatch.Success then
        transformLine (n + 1) (variableMatch.Groups.[1].Value + "$$" + variableMatch.Groups.[2].Value + "$$" + variableMatch.Groups.[3].Value)
    else
        let mcOffset = line.IndexOf("/")
        if mcOffset > 0 then
            let newLine =
                line.ToCharArray()
                |> Seq.fold
                    (fun (p,l,b) c ->
                        if Char.IsWhiteSpace c || c = '.' || c = ',' || c = '!' || c = '?' then
                            if b then
                                ("", l + "((" + p.Replace("_", " ") + "))" + (c.ToString()), false)
                            else
                                ("", l + p + (c.ToString()), false)
                        else if c = '/' then
                            (p + "||", l, true)
                        else
                            (p + c.ToString(), l, b))
                    ("", "", false)
                |> fun (_,l,_) -> l
            transformLine (n + 1) newLine
        else
            line

type State =
    | AllOver
    | InStycke of int
    | Preparing

let transformAll name file =
    file
    |> File.ReadLines
    |> Seq.fold
        (fun (currentSet,total,state) line ->
            if String.IsNullOrWhiteSpace line || state = AllOver then
                (currentSet,total,state)
            else if line.StartsWith("STYCKE SLUT") then
                (None, (if currentSet.IsSome then (state,(currentSet.Value |> List.rev))::total else total), AllOver)
            else if line.StartsWith("STYCKE ") then
                let num = Int32.Parse(line.Replace("STYCKE ", "").Replace(" ", ""))
                (None, (if currentSet.IsSome then (state,(currentSet.Value |> List.rev))::total else total), InStycke(num))
            else
                (Some((transformLine 0 line)::(currentSet |> defaultArg <| [])), total, state))
        (None,[],Preparing)
    |> fun (_,a,_) -> a
    |> List.rev
    |> List.map
        (fun (a,li) ->
            let sentenceChoices = li
            let sentence =
                {
                    SentenceName = (match a with | InStycke(n) -> sprintf "STYCKE %d" n | _ -> "")
                    Sentence = sentenceChoices
                }
            sentence)
    |> fun pc ->
        {
            ParagraphChoiceName = name
            ParagraphChoice = pc 
        }

let project =
    [
        "2016-08-23"
        "2016-08-29"
        "2016-08-31"
        "2016-09-14"
    ]
    |> List.map
        (fun name ->
            transformAll name ("/Users/Oliver/Projects/Skrivmaskin/SIXT/MALLAR/Sixt/Delar/1a stycket/" + name + ".txt"))
    |> fun p ->
        {
            ParagraphName   = "1a stycket"
            Paragraph       = p
        }
    |> Seq.singleton
    |> Seq.toList

project
|> ProjectWriter.write (new FileInfo("/Users/Oliver/Projects/Skrivmaskin/Json/1aStycket.json"))

