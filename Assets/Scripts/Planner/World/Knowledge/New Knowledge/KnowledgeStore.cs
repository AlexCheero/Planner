using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace GOAP
{
    internal class KnowledgeStore
    {
        private readonly Dictionary<string, IKnowlObj> _knowledge;

        public KnowledgeStore()
        {
            _knowledge = new Dictionary<string, IKnowlObj>();
        }

        public IKnowlObj this[string key]
        {
            get { return _knowledge[key]; }
            set { _knowledge[key] = value; }
        }

        public void Add<T>(string[] keyPath, KnowledgeObject<T> value, bool rewrite = false)
        {
            var key = MakeKey(keyPath);
            if (_knowledge.ContainsKey(key))
            {
                if (rewrite)
                    _knowledge[key] = value;
            }
            else
                _knowledge.Add(key, value);
        }

        public bool TryGetKnowledge<T>(string[] keyPath, ref T knowlValue)
        {
            var key = MakeKey(keyPath);
            if (!_knowledge.ContainsKey(key))
                return false;

#if UNITY_EDITOR
            var knowl = _knowledge[key] as KnowledgeObject<T>;
            if (knowl == null)
            {
                Debug.LogError("trying to get wrong knowlege type");
                return false;
            }
#endif
            knowlValue = ((KnowledgeObject<T>)_knowledge[key]).Value;
            return true;
        }

        public bool ApproxEquals(KnowledgeStore otherKnowledge, int importance = 2, params string[] subDirectory)
        {
            var subDir = MakeKey(subDirectory);
            foreach (var knowl in _knowledge)
            {
                if (knowl.Value.Importance < importance || !knowl.Key.Contains(subDir))
                    continue;

                if (knowl.Value.ApproxEquals(otherKnowledge[knowl.Key]))
                    return false;
            }

            return true;
        }

        private string MakeKey(string[] keyPath)
        {
            var keyBuilder = new StringBuilder();
            for (int i = 0; i < keyPath.Length; i++)
                keyBuilder.Append(keyPath[i] + "/");

            return keyBuilder.ToString();
        }
    }
}