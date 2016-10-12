using System;
using System.Linq;
using NUnit.Framework;
using TextOn.Nouns;

namespace TextOn.Test
{
    [TestFixture]
    public class TestNounProfile
    {
        [Test]
        public void TestSimpleDependencyRoute_Setup_ExistingDependencies ()
        {
            var profile = new NounProfile ();
            profile.AddNewNoun ("Country", "Name of a country.", true);
            profile.AddNewNoun ("City", "Name of a city.", true);
            profile.AddNewNoun ("Park", "Name of a park.", true);
            profile.AddSuggestion ("Country", "Japan", new NounSuggestionDependency [0]);
            profile.AddSuggestion ("City", "Tokyo", new NounSuggestionDependency [1] { new NounSuggestionDependency ("Country", "Japan") });
            profile.AddSuggestion ("Park", "Yoyogi Park", new NounSuggestionDependency [2] { new NounSuggestionDependency ("Country", "Japan"), new NounSuggestionDependency ("City", "Tokyo") });

            var actualDependenciesPark = profile.GetExistingDependencies ("Park").ToArray ();
            var expectedDependenciesPark = new string [] { "Country", "City" };

            var actualDependenciesCity = profile.GetExistingDependencies ("City").ToArray ();
            var expectedDependenciesCity = new string [] { "Country" };

            var actualDependenciesCountry = profile.GetExistingDependencies ("Country").ToArray ();
            var expectedDependenciesCountry = new string [] { };

            Assert.AreEqual (expectedDependenciesPark, actualDependenciesPark);
            Assert.AreEqual (expectedDependenciesCity, actualDependenciesCity);
            Assert.AreEqual (expectedDependenciesCountry, actualDependenciesCountry);

        }

        [Test]
        public void TestSimpleDependencyRoute_Remove_ExistingDependencies ()
        {
            var profile = new NounProfile ();
            profile.AddNewNoun ("Country", "Name of a country.", true);
            profile.AddNewNoun ("City", "Name of a city.", true);
            profile.AddNewNoun ("Park", "Name of a park.", true);
            profile.AddSuggestion ("Country", "Japan", new NounSuggestionDependency [0]);
            profile.AddSuggestion ("City", "Tokyo", new NounSuggestionDependency [1] { new NounSuggestionDependency ("Country", "Japan") });
            profile.AddSuggestion ("Park", "Yoyogi Park", new NounSuggestionDependency [2] { new NounSuggestionDependency ("Country", "Japan"), new NounSuggestionDependency ("City", "Tokyo") });
            profile.DeleteSuggestion ("Park", "Yoyogi Park");

            var actualDependenciesPark = profile.GetExistingDependencies ("Park").ToArray ();
            var expectedDependenciesPark = new string [] { };

            var actualDependenciesCity = profile.GetExistingDependencies ("City").ToArray ();
            var expectedDependenciesCity = new string [] { "Country" };

            var actualDependenciesCountry = profile.GetExistingDependencies ("Country").ToArray ();
            var expectedDependenciesCountry = new string [] { };

            Assert.AreEqual (expectedDependenciesPark, actualDependenciesPark);
            Assert.AreEqual (expectedDependenciesCity, actualDependenciesCity);
            Assert.AreEqual (expectedDependenciesCountry, actualDependenciesCountry);
        }

        [Test]
        public void TestSimpleDependencyRoute_RemoveOne_ExistingDependencies ()
        {
            var profile = new NounProfile ();
            profile.AddNewNoun ("Country", "Name of a country.", true);
            profile.AddNewNoun ("City", "Name of a city.", true);
            profile.AddNewNoun ("Park", "Name of a park.", true);
            profile.AddSuggestion ("Country", "Japan", new NounSuggestionDependency [0]);
            profile.AddSuggestion ("Country", "United Kingdom", new NounSuggestionDependency [0]);
            profile.AddSuggestion ("City", "Tokyo", new NounSuggestionDependency [1] { new NounSuggestionDependency ("Country", "Japan") });
            profile.AddSuggestion ("City", "London", new NounSuggestionDependency [1] { new NounSuggestionDependency ("Country", "United Kingdom") });
            profile.AddSuggestion ("Park", "Yoyogi Park", new NounSuggestionDependency [2] { new NounSuggestionDependency ("Country", "Japan"), new NounSuggestionDependency ("City", "Tokyo") });
            profile.AddSuggestion ("Park", "Hyde Park", new NounSuggestionDependency [2] { new NounSuggestionDependency ("Country", "United Kingdom"), new NounSuggestionDependency ("City", "London") });
            profile.DeleteSuggestion ("Park", "Yoyogi Park");

            var actualDependenciesPark = profile.GetExistingDependencies ("Park").ToArray ();
            var expectedDependenciesPark = new string [] { "Country", "City" };

            var actualDependenciesCity = profile.GetExistingDependencies ("City").ToArray ();
            var expectedDependenciesCity = new string [] { "Country" };

            var actualDependenciesCountry = profile.GetExistingDependencies ("Country").ToArray ();
            var expectedDependenciesCountry = new string [] { };

            Assert.AreEqual (expectedDependenciesPark, actualDependenciesPark);
            Assert.AreEqual (expectedDependenciesCity, actualDependenciesCity);
            Assert.AreEqual (expectedDependenciesCountry, actualDependenciesCountry);
        }

