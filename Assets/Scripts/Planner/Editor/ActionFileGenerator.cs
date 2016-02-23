using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;

namespace GOAPEditor
{
    public class ActionFileGenerator
    {
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

        private List<string> _entriesList;
        private List<string> _entriesToDelete;

        public void OnGenerate(List<string> entries, List<string> entriesToDelete)
        {
            _entriesList = entries;
            _entriesToDelete = entriesToDelete;

            DeleteFiles();
            GenerateEnum();
            RewriteActionBoard();
            foreach (var entry in _entriesList)
            {
                GenerateFactory(entry);
                GenerateAction(entry);
            }
        }

        private void GenerateEnum()
        {
            RewriteFile(EnumFilePath, "EActionType");
        }

        private void RewriteActionBoard()
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

            for (int i = 0; i < _entriesList.Count; i++)
            {
                var factory = _entriesList[i] + append;
                if (i < _entriesList.Count - 1)
                    factory += ",\n";
                else
                    factory += "\n";
                newFactories.Append(innerIndent + factory);
            }
            newFactories.Append(outerIndent + "}");

            var pattern = tag + @"\s*{(.|\n)*?}";
            WriteFile(pattern, path, fileContent, newFactories.ToString());
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
            GenerateNewFile(action + FactoryPostfix, FactoriesFolderPath, FactoryTemplatePath, ActionFactoryTemplate);
        }

        private void GenerateAction(string action)
        {
            GenerateNewFile(action + ActionPostfix, GeneratedActionsFolderPath, ActionTemplatePath, PlannerActionTemplate);
        }

        private void GenerateNewFile(string actionWithPostfix, string targetFolderPath, string templatePath, string pattern)
        {
            var fileName = actionWithPostfix;
            var fullPathWithExtension = targetFolderPath + fileName + ".cs";
            if (File.Exists(fullPathWithExtension))
                return;

            var templateContent = File.ReadAllText(templatePath);
            WriteFile(pattern, fullPathWithExtension, templateContent, fileName);
        }

        private void WriteFile(string pattern, string filePath, string templateContent, string replacement)
        {
            var regex = new Regex(pattern);
            File.WriteAllText(filePath, regex.Replace(templateContent, replacement));
            AssetDatabase.ImportAsset(filePath);
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

        private struct FileGenData
        {
            public string Pattern;
            public string FilePath;
            public string TemplateContent;
            public string Replacement;
        }
    }
}