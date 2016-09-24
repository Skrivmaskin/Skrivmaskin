using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Moq;
using Skrivmaskin.Compiler;
using Skrivmaskin.Design;

namespace Skrivmaskin.Test
{
    public class TestGeneratorBase
    {
        public CompiledProject MakeProject (IEnumerable<ICompiledVariable> variables, ICompiledNode definition, [CallerMemberName] string testName = "Test Project")
        {
            var project = new CompiledProject ();
            project.ProjectName = testName.Replace("Test", "");
            var variableDefinitions = new Dictionary<string, ICompiledVariable> ();
            if (variables != null) {
                foreach (var item in variables) {
                    variableDefinitions.Add (item.FullName, item);
                }
            }
            project.VariableDefinitions = variableDefinitions;
            project.Definition = definition;
            return project;
        }

        internal TextCompiledNode MakeSimpleText (string text)
        {
            var mockDesignNode = new Mock<INode> ();
            return new TextCompiledNode (text, mockDesignNode.Object, 17, 27);
        }

        internal VariableCompiledNode MakeSimpleVariable (string variableName)
        {
            var mockDesignNode = new Mock<INode> ();
            return new VariableCompiledNode (variableName, mockDesignNode.Object, 17, 27);
        }

        internal ChoiceCompiledNode MakeChoice (params ICompiledNode [] compiledNodes)
        {
            var mockDesignNode = new Mock<INode> ();
            return new ChoiceCompiledNode (compiledNodes.ToList (), mockDesignNode.Object, 17, 27);
        }
    
        internal SequentialCompiledNode MakeSequential (params ICompiledNode [] compiledNodes)
        {
            var mockDesignNode = new Mock<INode> ();
            return new SequentialCompiledNode (compiledNodes.ToList (), mockDesignNode.Object, 17, 27);
        }
    }
}
