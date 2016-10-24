// Learn more about F# at http://fsharp.net. See the 'F# Tutorial' project
// for more guidance on F# programming.

#r "../TextOn/bin/Debug/TextOn.dll"
#load "BasicStats.fs" 


open StatsPlaying
open TextOn.Design
open System


let shouldBeThree = BasicStats.countSentenceBreaksInString "Hello, my name is Jonas. I am from Sweden. This is a silly text."
let shouldBeZero = BasicStats.countSentenceBreaksInString ". I don't have a sentence"
let shouldBeTwo = BasicStats.countSentenceBreaksInString ". I have a couple of sentences....... But not many."

(* test whitespace cropping *)
let _ = "test      " |> BasicStats.removeTrailingWhitespace = "test"
let _ = "       test" |> BasicStats.removeInitialWhitespace = "test" 

(* test number of words *)
let zeroWords = "        " |> BasicStats.countWordsInString
let oneWord = "   ddmv " |> BasicStats.countWordsInString
let threeWords = " scd,ll'cds      cdsc' d" |> BasicStats.countWordsInString



(*
let textNode1 = TextOn.Design.TextNode("Hello, I", true) :> INode
let textNode2 = TextOn.Design.TextNode("Hello, I am a", true) :> INode
let textNode3 = TextOn.Design.TextNode("Hello, I am a text node", true) :> INode

let l = System.Collections.Generic.List()
let s = System.Collections.Generic.List()

l.Add(textNode1)
l.Add(textNode2)

let choiceNode = TextOn.Design.ChoiceNode("Choice1", true, l)

s.Add(choiceNode :> INode)
s.Add(textNode3)

let sequenceNode = TextOn.Design.SequentialNode("Sequence1", true, s)


let mi = BasicStats.minStat BasicStats.stringLengthTracker sequenceNode
let ma = BasicStats.maxStat BasicStats.stringLengthTracker sequenceNode
let av = BasicStats.averageStat BasicStats.stringLengthTracker sequenceNode

let mi2 = BasicStats.minStat BasicStats.wordCounter sequenceNode
let ma2 = BasicStats.maxStat BasicStats.wordCounter sequenceNode
let av2 = BasicStats.averageStat BasicStats.wordCounter sequenceNode

*)