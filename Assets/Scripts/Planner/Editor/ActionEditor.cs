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
    private Vector2 _scrollPosition;

    void Init()
    {
        _newEntry = string.Empty;
        _entriesList = new List<string>();

        foreach (var action in Enum.GetValues(typeof (EActionType)))
            _entriesList.Add(action.ToString());
    }

    void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        
        DrawEntries();
        AddEntry();
        Generate();

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
        var delete = GUILayout.Button("-", GUILayout.Width(20), GUILayout.Height(20));
        if (delete)
            _entriesList.Remove(action);
        
        EditorGUILayout.EndHorizontal();
    }

    private void Generate()
    {
        var generate = GUILayout.Button("Generate new actions enum");
        if (generate)
            OnGenerate();
    }

    private void OnGenerate()
    {
        var fileContent = File.ReadAllText(@"Assets\Scripts\Planner\Actions\Test.txt");
        var newEnumVals = new StringBuilder("{");

        for (int i = 0; i < _entriesList.Count; i++)
        {
            var entry = i < _entriesList.Count - 1 ? "\"" + _entriesList[i] + "\", " : "\"" + _entriesList[i] + "\"";
            newEnumVals.Append(entry);
        }
        newEnumVals.Append("}");

        var pattern = "{.*}";
        var regex = new Regex(pattern);

        File.WriteAllText(@"Assets\Scripts\Planner\Actions\Test.txt", regex.Replace(fileContent, newEnumVals.ToString()));
    }
}
