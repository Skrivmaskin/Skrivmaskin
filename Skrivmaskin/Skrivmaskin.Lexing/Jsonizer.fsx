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
open Newtonsoft.Json

let variableRegex = new Regex("(.*)\[([^\[\]]+)\](.*)")

/// Transform a single line into "new" toy syntax.
let rec transformLine n (line:string) =
    let variableMatch = variableRegex.Match(line)
    if n >= 10 then
        failwithf "%s" line
    else if variableMatch.Success then
        transformLine (n + 1) (variableMatch.Groups.[1].Value + "$L$" + variableMatch.Groups.[2].Value + "$R$" + variableMatch.Groups.[3].Value)
    else
        let mcOffset = line.IndexOf("/")
        if mcOffset > 0 then
            let newLine =
                line.ToCharArray()
                |> Seq.fold
                    (fun (p,l,b) c ->
                        if Char.IsWhiteSpace c || c = '.' || c = ',' || c = '!' || c = '?' then
                            if b then
                                ("", l + "{" + p.Replace("_", " ") + "}" + (c.ToString()), false)
                            else
                                ("", l + p + (c.ToString()), false)
                        else if c = '/' then
                            (p + "|", l, true)
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
                (Some(((transformLine 0 line).Replace("$L$", "[").Replace("$R$", "]"))::(currentSet |> defaultArg <| [])), total, state))
        (None,[],Preparing)
    |> fun (_,a,_) -> a
    |> List.rev
    |> List.map
        (fun (a,li) ->
            let sentenceChoices = new List<INode>(li |> Seq.map (fun text -> new TextNode(text) :> INode))
            new ChoiceNode((match a with | InStycke(n) -> sprintf "STYCKE %d" n | _ -> ""), sentenceChoices) :> INode)
    |> fun li -> new SequentialNode(name, (new List<INode>(li))) :> INode
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
        new ChoiceNode("1a stycket", (new List<INode>(p))) :> INode
    |> fun p ->
        let variables = new List<Variable>()
        let markeForm = new VariableForm(Name="", Suggestion="London")
        variables.Add(new Variable(Name="MÄRKE", Description="Stad", Forms=(new List<VariableForm>(Seq.singleton markeForm))))
        let p2Form = new VariableForm(Name="", Suggestion="Covent Garden")
        variables.Add(new Variable(Name="P2", Description="Vår biluthyrning är belägen", Forms=(new List<VariableForm>(Seq.singleton p2Form))))
        let p3Form = new VariableForm(Name="", Suggestion="Volvo")
        variables.Add(new Variable(Name="P3", Description="Typ av bilar?", Forms=(new List<VariableForm>(Seq.singleton p3Form))))
        new Project(ProjectName="Sixt", VariableDefinitions=variables, Definition=p)

let fileInfo = new FileInfo("/Users/Oliver/Projects/Skrivmaskin/Json/1aStycket.json")
ProjectWriter.Write (fileInfo, project)
let project2 = ProjectWriter.Read fileInfo





