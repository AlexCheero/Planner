using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

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
        private const string ActionType = "EActionType Type";
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
            GenerateFiles();
        }

        private void GenerateFiles()
        {
            var datas = new List<FileGenData> {GenerateEnum(), RewriteActionBoard()};
            foreach (var entry in _entriesList)
            {
                var factory = GenerateFactory(entry);
                if (factory.FilePath != null && factory.Content != null)
                    datas.Add(factory);

                var action = GenerateAction(entry);
                if (action.FilePath != null && action.Content != null)
                {
                    action.Content = RewriteContent(action.Content, ActionType,
                        new List<string> {"get { return EActionType." + entry + "; }"}, "", 3);
                    datas.Add(action);
                }
            }

            foreach (var data in datas)
                WriteFile(data);
        }

        private FileGenData GenerateEnum()
        {
            var fileContent = File.ReadAllText(EnumFilePath);
            var content = RewriteContent(fileContent, "enum EActionType", _entriesList);

            return new FileGenData
            {
                Content = content,
                FilePath = EnumFilePath
            };
        }

        private FileGenData RewriteActionBoard()
        {
            var fileContent = File.ReadAllText(ActionBoardPath);
            var content = RewriteContent(fileContent, "_factories = new List<IActionFactory>", _entriesList, "ActionFactory.Instance", 3);

            return new FileGenData
            {
                Content = content,
                FilePath = ActionBoardPath
            };
        }

        private string RewriteContent(string fileContent, string tag, List<string> toRewrite, string append = "", int indentCount = 2)
        {
            var outerIndent = "";
            var innerIndent = "";
            MakeIndents(indentCount, ref outerIndent, ref innerIndent);

            var newItems = new StringBuilder(tag + "\n" + outerIndent + "{\n");

            for (int i = 0; i < toRewrite.Count; i++)
            {
                var item = toRewrite[i] + append;
                if (i < toRewrite.Count - 1)
                    item += ",\n";
                else
                    item += "\n";
                newItems.Append(innerIndent + item);
            }
            var pattern = newItems.ToString().Contains("}") ? tag + @"\s*{\s*get { throw new System.NotImplementedException\(\); }\s*}" : tag + @"\s*{(.|\n)*?}";
            newItems.Append(outerIndent + "}");

            var regex = new Regex(pattern);
            return regex.Replace(fileContent, newItems.ToString());
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

        private FileGenData GenerateFactory(string action)
        {
            return GenerateNewContent(action + FactoryPostfix, FactoriesFolderPath, FactoryTemplatePath, ActionFactoryTemplate);
        }

        private FileGenData GenerateAction(string action)
        {
            return GenerateNewContent(action + ActionPostfix, GeneratedActionsFolderPath, ActionTemplatePath, PlannerActionTemplate);
        }

        private FileGenData GenerateNewContent(string actionWithPostfix, string targetFolderPath, string templatePath, string pattern)
        {
            var fileName = actionWithPostfix;
            var fullPathWithExtension = targetFolderPath + fileName + ".cs";
            if (File.Exists(fullPathWithExtension))
                return new FileGenData();

            var templateContent = File.ReadAllText(templatePath);
            
            var regex = new Regex(pattern);
            var newContent = regex.Replace(templateContent, fileName);
            return new FileGenData
            {
                FilePath = fullPathWithExtension,
                Content = newContent
            };
        }

        private void WriteFile(string filePath, string newContent)
        {
            if (string.IsNullOrEmpty(filePath) || string.IsNullOrEmpty(newContent))
                return;

            File.WriteAllText(filePath, newContent);
            AssetDatabase.ImportAsset(filePath);
        }

        private void WriteFile(FileGenData data)
        {
            if (string.IsNullOrEmpty(data.FilePath) || string.IsNullOrEmpty(data.Content))
                return;

            File.WriteAllText(data.FilePath, data.Content);
            AssetDatabase.ImportAsset(data.FilePath);
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
            public string FilePath;
            public string Content;
        }
    }
}