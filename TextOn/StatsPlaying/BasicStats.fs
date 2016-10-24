
namespace StatsPlaying

open TextOn.Design
open System

module BasicStats = 
   
(*
	ToDo: Write the stats functions for the compiled tree instead
		- improce sentence break logic...
		- write some simple test
*)
 
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

 let removeTrailingWhitespaceFromChars (a : char []) = 
      let mutable n = a.Length
      if n > 0
      then
       while n > 0 && a.[n - 1] |> isWhitespace do n <- n - 1
       a.[0 .. (n - 1)]
      else
          [||]

 let removeTrailingWhitespace (s : string) = 
     s.ToCharArray() |> removeTrailingWhitespaceFromChars |> charArrayToString

 let removeInitialWhitespace (s : string) = s.ToCharArray() |> Array.rev  |> removeTrailingWhitespaceFromChars |> Array.rev  |> charArrayToString



 // a sentence end is marked by a letter character and a "."
 let countSentenceBreaksInString (s : string) = 
     let a = s.ToCharArray()
     let rec f (s : char []) = 
         if s.Length < 2 then 0
         else (if s.[0] |> isCharLetter && s.[1] = '.' then 1 else 0) + f s.[1 .. ]
     f a 


 // returns number of characters in a string, including whitespace and non-letters
 let countCharactersInString (s : string) = s.ToCharArray().Length

 // a word is defined as a whitespace/start of string follwed by non-whitespace, followed by whitespace, or any of ".,:;!?" or end of string
 let countWordsInString (s : string) = 
     s.Split(' ') |> Array.map (fun a -> a |> removeTrailingWhitespace |> removeInitialWhitespace) |> Array.filter (fun x -> x.Length > 0) |> Array.length

(*
	ToDo: 	improve logic for word and letter counter
			- wich letters should count?
			- how do I detect a word?
			- Are whitespaces introduced between sentences? 
			- How should I count a paragraph?
			- Key word counter needs to handle first letter capitalized or not. 

*)


(*
 
 let private vanillaStat (choiceBehaviour : seq<float> -> float) (metric : TextNode -> float)  = 
    let rec f (n : INode) = 
        match n.Type with 
        | NodeType.Text -> 
            let n = n :?> TextNode
            metric n
        | NodeType.Sequential -> 
            let node = n :?> SequentialNode
            node.Sequential |> Seq.fold (fun acc n -> 
                                                let x = f n
                                                acc + x) 0.0
        | NodeType.Choice -> 
            let node = n :?> ChoiceNode
            let subNodes = node.Choices |> Seq.map f
            choiceBehaviour subNodes 
        | NodeType.ParagraphBreak -> 0.0
        | _ -> failwithf "Vanilla stats does not support NodeType %A" n.Type
        
    f
 
 
 let stringLengthTracker (s : TextNode) = s.Text.Length |> float
 let wordCounter (s : TextNode) = s.Text.Split(' ') |> Array.length |> float
 let keyWordCounter (word : string) = failwith "Not yet implemented" 


 let minStat : (TextNode -> float) -> (INode -> float) = vanillaStat (Seq.min)
 let maxStat : (TextNode -> float) -> (INode -> float) = vanillaStat (Seq.max)
 let averageStat : (TextNode -> float) -> (INode -> float) = vanillaStat (Seq.average)

 let medianStat : (TextNode -> float) -> (INode -> float) = 
    let median (s : seq<float>) = 
        let len = Seq.length s
        if len = 0 then nan 
        elif len = 1 then Seq.exactlyOne s
        else
           let s = s |> Seq.item (len / 2)
           s
    vanillaStat (median)

*)