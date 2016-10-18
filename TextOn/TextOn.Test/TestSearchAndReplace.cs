using System;
using NUnit.Framework;
using TextOn.Design;
using TextOn.Search;

namespace TextOn.Test
{
    [TestFixture]
    public class TestSearchAndReplace
    {
        [Test]
        public void TestTemplate1_SearchExactNotFound ()
        {
            using (var host = new TemplateHost ()) {
                var template = host.Object;
                var designNode = TextOnSearchAndReplacer.Find (template.DesignTree, "BlahBlah", true);
                Assert.IsNull (designNode);
            }
        }

        [Test]
        public void TestTemplate1_SearchExactFound ()
        {
            using (var host = new TemplateHost ()) {
                var template = host.Object;
                var stopwatch = new System.Diagnostics.Stopwatch ();
                stopwatch.Start ();
                var designNode = TextOnSearchAndReplacer.Find (template.DesignTree, "bestämmer", true);
                stopwatch.Stop ();
                Assert.IsNotNull (designNode);
            }
        }
    }
}
