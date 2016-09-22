using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using Skrivmaskin.Core.Lexing;

namespace Skrivmaskin.Core.Test
{
    [TestFixture]
    public class TestSpecialCharacters
    {
        [Test]
        public void TestSquareBracketEscape ()
        {
            var c = '[';
            var re = new Regex ("[" + SpecialCharacters.GetEscapeForRegexInSquareBracket (c) + "]");
            Assert.True (re.IsMatch (c.ToString ()));
            Assert.False (re.IsMatch ("Blah"));
        }
    }
}
