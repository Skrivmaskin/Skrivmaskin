using System;
using NUnit.Framework;
using Skrivmaskin.Core.Design;

namespace Skrivmaskin.Core.Test
{
    [TestFixture]
    public class TestSkrivmaskinParser
    {
        [Test]
        public void TestCompileText ()
        {
            var inputText = "Hello world";
            var node = new TextNode () { Text = inputText };
        }
    }
}