        [Test]
        public void TestSimpleDependencyRoute_NounsInRightOrder ()
        {
            var profile = new NounProfile ();
            profile.AddNewNoun ("Park", "Name of a park.", true);
            profile.AddNewNoun ("Country", "Name of a country.", true);
            profile.AddNewNoun ("City", "Name of a city.", true);
            profile.AddSuggestion ("Country", "Japan", new NounSuggestionDependency [0]);
            profile.AddSuggestion ("Country", "United Kingdom", new NounSuggestionDependency [0]);
            profile.AddSuggestion ("City", "Tokyo", new NounSuggestionDependency [1] { new NounSuggestionDependency ("Country", "Japan") });
            profile.AddSuggestion ("City", "London", new NounSuggestionDependency [1] { new NounSuggestionDependency ("Country", "United Kingdom") });
            profile.AddSuggestion ("Park", "Yoyogi Park", new NounSuggestionDependency [2] { new NounSuggestionDependency ("Country", "Japan"), new NounSuggestionDependency ("City", "Tokyo") });
            profile.AddSuggestion ("Park", "Hyde Park", new NounSuggestionDependency [2] { new NounSuggestionDependency ("Country", "United Kingdom"), new NounSuggestionDependency ("City", "London") });
            profile.DeleteSuggestion ("Park", "Yoyogi Park");

            var expectedOrder = new string [] { "Country", "City", "Park" };
            var actualOrder = profile.GetAllNouns ().Select ((n) => n.Name).ToArray ();

            Assert.AreEqual (expectedOrder, actualOrder);
        }

        [Test]
        public void TestTransitiveDependencyRoute_ExistingDependencies ()
        {
            var profile = new NounProfile ();
            profile.AddNewNoun ("Country", "Name of a country.", true);
            profile.AddNewNoun ("City", "Name of a city.", true);
            profile.AddNewNoun ("District", "Name of a district.", true);
            profile.AddNewNoun ("Park", "Name of a park.", true);
            profile.AddSuggestion ("Country", "Japan", new NounSuggestionDependency [0]);
            profile.AddSuggestion ("City", "Tokyo", new NounSuggestionDependency [1] { new NounSuggestionDependency ("Country", "Japan") });
            profile.AddSuggestion ("District", "Ginza", new NounSuggestionDependency [1] { new NounSuggestionDependency ("City", "Tokyo") });
            profile.AddSuggestion ("Park", "Yoyogi Park", new NounSuggestionDependency [2] { new NounSuggestionDependency ("Country", "Japan"), new NounSuggestionDependency ("City", "Tokyo") });

            var existingDependenciesPark = profile.GetExistingDependencies ("Park").ToArray ();
            var expectedDependenciesPark = new string [] { "Country", "City" };

            var existingDependenciesDistrict = profile.GetExistingDependencies ("District").ToArray ();
            var expectedDependenciesDistrict = new string [] { "Country", "City" };

            var existingDependenciesCity = profile.GetExistingDependencies ("City").ToArray ();
            var expectedDependenciesCity = new string [] { "Country" };

            var existingDependenciesCountry = profile.GetExistingDependencies ("Country").ToArray ();
            var expectedDependenciesCountry = new string [] { };

            Assert.AreEqual (expectedDependenciesPark, existingDependenciesPark);
            Assert.AreEqual (expectedDependenciesDistrict, existingDependenciesDistrict);
            Assert.AreEqual (expectedDependenciesCity, existingDependenciesCity);
            Assert.AreEqual (expectedDependenciesCountry, existingDependenciesCountry);
        }

