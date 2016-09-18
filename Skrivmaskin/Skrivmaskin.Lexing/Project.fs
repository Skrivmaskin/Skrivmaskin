namespace Skrivmaskin.Lexing

open System
open System.IO

open Newtonsoft.Json

//TODO OPS Sort out all these namespaces

type SentenceChoice = string // user format
type Sentence =
    {
        SentenceName            : string
        Sentence                : SentenceChoice list
    }
type ParagraphChoice =
    {
        ParagraphChoiceName     : string
        ParagraphChoice         : Sentence list
    }
type Paragraph =
    {
        ParagraphName           : string
        Paragraph               : ParagraphChoice list
    }
type Project = Paragraph list

[<RequireQualifiedAccess>]
module ProjectWriter =
    /// Serialize with Json.
    let write (fileInfo:FileInfo) (project:Project) =
        let serializer  = new JsonSerializer()
        serializer.Formatting <- Formatting.Indented
        use stream      = new FileStream(fileInfo.FullName, FileMode.Create, FileAccess.Write)
        use writer      = new StreamWriter(stream)
        serializer.Serialize(writer, project)

