using System;
using System.Collections.Generic;
using NUnit.Framework;
using TextOn.Nouns;

namespace TextOn.Test
{
    [TestFixture]
    public class TestNounProfileSession
    {
        private NounProfile profile1;
        private NounSetValuesSession session1;

        [SetUp]
        public void Setup ()
        {
            #region Profile 1 : Countries, Cities, Districts, Parks
            profile1 = new NounProfile ();
            profile1.AddNewNoun ("Country", "Name of a country.", true);
            profile1.AddNewNoun ("City", "Name of a city.", true);
            profile1.AddNewNoun ("District", "Name of a district.", true);
            profile1.AddNewNoun ("Park", "Name of a park.", true);
            profile1.AddSuggestion ("Country", "Japan", new NounSuggestionDependency [0]);
            profile1.AddSuggestion ("City", "Tokyo", new NounSuggestionDependency [1] { new NounSuggestionDependency ("Country", "Japan") });
            profile1.AddSuggestion ("District", "Ginza", new NounSuggestionDependency [1] { new NounSuggestionDependency ("City", "Tokyo") });
            profile1.AddSuggestion ("Park", "Yoyogi Park", new NounSuggestionDependency [2] { new NounSuggestionDependency ("Country", "Japan"), new NounSuggestionDependency ("City", "Tokyo") });
            profile1.AddSuggestion ("Country", "United Kingdom", new NounSuggestionDependency [0]);
            profile1.AddSuggestion ("City", "London", new NounSuggestionDependency [1] { new NounSuggestionDependency ("Country", "United Kingdom") });
            profile1.AddSuggestion ("District", "Covent Garden", new NounSuggestionDependency [1] { new NounSuggestionDependency ("City", "London") });
            profile1.AddSuggestion ("Park", "Hyde Park", new NounSuggestionDependency [2] { new NounSuggestionDependency ("Country", "United Kingdom"), new NounSuggestionDependency ("City", "London") });
            session1 = profile1.MakeSetValuesSession ();
            #endregion
        }

        [TearDown]
        public void Teardown ()
        {
            session1.Deactivate ();
            session1 = null;
            profile1 = null;
        }

        [Test]
        public void TestSuggestions_Profile1 ()
        {
            var expectedCountries = new string [] { "Japan", "United Kingdom" };
            var actualCountries = session1.GetCurrentSuggestionsForNoun ("Country");

            var expectedCities = new string [] { "Tokyo", "London" };
            var actualCities = session1.GetCurrentSuggestionsForNoun ("City");

            var expectedDistricts = new string [] { "Ginza", "Covent Garden" };
            var actualDistricts = session1.GetCurrentSuggestionsForNoun ("District");

            var expectedParks = new string [] { "Yoyogi Park", "Hyde Park" };
            var actualParks = session1.GetCurrentSuggestionsForNoun ("Park");

            Assert.AreEqual (expectedCountries, actualCountries);
            Assert.AreEqual (expectedCities, actualCities);
            Assert.AreEqual (expectedDistricts, actualDistricts);
            Assert.AreEqual (expectedParks, actualParks);
        }

        [Test]
        public void TestSuggestions_Profile1_Japan ()
        {
            var nounsUpdated = new List<string> ();
            session1.SuggestionsUpdated += (n) => {
                nounsUpdated.Add (n);
            };
            
            session1.SetValue ("Country", "Japan");

            var expectedUpdated = new string [] { "City", "District", "Park" };
            var actualUpdated = nounsUpdated.ToArray ();

            var expectedCountries = new string [] { "Japan", "United Kingdom" };
            var actualCountries = session1.GetCurrentSuggestionsForNoun ("Country");

            var expectedCities = new string [] { "Tokyo" };
            var actualCities = session1.GetCurrentSuggestionsForNoun ("City");

            // no direct dependency
            var expectedDistricts = new string [] { "Ginza", "Covent Garden" };
            var actualDistricts = session1.GetCurrentSuggestionsForNoun ("District");

            var expectedParks = new string [] { "Yoyogi Park" };
            var actualParks = session1.GetCurrentSuggestionsForNoun ("Park");

            Assert.AreEqual (expectedUpdated, actualUpdated);
            Assert.AreEqual (expectedCountries, actualCountries);
            Assert.AreEqual (expectedCities, actualCities);
            Assert.AreEqual (expectedDistricts, actualDistricts);
            Assert.AreEqual (expectedParks, actualParks);
        }


        [Test]
        public void TestSuggestions_Profile1_JapanTokyo ()
        {
            var nounsUpdated = new List<string> ();
            session1.SuggestionsUpdated += (n) => {
                nounsUpdated.Add (n);
            };

            session1.SetValue ("Country", "Japan");
            session1.SetValue ("City", "Tokyo");

            var expectedUpdated = new string [] { "City", "District", "Park", "District", "Park" };
            var actualUpdated = nounsUpdated.ToArray ();

            var expectedCountries = new string [] { "Japan", "United Kingdom" };
            var actualCountries = session1.GetCurrentSuggestionsForNoun ("Country");

            var expectedCities = new string [] { "Tokyo" };
            var actualCities = session1.GetCurrentSuggestionsForNoun ("City");

            var expectedDistricts = new string [] { "Ginza" };
            var actualDistricts = session1.GetCurrentSuggestionsForNoun ("District");

            var expectedParks = new string [] { "Yoyogi Park" };
            var actualParks = session1.GetCurrentSuggestionsForNoun ("Park");

            Assert.AreEqual (expectedUpdated, actualUpdated);
            Assert.AreEqual (expectedCountries, actualCountries);
            Assert.AreEqual (expectedCities, actualCities);
            Assert.AreEqual (expectedDistricts, actualDistricts);
            Assert.AreEqual (expectedParks, actualParks);
        }
    }
}
