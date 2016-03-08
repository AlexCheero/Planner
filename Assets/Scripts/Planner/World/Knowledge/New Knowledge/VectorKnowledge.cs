using UnityEngine;

namespace GOAP
{
    internal class VectorKnowledge : KnowledgeObject<Vector3>
    {
        public VectorKnowledge(Vector3 value, int importance = 2) : base(value, importance)
        {
        }

        public override bool ApproxEquals(IKnowlObj otherKnowl)
        {
            var otherObject = otherKnowl as VectorKnowledge;
            if (otherObject == null)
                return false;

            //do approximate comparison
            return true;
        }
    }
}