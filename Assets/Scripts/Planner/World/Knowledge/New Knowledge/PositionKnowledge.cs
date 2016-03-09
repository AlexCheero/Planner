using UnityEngine;

namespace GOAP
{
    internal class PositionKnowledge : KnowledgeObject<Vector3>
    {
        private float _heightDispersion;
        private float _onGroundDispersion;
        public PositionKnowledge(Vector3 value, float heightDispersion = 0, float onGroundDispersion = 0,
            uint importance = 2) : base(value, importance)
        {
            _heightDispersion = heightDispersion;
            _onGroundDispersion = onGroundDispersion;
        }

        protected override bool CheckApproximateEquality(IKnowlObj other)
        {
            var otherValue = ((PositionKnowledge)other).Value;
            var acceptableHeight = PlannerUtils.ValueBetween(otherValue.y, Value.y, _heightDispersion);

            var groundPos = PlannerUtils.PythagoreanTheorem(Value.x, Value.z);
            var otherGroundPos = PlannerUtils.PythagoreanTheorem(otherValue.x, otherValue.z);

            var acceptableGroundPos = PlannerUtils.ValueBetween(otherGroundPos, groundPos, _onGroundDispersion);

            return acceptableHeight && acceptableGroundPos;
        }
    }
}