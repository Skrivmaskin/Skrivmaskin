namespace Skrivmaskin.Lexing

open System

[<RequireQualifiedAccess>]
module internal Syntax =
    /// Delimits the start of a variable.
    let variableStartDelimiter = "$$"

    /// Delimits the end of a variable.
    let variableEndDelimiter = "$$"

    /// Delimits the start of a variable.
    let lenVariableStartDelimiter = variableStartDelimiter.Length

    /// Delimits the end of a variable.
    let lenVariableEndDelimiter = variableEndDelimiter.Length

    /// Delimits the variable name from its form.
    let variableFormDelimiter = ":"

    /// Delimits the variable name from its form.
    let lenVariableFormDelimiter = variableFormDelimiter.Length

    /// Delimits the beginning of a multiple choice.
    let multipleChoiceStartDelimiter = "(("

    /// Delimits the beginning of a multiple choice.
    let lenMultipleChoiceStartDelimiter = multipleChoiceStartDelimiter.Length

    /// Delimits the end of a multiple choice.
    let multipleChoiceEndDelimiter = "))"

    /// Delimits the end of a multiple choice.
    let lenMultipleChoiceEndDelimiter = multipleChoiceEndDelimiter.Length

    /// Delimits alternatives within a multiple choice.
    let multipleChoiceAlternativeDelimiter = "||"

    /// Delimits alternatives within a multiple choice.
    let lenMultipleChoiceAlternativeDelimiter = multipleChoiceAlternativeDelimiter.Length

    /// Is this a valid character within a variable name.
    let isValidVariableNameCharacter (c:char) =
        Char.IsLetterOrDigit (c) // hope this is cool with diacritics
