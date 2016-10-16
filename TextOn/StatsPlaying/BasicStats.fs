
namespace StatsPlaying

open TextOn.Design
open System

module BasicStats = 
   
(*
	ToDo: 	improve logic for word and letter counter
			- wich letters should count?
			- how do I detect a word?
			- Are whitespaces introduced between sentences? 
			- How should I count a paragraph?
			- Key word counter needs to handle first letter capitalized or not. 

*)

 
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
