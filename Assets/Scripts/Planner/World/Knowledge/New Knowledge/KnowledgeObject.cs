namespace GOAP
{
    internal interface IKnowlObj
    {
        int Importance { get; }
        bool ApproxEquals(IKnowlObj otherKnowl);
    }

    internal abstract class KnowledgeObject<T> : IKnowlObj
    {
        internal T Value;

        public int Importance { get; private set; }

        internal KnowledgeObject(T value, int importance = 2)
        {
            Value = value;
            Importance = importance;
        }

        public abstract bool ApproxEquals(IKnowlObj otherKnowl);
    }
}