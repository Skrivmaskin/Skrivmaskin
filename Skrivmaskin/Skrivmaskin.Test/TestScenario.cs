using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using NUnit.Framework;
using Skrivmaskin.Compiler;
using Skrivmaskin.Design;
using Skrivmaskin.Lexing;
using Moq;
using Skrivmaskin.Interfaces;
using Skrivmaskin.Generation;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Skrivmaskin.Services;

namespace Skrivmaskin.Test
{
    [TestFixture]
    public class TestScenario
    {
        private static string namespaceName = "Skrivmaskin.Test";
        private SkrivmaskinCompiler compiler;
        private SkrivmaskinGenerator generator;
        private Mock<IVariableSubstituter> mockVariableSubstituter;

        [SetUp]
        public void Setup ()
        {
            compiler = new SkrivmaskinCompiler (new DefaultLexerSyntax ());
            mockVariableSubstituter = new Mock<IVariableSubstituter> ();
            generator = new SkrivmaskinGenerator (new RandomChooser(), new SingleSpaceUnixGeneratorConfig ());
        }

        [TearDown]
        public void Teardown ()
        {
            compiler = null;
            mockVariableSubstituter = null;
            generator = null;
        }

        internal class ProjectHost : IDisposable
        {
            private readonly Stream stream;
            private readonly StreamReader streamReader;

            public ProjectHost ([CallerMemberName] string testName = "Don't know")
            {
                var assembly = Assembly.GetExecutingAssembly ();
                var resourceName = namespaceName + ".Scenario." + (testName.Replace ("Test", "").Split ('_') [0]) + ".json";
                var serializer = new JsonSerializer ();
                serializer.Formatting = Formatting.Indented;
                foreach (var converter in ProjectWriter.JsonConverters) {
                    serializer.Converters.Add (converter);
                }
                stream = assembly.GetManifestResourceStream (resourceName);
                streamReader = new StreamReader (stream);
                Object = (Project)serializer.Deserialize (streamReader, typeof (Project));
            }

            public Project Object { get; private set; }

            public void Dispose ()
            {
                streamReader.Dispose ();
                stream.Dispose ();
            }
        }

        [Test]
        public void TestProject1_London ()
        {
            using (var host = new ProjectHost ()) {
                var project = host.Object;
                var compiledProject = compiler.Compile (project);
                Assert.IsNotNull (compiledProject);
                Assert.AreEqual (3, compiledProject.Definition.RequiredVariables.Count ());
                mockVariableSubstituter.Setup ((vs) => vs.Substitute ("MÄRKE")).Returns ("London");
                mockVariableSubstituter.Setup ((vs) => vs.Substitute ("P2")).Returns ("Covent Garden");
                mockVariableSubstituter.Setup ((vs) => vs.Substitute ("P3")).Returns ("Ford");
                var generatedText = generator.GenerateWithSeed (compiledProject, mockVariableSubstituter.Object, 65);
                var expectedText = "Är du på jakt efter en hyrbil i London? Mitt i centrum av London, beläget Covent Garden, ligger vårt kontor. Inte långt härifrån hittar du några av stadens mest kända sevärdheter. Läget är med andra ord svårslaget om du planerar att utforska staden med en egen bil. Eller kanske planerar du en resa med jobbet? Oavsett vad som för dig hit kan vi på Sixt erbjuda dig som vill hyra bil i London ett brett urval av bilar. Dessutom erbjuds du även möjligheten att lägga till rad tillvalstjänster på vår biluthyrning i London . Boka en hyrbil på London och upplev höga service som vi på Sixt alltid erbjuder våra kunder. Vilken slags bil passar bäst för dig_din_resa? På vår biluthyrning i London  hittar du Ford. Besök vår webb för att göra en reservation. Här kan du självklart även hantera dina olika tillvalstjänster tilläggsfunktioner. Om du ska hyra bil i London, och komforten är a och _o, erbjuder vi till exempel en rad tillvalsfunktioner som underlättar gör resa bekväm. Sixt är en av världens främsta biluthyrare och hos oss hittar du alltid en bilmodell fordonsmodell anpassad utifrån din krav. Vårt team på Sixt biluthyrning i London önskar dig välkommen!";
                Assert.AreEqual (expectedText, generatedText);
            }
        }

