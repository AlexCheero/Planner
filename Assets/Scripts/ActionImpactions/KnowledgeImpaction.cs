using System.Collections.Generic;

namespace GOAP
{
    public abstract class KnowledgeImpaction
    {
        public string Path;
        public EImpaction Impaction;

        public abstract void ImpactKnowledge(ref Dictionary<string, object> knowledge);
    }
}