        [Test]
        public void TestTransitiveDependencyRoute_RemoveTransitiveness_ExistingDependencies ()
        {
            var profile = new NounProfile ();
            profile.AddNewNoun ("Country", "Name of a country.", true);
            profile.AddNewNoun ("City", "Name of a city.", true);
            profile.AddNewNoun ("District", "Name of a district.", true);
            profile.AddNewNoun ("Park", "Name of a park.", true);
            profile.AddSuggestion ("Country", "Japan", new NounSuggestionDependency [0]);
            profile.AddSuggestion ("City", "Tokyo", new NounSuggestionDependency [1] { new NounSuggestionDependency ("Country", "Japan") });
            profile.AddSuggestion ("District", "Ginza", new NounSuggestionDependency [1] { new NounSuggestionDependency ("City", "Tokyo") });
            profile.AddSuggestion ("Park", "Yoyogi Park", new NounSuggestionDependency [2] { new NounSuggestionDependency ("Country", "Japan"), new NounSuggestionDependency ("City", "Tokyo") });
            profile.DeleteSuggestion ("District", "Ginza");

            var existingDependenciesPark = profile.GetExistingDependencies ("Park").ToArray ();
            var expectedDependenciesPark = new string [] { "Country", "City" };

            var existingDependenciesDistrict = profile.GetExistingDependencies ("District").ToArray ();
            var expectedDependenciesDistrict = new string [] { };

            var existingDependenciesCity = profile.GetExistingDependencies ("City").ToArray ();
            var expectedDependenciesCity = new string [] { "Country" };

            var existingDependenciesCountry = profile.GetExistingDependencies ("Country").ToArray ();
            var expectedDependenciesCountry = new string [] { };

            Assert.AreEqual (expectedDependenciesPark, existingDependenciesPark);
            Assert.AreEqual (expectedDependenciesDistrict, existingDependenciesDistrict);
            Assert.AreEqual (expectedDependenciesCity, existingDependenciesCity);
            Assert.AreEqual (expectedDependenciesCountry, existingDependenciesCountry);
        }

        [Test]
        public void TestTransitiveDependencyRoute_RemoveStillTransitive_ExistingDependencies ()
        {
            var profile = new NounProfile ();
            profile.AddNewNoun ("Country", "Name of a country.", true);
            profile.AddNewNoun ("City", "Name of a city.", true);
            profile.AddNewNoun ("District", "Name of a district.", true);
            profile.AddNewNoun ("Park", "Name of a park.", true);
            profile.AddSuggestion ("Country", "Japan", new NounSuggestionDependency [0]);
            profile.AddSuggestion ("City", "Tokyo", new NounSuggestionDependency [1] { new NounSuggestionDependency ("Country", "Japan") });
            profile.AddSuggestion ("District", "Ginza", new NounSuggestionDependency [1] { new NounSuggestionDependency ("City", "Tokyo") });
            profile.AddSuggestion ("Park", "Yoyogi Park", new NounSuggestionDependency [2] { new NounSuggestionDependency ("Country", "Japan"), new NounSuggestionDependency ("City", "Tokyo") });
            profile.AddSuggestion ("Country", "United Kingdom", new NounSuggestionDependency [0]);
            profile.AddSuggestion ("City", "London", new NounSuggestionDependency [1] { new NounSuggestionDependency ("Country", "United Kingdom") });
            profile.AddSuggestion ("District", "Covent Garden", new NounSuggestionDependency [1] { new NounSuggestionDependency ("City", "London") });
            profile.AddSuggestion ("Park", "Hyde Park", new NounSuggestionDependency [2] { new NounSuggestionDependency ("Country", "United Kingdom"), new NounSuggestionDependency ("City", "London") });
            profile.DeleteSuggestion ("District", "Ginza");

            var existingDependenciesPark = profile.GetExistingDependencies ("Park").ToArray ();
            var expectedDependenciesPark = new string [] { "Country", "City" };

            var existingDependenciesDistrict = profile.GetExistingDependencies ("District").ToArray ();
            var expectedDependenciesDistrict = new string [] { "Country", "City" };

            var existingDependenciesCity = profile.GetExistingDependencies ("City").ToArray ();
            var expectedDependenciesCity = new string [] { "Country" };

            var existingDependenciesCountry = profile.GetExistingDependencies ("Country").ToArray ();
            var expectedDependenciesCountry = new string [] { };

            Assert.AreEqual (expectedDependenciesPark, existingDependenciesPark);
            Assert.AreEqual (expectedDependenciesDistrict, existingDependenciesDistrict);
            Assert.AreEqual (expectedDependenciesCity, existingDependenciesCity);
            Assert.AreEqual (expectedDependenciesCountry, existingDependenciesCountry);
        }

