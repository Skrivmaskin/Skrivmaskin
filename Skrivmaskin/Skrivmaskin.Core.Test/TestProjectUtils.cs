using System;
using System.Collections.Generic;
using Skrivmaskin.Core.Design;

namespace Skrivmaskin.Core.Test
{
    public enum TestProjects
    {
        Empty,
        OneVariable
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
            var emptyProject = new Project ();
            emptyProject.ProjectName = nameof (TestProjects.Empty);
            emptyProject.Definition = new ChoiceNode ();
            emptyProject.VariableDefinitions = new List<Variable> ();
            var emptyExpected = "{\n  \"ProjectName\": \"Empty\",\n  \"VariableDefinitions\": [],\n  \"Definition\": {\n    \"ChoiceName\": \"\",\n    \"Choices\": []\n  }\n}";
            projects.Add (TestProjects.Empty, new Tuple<Project, string> (emptyProject, emptyExpected));

            // One variable
            var oneVariableProject = new Project ();
            oneVariableProject.ProjectName = nameof (TestProjects.OneVariable);
            oneVariableProject.Definition = new ChoiceNode ();
            oneVariableProject.VariableDefinitions = new List<Variable> ();
            oneVariableProject.VariableDefinitions.Add (variable1);
            var oneVariableExpected = "{\n  \"ProjectName\": \"OneVariable\",\n  \"VariableDefinitions\": [\n    {\n      \"Name\": \"City\",\n      \"Description\": \"Where do you live?\",\n      \"Forms\": [\n        {\n          \"Name\": \"\",\n          \"Suggestion\": \"London\"\n        }\n      ]\n    }\n  ],\n  \"Definition\": {\n    \"ChoiceName\": \"\",\n    \"Choices\": []\n  }\n}";
            projects.Add (TestProjects.OneVariable, new Tuple<Project, string> (oneVariableProject, oneVariableExpected));

            return projects;
        }

    }
}