
namespace StatsPlaying

open TextOn.Design
open System

module BasicStats = 
   
(*
	ToDo: Write the stats functions for the compiled tree instead
		- improce sentence break logic...
		- write some simple test
*)
 
 type Counter = 
     | TextCounter of Func<string,int> 
     | VariableCounter of Func<string,int>
      
 


 let private alphabetLower = "abcdefghijklmnopqrstuvwxyzåäö"
 let private alphabetUpper = alphabetLower.ToUpper()
 let private alphabetLowerSet = Set.ofArray <| alphabetLower.ToCharArray()
 let private alphabetUpperSet = Set.ofArray <| alphabetUpper.ToCharArray()
 let private letterSet = Set.union alphabetLowerSet alphabetUpperSet
 let private isCharLetter (s : char) = letterSet.Contains(s)
 let private whiteSpace = ' '
 let private delimiters = ".,;:!?".ToCharArray() |> Set.ofArray

 let private isWhitespace (c : char) = c = whiteSpace
 let private isNotWhitespace (c : char) = not(isWhitespace c)

 let private charArrayToString (c : char []) = c |> Array.fold (fun acc c -> acc+(c.ToString())) ""

 let internal removeTrailingWhitespaceFromChars (a : char []) = 
      let mutable n = a.Length
      if n > 0
      then
       while n > 0 && a.[n - 1] |> isWhitespace do n <- n - 1
       a.[0 .. (n - 1)]
      else
          [||]

 let internal removeTrailingWhitespace (s : string) = 
     s.ToCharArray() |> removeTrailingWhitespaceFromChars |> charArrayToString

 let internal removeInitialWhitespace (s : string) = s.ToCharArray() |> Array.rev  |> removeTrailingWhitespaceFromChars |> Array.rev  |> charArrayToString

 // a sentence end is marked by a letter character and a "."
 let internal sentenceCounter (s : string) = 
     let a = s.ToCharArray()
     let rec f (s : char []) = 
         if s.Length < 2 then 0
         else (if s.[0] |> isCharLetter && s.[1] = '.' then 1 else 0) + f s.[1 .. ]
     f a 

 

 // returns number of characters in a string, including whitespace and non-letters
 let internal countCharactersInString (s : string) = s.ToCharArray().Length

 // a word is defined as a whitespace/start of string follwed by non-whitespace, followed by whitespace, or any of ".,:;!?" or end of string
 let internal countWordsInString (s : string) = 
     s.Split(' ') |> Array.map (fun a -> a |> removeTrailingWhitespace |> removeInitialWhitespace) |> Array.filter (fun x -> x.Length > 0) |> Array.length


 let internal variableCounter (variableFullName : string) = fun (variableName) -> if variableFullName=variableName then 1 else 0


 let sentenceCount = TextCounter (new Func<string,int>(sentenceCounter))
 let charachterCount = TextCounter (new Func<string,int>(countCharactersInString))
 let wordCount = TextCounter (new Func<string,int>(countWordsInString))
 let variableCount variableName = VariableCounter (new Func<string,int>(variableCounter variableName))

 (*
    We seed the guy doing the stats with a collection of counters. 
    It then traverses the tree and calculates min/max and median together with a backref to the designed tree.
 *)

 // we need a way to combine path's
 // assume that a Path is an ordered list of compiled nodes of type TextNode or VariableNode
 type Path = TextOn.Compiler.ICompiledNode List 

 let combineSequence (paths : Path List) = List.concat paths



 // A StatsMaker is a guy that inform us how we should treat non-final nodes
 type StatsMaker<'path> = 
     abstract member ChoiceNode : seq<Path * int> -> Path * int
     abstract member SequenceNode : seq<Path * int> -> Path * int


 let minMaker = 
     {
         new StatsMaker<int> with 
          member this.ChoiceNode (choiceContent :seq<Path * int>)  = choiceContent |> Seq.minBy snd
          member this.SequenceNode (sequenceContent :seq<Path * int>)  = 
                                         sequenceContent 
                                         |> Seq.fold 
                                                (fun (accP,accC) (p,c) -> List.append accP [p], accC + c) ([],0) 
     }

 let maxMaker = 
      {
          new StatsMaker<int> with 
           member this.ChoiceNode (choiceContent :seq<Path * int>)  = choiceContent |> Seq.maxBy snd
           member this.SequenceNode (sequenceContent :seq<Path * int>)  = 
                                          sequenceContent 
                                          |> Seq.fold 
                                                 (fun (accP,accC) (p,c) -> List.append accP p, accC + c) ([],0) 
      }