        [Test]
        public void TestTransitiveDependencyRoute_AllowedNewDependencies ()
        {
            var profile = new NounProfile ();
            profile.AddNewNoun ("Country", "Name of a country.", true);
            profile.AddNewNoun ("City", "Name of a city.", true);
            profile.AddNewNoun ("District", "Name of a district.", true);
            profile.AddNewNoun ("Park", "Name of a park.", true);
            profile.AddSuggestion ("Country", "Japan", new NounSuggestionDependency [0]);
            profile.AddSuggestion ("City", "Tokyo", new NounSuggestionDependency [1] { new NounSuggestionDependency ("Country", "Japan") });
            profile.AddSuggestion ("District", "Ginza", new NounSuggestionDependency [1] { new NounSuggestionDependency ("City", "Tokyo") });
            profile.AddSuggestion ("Park", "Yoyogi Park", new NounSuggestionDependency [2] { new NounSuggestionDependency ("Country", "Japan"), new NounSuggestionDependency ("City", "Tokyo") });

            var allowedDependenciesPark = profile.GetAllowedNewDependencies ("Park").ToArray ();
            var expectedDependenciesPark = new string [] { "District" };

            var allowedDependenciesDistrict = profile.GetAllowedNewDependencies ("District").ToArray ();
            var expectedDependenciesDistrict = new string [] { "Park" };

            var allowedDependenciesCity = profile.GetAllowedNewDependencies ("City").ToArray ();
            var expectedDependenciesCity = new string [] { };

            var allowedDependenciesCountry = profile.GetAllowedNewDependencies ("Country").ToArray ();
            var expectedDependenciesCountry = new string [] { };

            Assert.AreEqual (expectedDependenciesPark, allowedDependenciesPark);
            Assert.AreEqual (expectedDependenciesDistrict, allowedDependenciesDistrict);
            Assert.AreEqual (expectedDependenciesCity, allowedDependenciesCity);
            Assert.AreEqual (expectedDependenciesCountry, allowedDependenciesCountry);
        }

        [Test]
        public void TestTransitiveDependencyRoute_DeleteMediatorNoun_TransitiveGone ()
        {
            var profile = new NounProfile ();
            profile.AddNewNoun ("Country", "Name of a country.", true);
            profile.AddNewNoun ("City", "Name of a city.", true);
            profile.AddNewNoun ("District", "Name of a district.", true);
            profile.AddNewNoun ("Park", "Name of a park.", true);
            profile.AddSuggestion ("Country", "Japan", new NounSuggestionDependency [0]);
            profile.AddSuggestion ("City", "Tokyo", new NounSuggestionDependency [1] { new NounSuggestionDependency ("Country", "Japan") });
            profile.AddSuggestion ("District", "Ginza", new NounSuggestionDependency [1] { new NounSuggestionDependency ("City", "Tokyo") });
            profile.AddSuggestion ("Park", "Yoyogi Park", new NounSuggestionDependency [2] { new NounSuggestionDependency ("Country", "Japan"), new NounSuggestionDependency ("City", "Tokyo") });
            profile.DeleteNoun ("City");

            var existingDependenciesPark = profile.GetExistingDependencies ("Park").ToArray ();
            var expectedDependenciesPark = new string [] { "Country" };

            var existingDependenciesDistrict = profile.GetExistingDependencies ("District").ToArray ();
            var expectedDependenciesDistrict = new string [] { };

            var existingDependenciesCountry = profile.GetExistingDependencies ("Country").ToArray ();
            var expectedDependenciesCountry = new string [] { };

            Assert.AreEqual (expectedDependenciesPark, existingDependenciesPark);
            Assert.AreEqual (expectedDependenciesDistrict, existingDependenciesDistrict);
            Assert.AreEqual (expectedDependenciesCountry, existingDependenciesCountry);
        }
    }
}
