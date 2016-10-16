// Learn more about F# at http://fsharp.net. See the 'F# Tutorial' project
// for more guidance on F# programming.

#r "../TextOn/bin/Debug/TextOn.dll"
#load "BasicStats.fs" 


open StatsPlaying
open TextOn.Design
open System


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

