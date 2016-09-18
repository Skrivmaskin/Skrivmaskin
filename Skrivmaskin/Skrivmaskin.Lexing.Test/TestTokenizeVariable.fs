namespace Skrivmaskin.Lexing.Test
open System
open Xunit
open Skrivmaskin.Lexing

type TestTokenizeVariable() =
    let badCharacters =
        [|
            ':'
            ','
            '$'
            '/'
            '§'
            ';'
            ']'
            '['
            '£'
            '"'
            '@'
        |]

    [<Theory>]
    [<InlineData("HELlO")>]
    [<InlineData("HELlO:Singular")>]
    [<InlineData("HELlO17")>]
    [<InlineData("HELlO17:Sin35gular")>]
    [<InlineData("KLÄdER")>]
    [<InlineData("KLÄdER:Vår")>]
    [<InlineData("KLÄdER17")>]
    [<InlineData("KLÄdER17:Vå35r")>]
    member x.TestParseSuccess(variableText) =
        match Lexer.tokenizeVariable 2 variableText with
        | Error e -> failwithf "Unexpected error: %A" e
        | _ -> ()

    [<Theory>]
    [<InlineData(2, "A:B:CD", 3, 0)>]
    [<InlineData(2, "A,B:CD", 3, 1)>]
    [<InlineData(2, "A$B:CD", 3, 2)>]
    [<InlineData(2, "A/B:CD", 3, 3)>]
    [<InlineData(2, "A§B:CD", 3, 4)>]
    [<InlineData(2, "A;B:CD", 3, 5)>]
    [<InlineData(2, "A]B:CD", 3, 6)>]
    [<InlineData(2, "A[B:CD", 3, 7)>]
    [<InlineData(2, "A£B:CD", 3, 8)>]
    [<InlineData(2, "A\"B:CD", 3, 9)>]
    [<InlineData(2, "A@B:CD", 3, 10)>]
    member x.TestInvalidVariableName(offsetToStart, variableText, errorOffset, errorCharIndex) =
        match Lexer.tokenizeVariable offsetToStart variableText with
        | Success e -> failwithf "%s" (e.ToString())
        | Error e ->
            match e with
            | InvalidVariableName(a, _, c) ->
                Assert.Equal(errorOffset, a)
                Assert.Equal(badCharacters.[errorCharIndex], c)
            | _ -> failwithf "Unexpected error %A" e

    [<Theory>]
    [<InlineData(2, ":HELLO", 2)>]
    [<InlineData(2, "HELLO:", 2)>]
    [<InlineData(2, ":", 2)>]
    [<InlineData(2, "", 2)>]
    member x.TestUnnamedVariable(offsetToStart, variableText, errorOffset) =
        match Lexer.tokenizeVariable offsetToStart variableText with
        | Success e -> failwithf "Unexpected success: %s" (e.ToString())
        | Error e ->
            match e with
            | UnnamedVariable(a, _) ->
                Assert.Equal(errorOffset, a)
            | _ -> failwithf "Unexpected error %A" e


