namespace Skrivmaskin.Lexing

//TODO OPS Add comment syntax to the lexer.

/// Global level token.
type internal Token =
    /// Raw text or variable.
    | Basic of BasicToken
    /// Multiple choice.
    | MultipleChoice of ChoiceToken[]
/// Basic token like raw text or a variable.
and internal BasicToken =
    /// Raw text.
    | RawText of string
    /// Variable substitution.
    | Variable of string * string option
/// Choice token, representing a bunch of basic tokens to put in this choice.
and internal ChoiceToken =
    /// One choice.
    | Choice of BasicToken[]
/// Error encountered. This is as opposed to using exception handling to cope with lexing problems. Who knows if that's a good call?
and internal ParseError =
    | UnnamedVariable of int * int
    | UnfinishedVariable of int * int
    | InvalidVariableName of int * int * char
    | UnfinishedMultipleChoice of int * int
    | EmptyMultipleChoice of int * int
/// Parse results.
and internal 'r ParseResult =
    | Error of ParseError
    | Success of 'r

[<RequireQualifiedAccess>]
module internal Lexer =
    /// Tokenize a variable.
    let tokenizeVariable (offsetToStart:int) (variableText:string) : (string * string option) ParseResult =
        // Check that there are no illegal characters and 0-1 form delimiters.
        // We can do this simply by asking only for the last delimiter, and then parsing the name (the left hand side) looking for invalid characters.
        // Syntax module contract is that form delimiter and valid variable characters are mutually exclusive.
        let formDelimiterOffset = variableText.LastIndexOf(Syntax.variableFormDelimiter)
        if variableText.Length = 0 then
            Error(UnnamedVariable(offsetToStart, offsetToStart))
        else if formDelimiterOffset = 0 then
            Error(UnnamedVariable(offsetToStart, offsetToStart))
        else if formDelimiterOffset < 0 then
            match (variableText.ToCharArray() |> Seq.mapi (fun i c -> ((i,c),(Syntax.isValidVariableNameCharacter c))) |> Seq.tryFind (snd >> not)) |> Option.map fst with
            | Some (i,c) -> Error(InvalidVariableName(offsetToStart + i, offsetToStart + i, c))
            | None -> Success(variableText, None)
        else
            let variableNameText = variableText.Substring(0, formDelimiterOffset)
            let variableFormText = variableText.Substring(formDelimiterOffset + Syntax.lenVariableFormDelimiter)
            if variableNameText.Length = 0 || variableFormText.Length = 0 then Error(UnnamedVariable(offsetToStart, offsetToStart))
            else
                match (
                        [(variableNameText, 0); (variableFormText, (variableNameText.Length + Syntax.lenVariableFormDelimiter))]
                        |> Seq.map (fun (text,so) -> text.ToCharArray() |> Seq.mapi (fun i c -> ((so + i,c),(Syntax.isValidVariableNameCharacter c))))
                        |> Seq.concat
                        |> Seq.tryFind (snd >> not)
                        |> Option.map fst) with
                | Some (i,c) -> Error(InvalidVariableName(offsetToStart + i, offsetToStart + i, c))
                | None -> Success(variableNameText, Some variableFormText)

    /// Tokenize a block of text that must be a variable or raw text.
    let rec tokenizeBasic offsetToStart (basicText:string) : BasicToken list ParseResult =
        if basicText.Length = 0 then Success []
        else
            let variableStart = basicText.IndexOf(Syntax.variableStartDelimiter)
            if variableStart >= 0 then
                let variableEnd = basicText.Substring(variableStart + Syntax.lenVariableStartDelimiter).IndexOf(Syntax.variableEndDelimiter)
                if variableEnd < 0 then Error (UnfinishedVariable ((offsetToStart + variableStart), offsetToStart + basicText.Length))
                else
                    match (tokenizeVariable (offsetToStart + variableStart + Syntax.lenVariableStartDelimiter) (basicText.Substring(variableStart + Syntax.lenVariableStartDelimiter, variableEnd))) with
                    | Error e -> Error e
                    | Success (s, so) ->
                        let variableToken = Variable(s,so)
                        // The left now must necessarily be simply basic text (or nothing), so we do the right, then tack the left on.
                        match (tokenizeBasic (offsetToStart + variableStart + variableEnd + Syntax.lenVariableStartDelimiter + Syntax.lenVariableEndDelimiter) (basicText.Substring(variableStart + variableEnd + Syntax.lenVariableStartDelimiter + Syntax.lenVariableEndDelimiter))) with
                        | Error e -> Error e
                        | Success li ->
                            if variableStart = 0 then Success (variableToken :: li)
                            else Success (RawText(basicText.Substring(0, variableStart)) :: variableToken :: li)
            else Success [RawText(basicText)]

    /// Tokenize a multiple choice.
    let tokenizeMultipleChoice offsetToStart (multipleChoiceText:string) : ChoiceToken[] ParseResult =
        if multipleChoiceText.Length = 0 then Success [||]
        else
            // Check that there are 0-N alternative delimiters.
            let rec delimiterOffsets o  =
                let s = multipleChoiceText.Substring(o)
                let offset = s.IndexOf(Syntax.multipleChoiceAlternativeDelimiter)
                if offset < 0 then seq [(o, s)]
                else
                    seq {
                        yield (o, s.Substring(0, offset))
                        yield! (delimiterOffsets (o + offset + Syntax.lenMultipleChoiceAlternativeDelimiter)) }
            let offsets                 = delimiterOffsets 0
            let choices                 = offsets |> Seq.map (fun (o,s) -> tokenizeBasic (offsetToStart + o) s)
            let error                   = choices |> Seq.tryFind (function | Error e -> true | _ -> false)
            match error with
            | (Some (Error e)) -> Error e
            | _ ->
                choices
                |> Seq.toArray
                |> Array.map (function | Success a -> Choice (a |> List.toArray) | _ -> failwith "Assertion failed")
                |> Success                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       

    /// Extremely simple tokenizer. We care little about breaking things into sentences or cleaning up
    /// text on behalf of the user. Simply looking for a few very specific markers in the file in precedence order,
    /// therefore, no need to take things one character at a time.
    /// This can return either a parse error or an ordered list of tokens.
    let rec tokenize offsetToStart (fullScanText:string) : Token list ParseResult =
        if fullScanText.Length = 0 then Success []
        else
            // IMPORTANT : Must search for multiple choice delimiters prior to variables, since MC might contain a Var.
            let multipleChoiceStart = fullScanText.IndexOf(Syntax.multipleChoiceStartDelimiter)
            if multipleChoiceStart >= 0 then
                let multipleChoiceEnd = fullScanText.Substring(multipleChoiceStart + Syntax.lenMultipleChoiceStartDelimiter).IndexOf(Syntax.multipleChoiceEndDelimiter)
                if multipleChoiceEnd = 0 then Error (EmptyMultipleChoice ((offsetToStart + multipleChoiceStart), (offsetToStart + multipleChoiceStart + Syntax.lenMultipleChoiceStartDelimiter + Syntax.lenMultipleChoiceEndDelimiter)))
                else if multipleChoiceEnd < 0 then Error (UnfinishedMultipleChoice ((offsetToStart + multipleChoiceStart), offsetToStart + fullScanText.Length))
                else
                    match (tokenizeMultipleChoice (offsetToStart + multipleChoiceStart + Syntax.lenMultipleChoiceStartDelimiter) (fullScanText.Substring(multipleChoiceStart + Syntax.lenMultipleChoiceStartDelimiter, multipleChoiceEnd))) with
                    | Error e -> Error e
                    | Success arr ->
                        let multipleChoiceToken = MultipleChoice(arr)
                        // The left now must necessarily be simply basic text (or nothing).
                        match (tokenize (offsetToStart + multipleChoiceStart + multipleChoiceEnd + Syntax.lenMultipleChoiceStartDelimiter + Syntax.lenMultipleChoiceEndDelimiter) (fullScanText.Substring(multipleChoiceStart + multipleChoiceEnd + Syntax.lenMultipleChoiceStartDelimiter + Syntax.lenMultipleChoiceEndDelimiter))) with
                        | Error e -> Error e
                        | Success li ->
                            if multipleChoiceStart = 0 then Success (multipleChoiceToken :: li)
                            else Success (Basic(RawText(fullScanText.Substring(0, multipleChoiceStart))) :: multipleChoiceToken :: li)
            else
                match tokenizeBasic offsetToStart fullScanText with
                | Error e -> Error e
                | Success ts -> Success (ts |> List.map Basic)