        [Test]
        public void TestProject1_Leeds ()
        {
            using (var host = new ProjectHost ()) {
                var project = host.Object;
                var compiledProject = compiler.Compile (project);
                Assert.IsNotNull (compiledProject);
                Assert.AreEqual (3, compiledProject.Definition.RequiredVariables.Count ());
                mockVariableSubstituter.Setup ((vs) => vs.Substitute ("MÄRKE")).Returns ("Leeds");
                mockVariableSubstituter.Setup ((vs) => vs.Substitute ("P2")).Returns ("Carlton");
                mockVariableSubstituter.Setup ((vs) => vs.Substitute ("P3")).Returns ("Honda");
                var generatedText = generator.GenerateWithSeed (compiledProject, mockVariableSubstituter.Object, 427);
                var expectedText = "Letar du efter en centralt belägen biluthyrning i Leeds? kontoret har ett perfekt läge vid..på kort avstånd från några av stadens mest kända turistmål. Läget är även perfekt för fortsatta resor i angränsande städer. Läget är även utmärkt för längre bilresor som fortsätter vidare utanför_ ill närliggande platser i regionen. Vårt_ team hjälper dig att snabbt komma igång med din resa. Vi_på_Sixt strävar alltid efter att ge dig som vill hyra bil i Leeds en lösning av bästa kvalitet. Boka din hyrbil i Leeds smidigt direkt på webben. resenärer som vill hyra bil i Leeds erbjuds alltid en rad prisvärda erbjudanden. Vår moderna flotta rymmer.. Om du har frågor om att  hyra bil i Leeds kan du självklart också höra av dig till oss direkt. Vi hjälper gärna till en hitta en hyrbil i Leeds som passar just dina behov. Du kan till exempel göra en rad tillvalstjänster. Kanske har du särskilda önskemål då det gäller komfort och navigering? Kolla in våra paket och sätt ihop en unikt för dig. flexibiliteten för dig som vill boka en hyrbil i Leeds är stor.";
                Assert.AreEqual (expectedText, generatedText);
            }
        }
    
        [Test]
        public void TestProject2_BrownTerrier ()
        {
            using (var host = new ProjectHost ()) {
                var project = host.Object;
                var compiledProject = compiler.Compile (project);
                Assert.IsNotNull (compiledProject);
                Assert.AreEqual (7, compiledProject.Definition.RequiredVariables.Count ());
                mockVariableSubstituter.Setup ((vs) => vs.Substitute ("BREED")).Returns ("terrier");
                mockVariableSubstituter.Setup ((vs) => vs.Substitute ("BREED|Plural")).Returns ("terriers");
                mockVariableSubstituter.Setup ((vs) => vs.Substitute ("CITY")).Returns ("London");
                mockVariableSubstituter.Setup ((vs) => vs.Substitute ("NAME")).Returns ("Susie");
                mockVariableSubstituter.Setup ((vs) => vs.Substitute ("NAME|Pronoun")).Returns ("She");
                mockVariableSubstituter.Setup ((vs) => vs.Substitute ("COLOUR")).Returns ("brown");
                mockVariableSubstituter.Setup ((vs) => vs.Substitute ("COLOUR|Capitalized")).Returns ("Brown");
                var generatedText = generator.GenerateWithSeed (compiledProject, mockVariableSubstituter.Object, 17);
                var expectedText = "I used to have a brown terrier named Susie. She would run alongside me while I jogged through the streets of London. Brown terriers are so irritating.\n\nThe end.";
                Assert.AreEqual (expectedText, generatedText);
            }
        }
    }
}
