namespace GOAP
{
    public interface IKnowledge
    {
        bool ContainsAtFirstDepth(string key);
        IKnowledge this[string key] { get; set; }
        bool Equals(IKnowledge otherKnowledge);
    }
}