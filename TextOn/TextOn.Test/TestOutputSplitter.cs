using System;
using TextOn.Design;
using NUnit.Framework;

namespace TextOn.Test
{
    [TestFixture]
    public class TestOutputSplitter
    {
        [Test]
        public void TestEmpty ()
        {
            var sequentialNode = OutputSplitter.Split ("") as SequentialNode;
            Assert.IsNotNull (sequentialNode);
            Assert.AreEqual ("Sentences", sequentialNode.SequentialName);
            Assert.IsEmpty (sequentialNode.Sequential);
        }

        [Test]
        public void TestSingleParagraph ()
        {
            var output = "Hello. I am the Elk...    What do you want to do today? Amazing! Let's do it then. OK? ";
            var expected = new string []{
                "Hello.",
                "I am the Elk...",
                "What do you want to do today?",
                "Amazing!",
                "Let's do it then.",
                "OK?"
            };
            var sequentialNode = OutputSplitter.Split (output) as SequentialNode;
            Assert.IsNotNull (sequentialNode);
            Assert.AreEqual ("Sentences", sequentialNode.SequentialName);
            Assert.AreEqual (expected.Length, sequentialNode.Sequential.Count);
            for (int i = 0; i < expected.Length; i++) {
                var textNode = sequentialNode.Sequential [i] as TextNode;
                Assert.IsNotNull (textNode);
                Assert.AreEqual (expected [i], textNode.Text);
            }
        }


        [Test]
        public void TestSingleParagraphEllipsis ()
        {
            var output = "Some output. Here it is... I'm seriously!";
            var expected = new string []{
                "Some output.",
                "Here it is...",
                "I'm seriously!"
            };
            var sequentialNode = OutputSplitter.Split (output) as SequentialNode;
            Assert.IsNotNull (sequentialNode);
            Assert.AreEqual ("Sentences", sequentialNode.SequentialName);
            Assert.AreEqual (expected.Length, sequentialNode.Sequential.Count);
            for (int i = 0; i < expected.Length; i++) {
                var textNode = sequentialNode.Sequential [i] as TextNode;
                Assert.IsNotNull (textNode);
                Assert.AreEqual (expected [i], textNode.Text);
            }
        }

        [Test]
        public void TestMultipleParagraphs ()
        {
            var output = "Hello. I am the Elk...    What do you want to do today? Amazing! Let's do it then. OK? \r\n\r\nThis is the second paragraph!! I forgot the full stop at the end though\nAnd this is the third paragraph...";
            var expected1 = new string []{
                "Hello.",
                "I am the Elk...",
                "What do you want to do today?",
                "Amazing!",
                "Let's do it then.",
                "OK?"
            };
            var expected2 = new string []{
                "This is the second paragraph!!",
                "I forgot the full stop at the end though"
            };
            var expected3 = new string []{
                "And this is the third paragraph..."
            };
            var sequentialNode = OutputSplitter.Split (output) as SequentialNode;
            Assert.IsNotNull (sequentialNode);
            Assert.AreEqual ("Paragraphs", sequentialNode.SequentialName);
            Assert.AreEqual (5, sequentialNode.Sequential.Count);

            var sn1 = sequentialNode.Sequential [0] as SequentialNode;
            Assert.IsNotNull (sn1);
            Assert.AreEqual ("Sentences 1", sn1.SequentialName);
            Assert.AreEqual (expected1.Length, sn1.Sequential.Count);
            for (int i = 0; i < expected1.Length; i++) {
                var textNode = sn1.Sequential [i] as TextNode;
                Assert.IsNotNull (textNode);
                Assert.AreEqual (expected1 [i], textNode.Text);
            }

            Assert.IsTrue (sequentialNode.Sequential [1] is ParagraphBreakNode);

            var sn2 = sequentialNode.Sequential [2] as SequentialNode;
            Assert.IsNotNull (sn2);
            Assert.AreEqual ("Sentences 2", sn2.SequentialName);
            Assert.AreEqual (expected2.Length, sn2.Sequential.Count);
            for (int i = 0; i < expected2.Length; i++) {
                var textNode = sn2.Sequential [i] as TextNode;
                Assert.IsNotNull (textNode);
                Assert.AreEqual (expected2 [i], textNode.Text);
            }

            Assert.IsTrue (sequentialNode.Sequential [3] is ParagraphBreakNode);

            var sn3 = sequentialNode.Sequential [4] as SequentialNode;
            Assert.IsNotNull (sn3);
            Assert.AreEqual ("Sentences 3", sn3.SequentialName);
            Assert.AreEqual (expected3.Length, sn3.Sequential.Count);
            for (int i = 0; i < expected3.Length; i++) {
                var textNode = sn3.Sequential [i] as TextNode;
                Assert.IsNotNull (textNode);
                Assert.AreEqual (expected3 [i], textNode.Text);
            }
        }
    }
}
