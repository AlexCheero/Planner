namespace GOAP
{
    internal class IntKnowledge : KnowledgeObject<int>
    {
        private int _dispersion;

        public IntKnowledge(int value, int dispersion = 0, uint importance = 2)
            : base(value, importance)
        {
            _dispersion = dispersion;
        }

        protected override bool CheckApproximateEquality(IKnowlObj other)
        {
            var otherValue = ((IntKnowledge)other).Value;
            return PlannerUtils.ValueBetween(otherValue, Value, _dispersion);
        }
    }
}