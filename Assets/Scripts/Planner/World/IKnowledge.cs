namespace GOAP
{
    public interface IKnowledge
    {
        bool Contains(string key);
        IKnowledge this[string key] { get; set; }
        bool Equals(IKnowledge otherKnowledge);
    }
}