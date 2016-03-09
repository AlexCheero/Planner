namespace GOAP
{
    internal class FloatKnowledge : KnowledgeObject<float>
    {
        private float _dispersion;

        public FloatKnowledge(float value, float dispersion = 0, uint importance = 2)
            : base(value, importance)
        {
            _dispersion = dispersion;
        }

        protected override bool CheckApproximateEquality(IKnowlObj other)
        {
            var otherValue = ((FloatKnowledge)other).Value;
            return PlannerUtils.ValueBetween(otherValue, Value, _dispersion);
        }
    }
}