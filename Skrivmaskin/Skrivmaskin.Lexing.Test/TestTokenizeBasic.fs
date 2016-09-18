namespace Skrivmaskin.Lexing.Test
open System
open Xunit
open Skrivmaskin.Lexing

type TestTokenizeBasic() =
    [<Theory>]
    [<InlineData("Only one raw text", 1)>]
    [<InlineData("Variable at the $$END$$", 2)>]
    [<InlineData("Some $$RAW$$ text with $$VARIABLE:S$$ in it", 5)>]
    member x.TestParseSuccess(multipleChoiceText, expectedNumChoices) =
        match Lexer.tokenizeBasic 2 multipleChoiceText with
        | Error e -> failwithf "Unexpected error: %A" e
        | Success s ->
            Assert.Equal(expectedNumChoices, s |> List.length)

