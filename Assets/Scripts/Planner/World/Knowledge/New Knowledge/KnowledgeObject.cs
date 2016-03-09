namespace GOAP
{
    internal interface IKnowlObj
    {
        uint Importance { get; }
        bool ApproxEquals(IKnowlObj otherKnowl);
    }

    internal abstract class KnowledgeObject<T> : IKnowlObj
    {
        internal T Value;

        public uint Importance { get; private set; }

        internal KnowledgeObject(T value, uint importance = 2)
        {
            Value = value;
            Importance = importance;
        }

        public bool ApproxEquals(IKnowlObj otherKnowl)
        {
            return TypeEquals(otherKnowl) && CheckApproximateEquality(otherKnowl);
        }

        private bool TypeEquals(IKnowlObj other)
        {
            return other.GetType() == GetType();
        }

        protected abstract bool CheckApproximateEquality(IKnowlObj other);
    }
}