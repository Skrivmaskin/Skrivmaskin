using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Moq;
using TextOn.Compiler;
using TextOn.Design;
using TextOn.Parsing;

namespace TextOn.Test
{
    public class TestGeneratorBase
    {
        public CompiledTemplate MakeProject (IEnumerable<ICompiledVariable> variables, ICompiledNode definition, [CallerMemberName] string testName = "Test Project")
        {
            return new CompiledTemplate (variables ?? new List<ICompiledVariable> (), definition);
        }

        internal TextCompiledNode MakeSimpleText (string text)
        {
            var mockDesignNode = new Mock<INode> ();
            return new TextCompiledNode (text, mockDesignNode.Object);
        }

        internal VariableCompiledNode MakeSimpleVariable (string variableName)
        {
            var mockDesignNode = new Mock<INode> ();
            return new VariableCompiledNode (variableName, mockDesignNode.Object);
        }

        internal ChoiceCompiledNode MakeChoice (params ICompiledNode [] compiledNodes)
        {
            var mockDesignNode = new Mock<INode> ();
            return new ChoiceCompiledNode (compiledNodes.ToList (), mockDesignNode.Object);
        }
    
        internal SequentialCompiledNode MakeSequential (params ICompiledNode [] compiledNodes)
        {
            var mockDesignNode = new Mock<INode> ();
            return new SequentialCompiledNode (compiledNodes.ToList (), mockDesignNode.Object);
        }
    }
}
