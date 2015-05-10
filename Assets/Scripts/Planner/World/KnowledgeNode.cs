using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public interface IKnowledge
    {
        T GetKnowledge<T>(params string[] keyPath);
    }
    public class KnowledgeNode : IKnowledge
    {
        public Dictionary<string, IKnowledge> Knowledge;
        public int Depth;

        public KnowledgeNode()
        {
            Knowledge = new Dictionary<string, IKnowledge>();
        }

        public T GetKnowledge<T>(params string[] keyPath)
        {
            if (keyPath.Length <= 0)
            {
                Debug.LogError("trying to get value from node");
                return default(T);
            }
            IKnowledge resultKnowledge = this;
            foreach (var key in keyPath)
            {
                if (Knowledge.ContainsKey(key))
                    resultKnowledge = Knowledge[key];
                else
                {
                    Debug.LogError("invalid key path");
                    return default(T);
                }
            }
            var knowledge = (resultKnowledge as KnowledgeLeaf<T>);
            return knowledge == null ? default(T) : knowledge.GetValue();
        }
    }

    public class KnowledgeLeaf<T1> : IKnowledge
    {
        public T1 Value;
        public T2 GetKnowledge<T2>(params string[] keyPath)
        {
            Debug.LogError("trying to get next depth knowledge from leaf");
            return default(T2);
        }

        public T1 GetValue()
        {
            return Value;
        }
    }
}