namespace GOAP
{
    internal class StringKnowledge : KnowledgeObject<string>
    {
        public StringKnowledge(string value, uint importance = 2)
            : base(value, importance)
        {
        }

        protected override bool CheckApproximateEquality(IKnowlObj other)
        {
            var otherValue = ((StringKnowledge) other).Value;
            return Value.Equals(otherValue);
        }
    }
}