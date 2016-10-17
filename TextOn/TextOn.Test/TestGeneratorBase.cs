using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Moq;
using TextOn.Compiler;
using TextOn.Design;
using TextOn.Parsing;
using TextOn.Nouns;

namespace TextOn.Test
{
    public class TestGeneratorBase
    {
        public CompiledTemplate MakeProject (NounProfile nouns, CompiledNode definition, [CallerMemberName] string testName = "Test Project")
        {
            return new CompiledTemplate (nouns ?? new NounProfile (), definition);
        }

        internal CompiledNode MakeSimpleText (string text)
        {
            var mockDesignNode = new Mock<INode> ();
            return new CompiledNode (CompiledNodeType.Text, mockDesignNode.Object, false, new string [0], text, new CompiledNode [0], new TextOnParseElement [0]);
        }

        internal CompiledNode MakeSimpleVariable (string variableName)
        {
            var mockDesignNode = new Mock<INode> ();
            return new CompiledNode (CompiledNodeType.Variable, mockDesignNode.Object, false, new string [0], variableName, new CompiledNode [0], new TextOnParseElement [0]);
        }

        internal CompiledNode MakeChoice (params CompiledNode [] compiledNodes)
        {
            var mockDesignNode = new Mock<INode> ();
            var hasErrors = false;
            var requiredNouns = new HashSet<string> ();
            foreach (var item in compiledNodes) {
                hasErrors = hasErrors || item.HasErrors;
                foreach (var requiredNoun in item.RequiredNouns) {
                    requiredNouns.Add (requiredNoun);
                }
            }
            return new CompiledNode (CompiledNodeType.Choice, mockDesignNode.Object, hasErrors, requiredNouns, "", compiledNodes, new TextOnParseElement [0]);
        }
    
        internal CompiledNode MakeSequential (params CompiledNode [] compiledNodes)
        {
            var mockDesignNode = new Mock<INode> ();
            var hasErrors = false;
            var requiredNouns = new HashSet<string> ();
            foreach (var item in compiledNodes) {
                hasErrors = hasErrors || item.HasErrors;
                foreach (var requiredNoun in item.RequiredNouns) {
                    requiredNouns.Add (requiredNoun);
                }
            }
            return new CompiledNode (CompiledNodeType.Sequential, mockDesignNode.Object, hasErrors, requiredNouns, "", compiledNodes, new TextOnParseElement [0]);
        }
    }
}
