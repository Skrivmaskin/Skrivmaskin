using System;
using NUnit.Framework;
using Skrivmaskin.Compiler;
using Skrivmaskin.Design;
using Skrivmaskin.Lexing;
using Skrivmaskin.Parsing;
using Skrivmaskin.Services;

namespace Skrivmaskin.Test
{
    [TestFixture]
    public class TestRandomChooser
    {
        [Test]
        public void TestLastSeed ()
        {
            var rc = new RandomChooser ();

            // Choice 1
            rc.Begin ();
            var tn1 = rc.Choose (4);
            rc.End ();

            // Choice 2
            var lastSeed = rc.LastSeed;
            Assert.IsNotNull (lastSeed);
            rc.BeginWithSeed (lastSeed.Value);
            var tn2 = rc.Choose (4);
            rc.End ();

            Assert.AreEqual (tn1, tn2);
        }

        [Test]
        public void TestLastSeedGivenSeed ()
        {

            var rc = new RandomChooser ();
            var inputSeed = 57;

            // Choice 1
            rc.BeginWithSeed (inputSeed);
            var tn1 = rc.Choose (4);
            rc.End ();

            // Choice 2
            var lastSeed = rc.LastSeed;
            Assert.AreEqual (inputSeed, lastSeed.Value);
            Assert.IsNotNull (lastSeed);
            rc.BeginWithSeed (lastSeed.Value);
            var tn2 = rc.Choose (4);
            rc.End ();

            Assert.AreEqual (inputSeed, lastSeed.Value);

            Assert.AreEqual (tn1, tn2);
            // One I checked earlier.
            Assert.AreEqual (2, tn1);
            Assert.AreEqual (2, tn2);
        }
    }
}
