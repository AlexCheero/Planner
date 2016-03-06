namespace GOAP
{
    internal interface IKnowlObj { }

    internal class KnowledgeObject<T> : IKnowlObj
    {
        internal T Value;
        internal float Dispersion { get; private set; }
        internal int Importance { get; private set; }

        internal KnowledgeObject(T value, int importance = 2, float dispersion = -1)
        {
            Value = value;
            Dispersion = dispersion;
            Importance = importance;
        }

        internal bool ApproxEquals(KnowledgeObject<T> otherKnowl)
        {
            return false;
        }
    }
}