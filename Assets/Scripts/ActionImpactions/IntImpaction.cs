using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class IntImpaction : KnowledgeImpaction
    {
        public float Value;

        public IntImpaction(string path, float value, EImpaction impaction)
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
                if (!(currentValue is int))
                {
                    Debug.LogError("invalid knowledge type");
                    return;
                }
                var castedCurrentValue = (int) currentValue;
                switch (Impaction)
                {
                    case EImpaction.Add:
                        knowledge[Path] = (int)(castedCurrentValue + Value);
                        break;
                    case EImpaction.Mult:
                        knowledge[Path] = (int)(castedCurrentValue * Value);
                        break;
                    case EImpaction.Replace:
                        knowledge[Path] = (int)(Value);
                        break;
                }
            }
            else
            {
                switch (Impaction)
                {
                    case EImpaction.Add:
                        knowledge.Add(Path, (int)Value);
                        break;
                    case EImpaction.Replace:
                        knowledge.Add(Path, (int)Value);
                        break;
                }
            }
        }
    }

    public class StringImpaction : KnowledgeImpaction
    {
        public string Value;

        public StringImpaction(string path, string value, EImpaction impaction)
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
                if (!(currentValue is string))
                {
                    Debug.LogError("invalid knowledge type");
                    return;
                }
                var castedCurrentValue = (string)currentValue;
                switch (Impaction)
                {
                    case EImpaction.Add:
                        knowledge[Path] = string.Concat(castedCurrentValue, Value);
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
