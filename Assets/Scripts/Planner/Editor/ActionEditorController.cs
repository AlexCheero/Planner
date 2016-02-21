using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using GOAP;
using UnityEditor;

namespace GOAPEditor
{
    public class ActionEditorController
    {
        public List<string> EntriesList;
        private List<string> _entriesToDelete;
        public string NewEntry;

        public ActionEditorController()
        {
            Reset();
        }

        public void Reset()
        {
            EntriesList = new List<string>();
            _entriesToDelete = new List<string>();
            NewEntry = string.Empty;

            foreach (var action in Enum.GetValues(typeof (EActionType)))
                EntriesList.Add(action.ToString());
        }

        public void RemoveEntry(string entry)
        {
            EntriesList.Remove(entry);
            _entriesToDelete.Add(entry);
        }

        public void AddEntry()
        {
            EntriesList.Add(NewEntry);
            NewEntry = string.Empty;
        }

        private const string EnumFilePath = @"Assets\Scripts\Planner\ActionStuff\Actions\EActionType.cs";
        public void OnGenerate()
        {
            DeleteFiles();
            GenerateEnum();
            foreach (var entry in EntriesList)
                GenerateFactory(entry);
            foreach (var entry in EntriesList)
                GenerateAction(entry);
        }

        private void GenerateEnum()
        {
            var fileContent = File.ReadAllText(EnumFilePath);
            var newEnumVals = new StringBuilder("EActionType\n\t{\n");

            for (int i = 0; i < EntriesList.Count; i++)
            {
                var entry = i < EntriesList.Count - 1 ? EntriesList[i] + ",\n" : EntriesList[i] + "\n";
                newEnumVals.Append("\t\t" + entry);
            }
            newEnumVals.Append("\t}");

            var pattern = @"EActionType\s*{(.|\n)*?}";
            ReplaceInFile(pattern, EnumFilePath, fileContent, newEnumVals.ToString());
        }

        private const string FactoriesFolderPath = @"Assets\Scripts\Planner\ActionStuff\Factories\";
        private const string ActionsFolderPath = @"Assets\Scripts\GeneratedActions\";
        private const string TemplatesPath = @"Assets\Scripts\Planner\ActionStuff\Templates\";
        private const string ActionFactoryTemplate = "ActionFactoryTemplate";
        private const string PlannerActionTemplate = "PlannerActionTemplate";
        private const string FactoryTemplatePath = TemplatesPath + ActionFactoryTemplate + ".cs";
        private const string ActionTemplatePath = TemplatesPath + PlannerActionTemplate + ".cs";
        private const string ActionPostfix = "PlannerAction";
        private const string FactoryPostfix = "ActionFactory";
        private void GenerateFactory(string action)
        {
            var factoryName = action + FactoryPostfix;
            var fullPathWithExtension = FactoriesFolderPath + factoryName + ".cs";
            if (File.Exists(fullPathWithExtension))
                return;

            var templateContent = File.ReadAllText(FactoryTemplatePath);
            var pattern = ActionFactoryTemplate;
            ReplaceInFile(pattern, fullPathWithExtension, templateContent, factoryName);
        }

        private static void ReplaceInFile(string pattern, string filePath, string templateContent, string replacement)
        {
            var regex = new Regex(pattern);
            File.WriteAllText(filePath, regex.Replace(templateContent, replacement));
            AssetDatabase.ImportAsset(filePath);
        }

        private void GenerateAction(string action)
        {
            var actionName = action + ActionPostfix;
            var fullPathWithExtension = ActionsFolderPath + actionName + ".cs";
            if (File.Exists(fullPathWithExtension))
                return;

            var templateContent = File.ReadAllText(ActionTemplatePath);
            var pattern = PlannerActionTemplate;
            ReplaceInFile(pattern, fullPathWithExtension, templateContent, actionName);
        }

        private void DeleteFiles()
        {
            foreach (var entry in _entriesToDelete)
            {
                File.Delete(FactoriesFolderPath + entry + FactoryPostfix + ".cs");
                File.Delete(FactoriesFolderPath + entry + FactoryPostfix + ".meta");
                File.Delete(ActionsFolderPath + entry + ActionPostfix + ".cs");
                File.Delete(ActionsFolderPath + entry + ActionPostfix + ".meta");
            }
            _entriesToDelete.Clear();
            AssetDatabase.Refresh();
        }
    }
}