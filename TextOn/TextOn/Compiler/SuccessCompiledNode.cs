using System;
using System.Collections.Generic;
using TextOn.Design;
using TextOn.Parsing;

namespace TextOn.Compiler
{
    internal class SuccessCompiledNode : ICompiledNode, ICompiledText
    {
        public SuccessCompiledNode (ICompiledNode subNode, INode designNode, IEnumerable<TextOnParseElement> elements)
        {
            Node = subNode;
            Location = designNode;
            Elements = elements;
        }

        public ICompiledNode Node { get; private set; }

        public IEnumerable<TextOnParseElement> Elements { get; private set; }

        public bool HasErrors {
            get {
                return false;
            }
        }

        public INode Location { get; private set; }

        public IEnumerable<string> RequiredVariables {
            get {
                return Node.RequiredVariables;
            }
        }

        public CompiledNodeType Type {
            get {
                return CompiledNodeType.Success;
            }
        }
    }
}
