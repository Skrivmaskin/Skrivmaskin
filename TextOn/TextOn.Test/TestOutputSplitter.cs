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
            var sequentialNode = OutputSplitter.Split ("");
            Assert.IsNotNull (sequentialNode);
            Assert.AreEqual (NodeType.Sequential, sequentialNode.Type);
            Assert.AreEqual ("Sentences", sequentialNode.Text);
            Assert.IsEmpty (sequentialNode.ChildNodes);
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
            var sequentialNode = OutputSplitter.Split (output);
            Assert.IsNotNull (sequentialNode);
            Assert.AreEqual (NodeType.Sequential, sequentialNode.Type);
            Assert.AreEqual ("Sentences", sequentialNode.Text);
            Assert.AreEqual (expected.Length, sequentialNode.ChildNodes.Length);
            for (int i = 0; i < expected.Length; i++) {
                var textNode = sequentialNode.ChildNodes [i];
                Assert.IsNotNull (textNode);
                Assert.AreEqual (NodeType.Text, textNode.Type);
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
            var sequentialNode = OutputSplitter.Split (output);
            Assert.IsNotNull (sequentialNode);
            Assert.AreEqual (NodeType.Sequential, sequentialNode.Type);
            Assert.AreEqual ("Sentences", sequentialNode.Text);
            Assert.AreEqual (expected.Length, sequentialNode.ChildNodes.Length);
            for (int i = 0; i < expected.Length; i++) {
                var textNode = sequentialNode.ChildNodes [i];
                Assert.IsNotNull (textNode);
                Assert.AreEqual (NodeType.Text, textNode.Type);
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
            var sequentialNode = OutputSplitter.Split (output);
            Assert.IsNotNull (sequentialNode);
            Assert.AreEqual (NodeType.Sequential, sequentialNode.Type);
            Assert.AreEqual ("Paragraphs", sequentialNode.Text);
            Assert.AreEqual (5, sequentialNode.ChildNodes.Length);

            var sn1 = sequentialNode.ChildNodes [0];
            Assert.IsNotNull (sn1);
            Assert.AreEqual (NodeType.Sequential, sn1.Type);
            Assert.AreEqual ("Sentences 1", sn1.Text);
            Assert.AreEqual (expected1.Length, sn1.ChildNodes.Length);
            for (int i = 0; i < expected1.Length; i++) {
                var textNode = sn1.ChildNodes [i];
                Assert.IsNotNull (textNode);
                Assert.AreEqual (NodeType.Text, textNode.Type);
                Assert.AreEqual (expected1 [i], textNode.Text);
            }

            Assert.IsTrue (sequentialNode.ChildNodes [1].Type == NodeType.ParagraphBreak);

            var sn2 = sequentialNode.ChildNodes [2];
            Assert.IsNotNull (sn2);
            Assert.AreEqual (NodeType.Sequential, sn2.Type);
            Assert.AreEqual ("Sentences 2", sn2.Text);
            Assert.AreEqual (expected2.Length, sn2.ChildNodes.Length);
            for (int i = 0; i < expected2.Length; i++) {
                var textNode = sn2.ChildNodes [i];
                Assert.IsNotNull (textNode);
                Assert.AreEqual (NodeType.Text, textNode.Type);
                Assert.AreEqual (expected2 [i], textNode.Text);
            }

            Assert.IsTrue (sequentialNode.ChildNodes [3].Type == NodeType.ParagraphBreak);

            var sn3 = sequentialNode.ChildNodes [4];
            Assert.IsNotNull (sn3);
            Assert.AreEqual (NodeType.Sequential, sn3.Type);
            Assert.AreEqual ("Sentences 3", sn3.Text);
            Assert.AreEqual (expected3.Length, sn3.ChildNodes.Length);
            for (int i = 0; i < expected3.Length; i++) {
                var textNode = sn3.ChildNodes [i];
                Assert.IsNotNull (textNode);
                Assert.AreEqual (NodeType.Text, textNode.Type);
                Assert.AreEqual (expected3 [i], textNode.Text);
            }
        }
    }
}
