using System;
using System.Collections.Generic;
using TextOn.Design;

namespace TextOn.Test
{
    public enum TestTemplates
    {
        Empty,
        OneVariable,
        ParagraphBreak
    }

    public static class TestTemplateUtils
    {
        readonly static Variable variable1 = MakeVariable ("City", "Where do you live?", new string [1] { "" }, new string [1] { "London" });

        public static Variable MakeVariable (string name, string description, string [] formNames, string [] formSuggestions)
        {
            var variable = new Variable ();
            variable.Forms = new List<VariableForm> ();
            variable.Name = name;
            variable.Description = description;
            for (int i = 0; i < formNames.Length; i++) {
                var form = new VariableForm () { Name = formNames [i], Suggestion = formSuggestions [i] };
                variable.Forms.Add (form);
            }
            return variable;
        }

        public static IDictionary<TestTemplates, Tuple<TextOnTemplate, string>> SetupProjects ()
        {
            var projects = new Dictionary<TestTemplates, Tuple<TextOnTemplate, string>> ();

            // Empty project
            var emptyProject = new TextOnTemplate (new List<Variable> (), new ChoiceNode ());
            var emptyExpected = "{\n  \"VariableDefinitions\": [],\n  \"Definition\": {\n    \"ChoiceName\": \"\",\n    \"Choices\": []\n  }\n}";
            projects.Add (TestTemplates.Empty, new Tuple<TextOnTemplate, string> (emptyProject, emptyExpected));

            // One variable
            var oneVariableProjectVariableDefinitions = new List<Variable> ();
            oneVariableProjectVariableDefinitions.Add (variable1);
            var oneVariableProject = new TextOnTemplate (oneVariableProjectVariableDefinitions, new ChoiceNode ());
            var oneVariableExpected = "{\n  \"VariableDefinitions\": [\n    {\n      \"Name\": \"City\",\n      \"Description\": \"Where do you live?\",\n      \"Forms\": [\n        {\n          \"Name\": \"\",\n          \"Suggestion\": \"London\"\n        }\n      ]\n    }\n  ],\n  \"Definition\": {\n    \"ChoiceName\": \"\",\n    \"Choices\": []\n  }\n}";
            projects.Add (TestTemplates.OneVariable, new Tuple<TextOnTemplate, string> (oneVariableProject, oneVariableExpected));

            // Paragraph break.
            var paragraphBreakProject = new TextOnTemplate (new List<Variable>(), new ParagraphBreakNode ());
            var paragraphBreakExpected = "{\n  \"VariableDefinitions\": [],\n  \"Definition\": {}\n}";
            projects.Add (TestTemplates.ParagraphBreak, new Tuple<TextOnTemplate, string> (paragraphBreakProject, paragraphBreakExpected));
            return projects;
        }
    }
}