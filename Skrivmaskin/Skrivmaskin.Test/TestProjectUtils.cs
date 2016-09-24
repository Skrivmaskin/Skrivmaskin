using System;
using System.Collections.Generic;
using Skrivmaskin.Design;

namespace Skrivmaskin.Test
{
    public enum TestProjects
    {
        Empty,
        OneVariable,
        ParagraphBreak
    }

    public static class TestProjectUtils
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

        public static IDictionary<TestProjects, Tuple<Project, string>> SetupProjects ()
        {
            var projects = new Dictionary<TestProjects, Tuple<Project, string>> ();

            // Empty project
            var emptyProject = new Project (nameof (TestProjects.Empty), new List<Variable> (), new ChoiceNode ());
            var emptyExpected = "{\n  \"ProjectName\": \"Empty\",\n  \"VariableDefinitions\": [],\n  \"Definition\": {\n    \"ChoiceName\": \"\",\n    \"Choices\": []\n  }\n}";
            projects.Add (TestProjects.Empty, new Tuple<Project, string> (emptyProject, emptyExpected));

            // One variable
            var oneVariableProjectVariableDefinitions = new List<Variable> ();
            oneVariableProjectVariableDefinitions.Add (variable1);
            var oneVariableProject = new Project (nameof (TestProjects.OneVariable), oneVariableProjectVariableDefinitions, new ChoiceNode ());
            var oneVariableExpected = "{\n  \"ProjectName\": \"OneVariable\",\n  \"VariableDefinitions\": [\n    {\n      \"Name\": \"City\",\n      \"Description\": \"Where do you live?\",\n      \"Forms\": [\n        {\n          \"Name\": \"\",\n          \"Suggestion\": \"London\"\n        }\n      ]\n    }\n  ],\n  \"Definition\": {\n    \"ChoiceName\": \"\",\n    \"Choices\": []\n  }\n}";
            projects.Add (TestProjects.OneVariable, new Tuple<Project, string> (oneVariableProject, oneVariableExpected));

            // Paragraph break.
            var paragraphBreakProject = new Project (nameof (TestProjects.ParagraphBreak), new List<Variable>(), new ParagraphBreakNode ());
            var paragraphBreakExpected = "{\n  \"ProjectName\": \"ParagraphBreak\",\n  \"VariableDefinitions\": [],\n  \"Definition\": {}\n}";
            projects.Add (TestProjects.ParagraphBreak, new Tuple<Project, string> (paragraphBreakProject, paragraphBreakExpected));
            return projects;
        }
    }
}