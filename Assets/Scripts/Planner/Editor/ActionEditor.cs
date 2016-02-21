using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using GOAP;

public class ActionEditor : EditorWindow
{
    [MenuItem("Tools/PlannerTools/ActionEditor")]
    public static void ShowWindow()
    {
        var window = GetWindow(typeof(ActionEditor));
        ((ActionEditor) window).Init();
    }

    private string _newEntry;
    private List<string> _entriesList;
    private List<string> _entriesToDelete;
    private Vector2 _scrollPosition;

    void Init()
    {
        _newEntry = string.Empty;
        _entriesList = new List<string>();
        _entriesToDelete = new List<string>();

        foreach (var action in Enum.GetValues(typeof (EActionType)))
            _entriesList.Add(action.ToString());
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        
        //rewrite this, drag all the gui functions to this method, so unity won't swear
        DrawEntries();
        AddEntry();
        Generate();
        Reinit();

        EditorGUILayout.EndVertical();
    }

    private void AddEntry()
    {
        _newEntry = GUILayout.TextField(_newEntry);
        var add = GUILayout.Button("+");
        if (add && !string.IsNullOrEmpty(_newEntry) && !_entriesList.Contains(_newEntry))
        {
            _entriesList.Add(_newEntry);
            _newEntry = string.Empty;
        }
    }

    private void DrawEntries()
    {
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        for (int i = 0; i < _entriesList.Count; i++)
            DrawEntry(_entriesList[i]);
        EditorGUILayout.EndScrollView();
    }

    private void DrawEntry(string action)
    {
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label(action);
        DeleteButton(action);
        EditorGUILayout.EndHorizontal();
    }

    private void DeleteButton(string action)
    {
        var delete = GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20));
        if (!delete)
            return;
        _entriesList.Remove(action);
        _entriesToDelete.Add(action);
    }

    private void Generate()
    {
        var generate = GUILayout.Button("Generate");
        if (generate)
            OnGenerate();
    }

    private void Reinit()
    {
        var reset = GUILayout.Button("Reinit");
        if (reset)
            Init();
    }

    private const string EnumFilePath = @"Assets\Scripts\Planner\ActionStuff\Actions\EActionType.cs";
    private void OnGenerate()
    {
        GenerateEnum();
        foreach (var entry in _entriesList)
            GenerateFactory(entry);
        foreach (var entry in _entriesList)
            GenerateAction(entry);
        
    }

    private void GenerateEnum()
    {
        var fileContent = File.ReadAllText(EnumFilePath);
        var newEnumVals = new StringBuilder("EActionType\n\t{\n");

        for (int i = 0; i < _entriesList.Count; i++)
        {
            var entry = i < _entriesList.Count - 1 ? _entriesList[i] + ",\n" : _entriesList[i] + "\n";
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
}
