using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class FloatImpaction : KnowledgeImpaction
    {
        public float Value;

        public FloatImpaction(string path, float value, EImpaction impaction)
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
                if (!(currentValue is float))
                {
                    Debug.LogError("invalid knowledge type");
                    return;
                }
                var castedCurrentValue = (float)currentValue;
                switch (Impaction)
                {
                    case EImpaction.Add:
                        knowledge[Path] = castedCurrentValue + Value;
                        break;
                    case EImpaction.Mult:
                        knowledge[Path] = castedCurrentValue * Value;
                        break;
                    case EImpaction.Replace:
                        knowledge[Path] = Value;
                        break;
                }
            }
            else
            {
                switch (Impaction)
                {
                    case EImpaction.Add:
                        knowledge.Add(Path, Value);
                        break;
                    case EImpaction.Replace:
                        knowledge.Add(Path, Value);
                        break;
                }
            }
        }
    }
}