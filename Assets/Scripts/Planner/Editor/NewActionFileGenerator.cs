using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace GOAPEditor
{
    public class NewActionFileGenerator
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