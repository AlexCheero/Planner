using System.Collections.Generic;
using System.Text;

namespace GOAP
{
    internal class KnowledgeStore
    {
        private Dictionary<string, IKnowlObj> _knowledge;

        public KnowledgeStore()
        {
            _knowledge = new Dictionary<string, IKnowlObj>();
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

        public void Add<T>(string key, KnowledgeObject<T> value, bool rewrite = false)
        {
            Add(new[] { key }, value, rewrite);
        }

        public bool TryGetKnowledge<T>(string[] keyPath, ref T knowlValue)
        {
            var key = MakeKey(keyPath);
            if (!_knowledge.ContainsKey(key))
                return false;

            knowlValue = ((KnowledgeObject<T>)_knowledge[key]).Value;
            return true;
        }

        public bool ApproxEquals(KnowledgeStore otherKnowledge, int importance = 2)
        {
            return false;
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