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
        public string NewEntry;
        
        private List<string> _entriesToDelete;
        private ActionFileGenerator _fileGenerator;


        public ActionEditorController()
        {
            _fileGenerator = new ActionFileGenerator();
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

        public void OnGenerate()
        {
            _fileGenerator.OnGenerate(EntriesList, _entriesToDelete);
        }
    }
}