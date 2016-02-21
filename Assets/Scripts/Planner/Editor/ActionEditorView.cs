using UnityEditor;
using UnityEngine;

namespace GOAPEditor
{
    public class ActionEditorView : EditorWindow
    {
        [MenuItem("Tools/PlannerTools/ActionEditor")]
        public static void ShowWindow()
        {
            GetWindow(typeof(ActionEditorView));
        }

        private ActionEditorController _controller;

        private ActionEditorController Controller
        {
            get { return _controller ?? (_controller = new ActionEditorController()); }
        }

        private Vector2 _scrollPosition;

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            DrawEntries();
            AddEntry();
            Generate();
            Reinit();

            EditorGUILayout.EndVertical();
        }

        private void AddEntry()
        {
            Controller.NewEntry = GUILayout.TextField(Controller.NewEntry);
            var add = GUILayout.Button("+");
            if (!add || string.IsNullOrEmpty(Controller.NewEntry) ||
                Controller.EntriesList.Contains(Controller.NewEntry))
                return;

            Controller.AddEntry();
        }

        private void DrawEntries()
        {
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            for (int i = 0; i < Controller.EntriesList.Count; i++)
                DrawEntry(Controller.EntriesList[i]);
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
            Controller.RemoveEntry(action);
        }

        private void Generate()
        {
            var generate = GUILayout.Button("Generate");
            if (generate)
                Controller.OnGenerate();
        }

        private void Reinit()
        {
            var reset = GUILayout.Button("Reinit");
            if (reset)
                Controller.Reset();
        }
    }
}