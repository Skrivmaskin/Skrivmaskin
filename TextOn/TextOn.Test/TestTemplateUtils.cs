using System;
using System.Collections.Generic;
using TextOn.Design;
using TextOn.Nouns;
using TextOn.Version0;

namespace TextOn.Test
{
    public enum TestTemplates
    {
        Empty,
        ParagraphBreak,
		OneNoun,
        TwoNouns
    }

    public static class TestTemplateUtils
    {
        public static IDictionary<TestTemplates, Tuple<TextOnTemplate, string>> SetupProjects ()
        {
            var templates = new Dictionary<TestTemplates, Tuple<TextOnTemplate, string>> ();

            // Empty project
            var emptyTemplate = new TextOnTemplate (new NounProfile (), new DesignNode (NodeType.Choice, true, "", new DesignNode [0]));
            var emptyExpected = "{\n  \"Nouns\": [],\n  \"Definition\": {\n    \"ChoiceName\": \"\",\n    \"Choices\": []\n  },\n  \"Version\": 1\n}";
            templates.Add (TestTemplates.Empty, new Tuple<TextOnTemplate, string> (emptyTemplate, emptyExpected));

            // Paragraph break.
            var paragraphBreakTemplate = new TextOnTemplate (new NounProfile (), new DesignNode (NodeType.ParagraphBreak, true, "", new DesignNode [0]));
            var paragraphBreakExpected = "{\n  \"Nouns\": [],\n  \"Definition\": {},\n  \"Version\": 1\n}";
            templates.Add (TestTemplates.ParagraphBreak, new Tuple<TextOnTemplate, string> (paragraphBreakTemplate, paragraphBreakExpected));

            // One noun.
            var oneNounProfile = new NounProfile ();
            oneNounProfile.AddNewNoun ("City", "Name of a city.", true);
            var oneNounTemplate = new TextOnTemplate (oneNounProfile, new DesignNode (NodeType.Sequential, true, "Sequential", new DesignNode [0]));
            var oneNounExpected = "{\n  \"Nouns\": [\n    {\n      \"Name\": \"City\",\n      \"Description\": \"Name of a city.\",\n      \"AcceptsUserValue\": true,\n      \"Suggestions\": []\n    }\n  ],\n  \"Definition\": {\n    \"SequentialName\": \"Sequential\",\n    \"Sequential\": []\n  },\n  \"Version\": 1\n}";
            templates.Add (TestTemplates.OneNoun, new Tuple<TextOnTemplate, string> (oneNounTemplate, oneNounExpected));

            // Two nouns, with a suggestion and a dependency.
            var twoNounsProfile = new NounProfile ();
            twoNounsProfile.AddNewNoun ("Country", "Name of a country.", true);
            twoNounsProfile.AddNewNoun ("City", "Name of a city.", true);
            twoNounsProfile.AddSuggestion ("Country", "Japan", new NounSuggestionDependency [0]);
            twoNounsProfile.AddSuggestion ("City", "Tokyo", new NounSuggestionDependency [1] { new NounSuggestionDependency ("Country", "Japan") });
            var twoNounsTemplate = new TextOnTemplate (twoNounsProfile, new DesignNode (NodeType.Sequential, true, "Sequential", new DesignNode [0]));
            var twoNounsExpected = "{\n  \"Nouns\": [\n    {\n      \"Name\": \"Country\",\n      \"Description\": \"Name of a country.\",\n      \"AcceptsUserValue\": true,\n      \"Suggestions\": [\n        {\n          \"Value\": \"Japan\",\n          \"Dependencies\": []\n        }\n      ]\n    },\n    {\n      \"Name\": \"City\",\n      \"Description\": \"Name of a city.\",\n      \"AcceptsUserValue\": true,\n      \"Suggestions\": [\n        {\n          \"Value\": \"Tokyo\",\n          \"Dependencies\": [\n            {\n              \"Name\": \"Country\",\n              \"Value\": \"Japan\"\n            }\n          ]\n        }\n      ]\n    }\n  ],\n  \"Definition\": {\n    \"SequentialName\": \"Sequential\",\n    \"Sequential\": []\n  },\n  \"Version\": 1\n}";
            templates.Add (TestTemplates.TwoNouns, new Tuple<TextOnTemplate, string> (twoNounsTemplate, twoNounsExpected));

            return templates;
        }
    }
}