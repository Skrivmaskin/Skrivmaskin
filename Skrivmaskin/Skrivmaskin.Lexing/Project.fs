namespace Skrivmaskin.Lexing

open System
open System.IO

open Newtonsoft.Json

//TODO OPS Sort out all these namespaces

type SentenceChoice = string // user format
type Sentence =
    {
        SentenceName            : string
        Sentence                : SentenceChoice seq
    }
type ParagraphChoice =
    {
        ParagraphChoiceName     : string
        ParagraphChoice         : Sentence seq
    }
type Paragraph =
    {
        ParagraphName           : string
        Paragraph               : ParagraphChoice seq
    }
type VariableFormDefinition     = string
type VariableDefinition         =
    {
        Name                    : string
        Description             : string
        Suggestion              : string
        Forms                   : VariableFormDefinition seq
    }
type Project =
    {
        Variables               : VariableDefinition seq
        Paragraphs              : Paragraph seq
    }

[<RequireQualifiedAccess>]
module ProjectWriter =
    /// Serialize with Json.
    let write (fileInfo:FileInfo) (project:Project) =
        let serializer  = new JsonSerializer()
        serializer.Formatting <- Formatting.Indented
        use stream      = new FileStream(fileInfo.FullName, FileMode.Create, FileAccess.Write)
        use writer      = new StreamWriter(stream)
        serializer.Serialize(writer, project)
    
    /// Deserialize with Json.
    let read (fileInfo:FileInfo) =
        let serializer  = new JsonSerializer()
        use stream      = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read)
        use reader      = new StreamReader(stream)
        unbox<Project>(serializer.Deserialize(reader, typeof<Project>))

