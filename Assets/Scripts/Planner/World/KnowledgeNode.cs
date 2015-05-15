using System;
using System.Collections.Generic;

namespace GOAP
{
    public interface IKnowledge
    {
        bool Contains(string key);
        IKnowledge this[string key] { get; set; }
    }
    public class KnowledgeNode : IKnowledge
    {
        public IKnowledge this[string key]
        {
            get { return Knowledge[key]; }
            set
            {
                if (Knowledge.ContainsKey(key))
                    Knowledge[key] = value;
                else
                    Knowledge.Add(key, value);
            }
        }

        public Dictionary<string, IKnowledge> Knowledge;
        public int Depth;

        public KnowledgeNode()
        {
            Knowledge = new Dictionary<string, IKnowledge>();
        }

        //deep copy constructor
        public KnowledgeNode(KnowledgeNode otherNode)
        {
            Knowledge = new Dictionary<string, IKnowledge>();
            foreach (var otherKnowledge in otherNode.Knowledge)
            {
                var key = otherKnowledge.Key;
                var value = otherKnowledge.Value;
                if (value is KnowledgeNode)
                    Knowledge.Add(key, new KnowledgeNode(value as KnowledgeNode));
                else
                    Knowledge.Add(key, value);
            }
        }

        public bool TryGetValue<T>(out T value, params string[] keyPath)
        {
            var knowledgeLeaf = GetKnowledge(keyPath) as KnowledgeLeaf<T>;
            if (knowledgeLeaf == null)
            {
                value = default(T);
                return false;
            }

            value = knowledgeLeaf.Value;
            return true;
        }

        public void SetValue<T>(T value, params string[] keyPath)
        {
            IKnowledge currentLevelKnowledge = this;
            for (var i = 0; i < keyPath.Length; i++)
            {
                var key = keyPath[i];
                if (i == keyPath.Length - 1)
                {
                    if (currentLevelKnowledge.Contains(key))
                    {
                        var knowledgeLeaf = currentLevelKnowledge[key] as KnowledgeLeaf<T>;
                        if (knowledgeLeaf != null)
                            knowledgeLeaf.Value = value;
                        else
                            currentLevelKnowledge[key] = new KnowledgeLeaf<T>(value);
                    }
                    else
                        currentLevelKnowledge[key] = new KnowledgeLeaf<T>(value);
                }
                else
                {
                    if (currentLevelKnowledge.Contains(key))
                        currentLevelKnowledge = currentLevelKnowledge[key];
                    else
                    {
                        currentLevelKnowledge[key] = new KnowledgeNode();
                        currentLevelKnowledge = currentLevelKnowledge[key];
                    }
                }
            }
        }

        public bool Contains(string key)
        {
            return Knowledge.ContainsKey(key);
        }

        private IKnowledge GetKnowledge(params string[] keyPath)
        {
            IKnowledge resultKnowledge = this;
            for (var i = 0; i < keyPath.Length; i++)
            {
                var key = keyPath[i];
                if (!resultKnowledge.Contains(key))
                    return null;
                resultKnowledge = resultKnowledge[key];
            }

            return resultKnowledge;
        }
    }

    public class KnowledgeLeaf<T> : IKnowledge
    {
        public T Value { get; set; }

        public KnowledgeLeaf(T value)
        {
            Value = value;
        }

        public bool Contains(string key)
        {
            return false;
        }

        public IKnowledge this[string key]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }
    }
}