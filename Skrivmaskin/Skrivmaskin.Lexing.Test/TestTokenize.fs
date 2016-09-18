namespace Skrivmaskin.Lexing.Test
open System
open Xunit
open Skrivmaskin.Lexing

type TestTokenize() =
    let tests =
        [|
            ("Only one raw text",
                [|
                    Basic(RawText("Only one raw text"))
                |])
            ("Some $$RAW$$ text with $$VARIABLE:S$$ in it",
                [|
                    Basic(RawText("Some "))
                    Basic(Variable("RAW", None))
                    Basic(RawText(" text with "))
                    Basic(Variable("VARIABLE", Some "S"))
                    Basic(RawText(" in it"))
                |])
            ("((Some $$RAW$$ text||with $$VARIABLE:S$$ in it))",
                [|
                    MultipleChoice(
                        [|
                            Choice(
                                [|
                                    RawText("Some ")
                                    Variable("RAW", None)
                                    RawText(" text")
                                |])
                            Choice(
                                [|
                                    RawText("with ")
                                    Variable("VARIABLE", Some "S")
                                    RawText(" in it")
                                |])
                        |])
                |])
            ("((One Two||$$THREE$$ 4||5 $$SIX:7$$||eight))",
                [|
                    MultipleChoice(
                        [|
                            Choice([|RawText("One Two")|])
                            Choice([|Variable("THREE", None);RawText(" 4")|])
                            Choice([|RawText("5 ");Variable("SIX", Some "7")|])
                            Choice([|RawText("eight")|])
                        |])
                |])
            ("Sixt är en av världens främsta ((biluthyrare||biluthyrare)) och hos oss ((hittar||finner)) du alltid en ((modell||bilmodell)) fordonsmodell anpassad utifrån din ((krav||önskemål||resebehov||reseönskemål)).",
                [|
                    Basic(RawText("Sixt är en av världens främsta "))
                    MultipleChoice([|Choice([|RawText("biluthyrare")|]);Choice([|RawText("biluthyrare")|])|])
                    Basic(RawText(" och hos oss "))
                    MultipleChoice([|Choice([|RawText("hittar")|]);Choice([|RawText("finner")|])|])
                    Basic(RawText(" du alltid en "))
                    MultipleChoice([|Choice([|RawText("modell")|]);Choice([|RawText("bilmodell")|])|])
                    Basic(RawText(" fordonsmodell anpassad utifrån din "))
                    MultipleChoice([|Choice([|RawText("krav")|]);Choice([|RawText("önskemål")|]);Choice([|RawText("resebehov")|]);Choice([|RawText("reseönskemål")|])|])
                    Basic(RawText("."))
                |])
        |]

    [<Theory>]
    [<InlineData(0)>]
    [<InlineData(1)>]
    [<InlineData(2)>]
    [<InlineData(3)>]
    [<InlineData(4)>]
    member x.TestParseSuccess(n) =
        let text, expected = tests.[n]
        match Lexer.tokenize 0 text with
        | Error e -> failwithf "Unexpected error: %A" e
        | Success result ->
            Assert.Equal(expected, result)

