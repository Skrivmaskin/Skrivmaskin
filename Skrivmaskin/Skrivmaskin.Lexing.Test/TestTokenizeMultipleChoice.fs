namespace Skrivmaskin.Lexing.Test
open System
open Xunit
open Skrivmaskin.Lexing

type TestTokenizeMultipleChoice() =
    [<Theory>]
    [<InlineData("Only one raw text", 1)>]
    [<InlineData("Some $$RAW$$ text with $$VARIABLE:S$$ in it", 1)>]
    [<InlineData("Some $$RAW$$ text||with $$VARIABLE:S$$ in it", 2)>]
    [<InlineData("One Two||$$THREE$$ 4||5 $$SIX:7$$||eight", 4)>]
    member x.TestParseSuccess(multipleChoiceText, expectedNumChoices) =
        match Lexer.tokenizeMultipleChoice 2 multipleChoiceText with
        | Error e -> failwithf "Unexpected error: %A" e
        | Success s ->
            Assert.Equal(expectedNumChoices, s |> Array.length)
