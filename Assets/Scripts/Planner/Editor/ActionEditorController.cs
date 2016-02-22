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

        private const string ActionStuffPath = @"Assets\Scripts\Planner\ActionStuff\";
        private const string ActionsPath = ActionStuffPath + @"Actions\";
        private const string ActionBoardPath = ActionsPath + "ActionBoard.cs";
        private const string EnumFilePath = ActionsPath + @"EActionType.cs";
        private const string FactoriesFolderPath = ActionStuffPath + @"Factories\";
        private const string GeneratedActionsFolderPath = @"Assets\Scripts\GeneratedActions\";
        private const string TemplatesPath = ActionStuffPath + @"Templates\";
        private const string ActionFactoryTemplate = "ActionFactoryTemplate";
        private const string PlannerActionTemplate = "PlannerActionTemplate";
        private const string FactoryTemplatePath = TemplatesPath + ActionFactoryTemplate + ".cs";
        private const string ActionTemplatePath = TemplatesPath + PlannerActionTemplate + ".cs";
        private const string ActionPostfix = "PlannerAction";
        private const string FactoryPostfix = "ActionFactory";
        public void OnGenerate()
        {
            DeleteFiles();
            GenerateEnum();
            foreach (var entry in EntriesList)
            {
                GenerateFactory(entry);
                GenerateAction(entry);
                RewriteActionBoard(entry);
            }
        }

        private void GenerateEnum()
        {
            RewriteFile(EnumFilePath, "EActionType");
        }

        private void RewriteActionBoard(string action)
        {
            RewriteFile(ActionBoardPath, "_factories = new List<IActionFactory>", "ActionFactory.Instance", 3);
        }

        private void RewriteFile(string path, string tag, string append = "", int indentCount = 2)
        {
            var fileContent = File.ReadAllText(path);
            
            var outerIndent = "";
            var innerIndent = "";
            MakeIndents(indentCount, ref outerIndent, ref innerIndent);

            var newFactories = new StringBuilder(tag + "\n" + outerIndent + "{\n");

            for (int i = 0; i < EntriesList.Count; i++)
            {
                var factory = EntriesList[i] + append;
                if (i < EntriesList.Count - 1)
                    factory += ",\n";
                else
                    factory += "\n";
                newFactories.Append(innerIndent + factory);
            }
            newFactories.Append(outerIndent + "}");

            var pattern = tag + @"\s*{(.|\n)*?}";
            ReplaceInFile(pattern, path, fileContent, newFactories.ToString());
        }

        private void MakeIndents(int indentCount, ref string outerIndent, ref string innerIndent)
        {
            outerIndent = "";
            innerIndent = "";

            if (indentCount <= 0)
                return;

            for (int i = 0; i < indentCount - 1; i++)
            {
                outerIndent += "\t";
                innerIndent += "\t";
            }

            innerIndent += "\t";
        }

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
            var fullPathWithExtension = GeneratedActionsFolderPath + actionName + ".cs";
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
                var factoryPath = FactoriesFolderPath + entry + FactoryPostfix;
                var actionPath = GeneratedActionsFolderPath + entry + ActionPostfix;

                File.Delete(factoryPath + ".cs");
                File.Delete(factoryPath + ".meta");

                File.Delete(actionPath + ".cs");
                File.Delete(actionPath + ".meta");
            }
            _entriesToDelete.Clear();
            AssetDatabase.Refresh();
        }
    }
}