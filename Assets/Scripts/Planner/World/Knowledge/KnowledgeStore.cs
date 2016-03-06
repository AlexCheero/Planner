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

        public void Add<T>(string[] keyPath, T value, float dispersion = -1, bool rewrite = false)
        {
            var key = MakeKey(keyPath);
            if (_knowledge.ContainsKey(key))
            {
                if (rewrite)
                    ((KnowledgeObject<T>) _knowledge[key]).Value = value;
            }
            else
                _knowledge.Add(key, new KnowledgeObject<T>(value));
        }

        public void Add<T>(string key, T value, float dispersion = -1, bool rewrite = false)
        {
            Add(new[] { key }, value, dispersion, rewrite);
        }

        public bool TryGetKnowledge<T>(string[] keyPath, ref T knowlValue)
        {
            var key = MakeKey(keyPath);
            if (!_knowledge.ContainsKey(key))
                return false;

            knowlValue = ((KnowledgeObject<T>)_knowledge[key]).Value;
            return true;
        }

        public bool ApproxEquals(KnowledgeStore otherKnowledge)
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

        //internal classes
        private interface IKnowlObj { }

        private class KnowledgeObject<T> : IKnowlObj
        {
            internal T Value;
            internal float Dispersion;

            internal KnowledgeObject (T value, float dispersion = -1)
            {
                Value = value;
                Dispersion = dispersion;
            }
        }
    }
}