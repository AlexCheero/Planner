using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class BoolImpaction : KnowledgeImpaction
    {
        public bool Value;

        public BoolImpaction(string path, bool value, EImpaction impaction)
        {
            Path = path;
            Value = value;
            Impaction = impaction;
        }

        public override void ImpactKnowledge(ref Dictionary<string, object> knowledge)
        {
            if (knowledge.ContainsKey(Path))
            {
                var currentValue = knowledge[Path];
                if (!(currentValue is bool))
                {
                    Debug.LogError("invalid knowledge type");
                    return;
                }
                var castedCurrentValue = (bool)currentValue;
                switch (Impaction)
                {
                    case EImpaction.Replace:
                        knowledge[Path] = Value;
                        break;
                }
            }
            else
            {
                switch (Impaction)
                {
                    case EImpaction.Replace:
                        knowledge.Add(Path, Value);
                        break;
                }
            }
        }
    }
}