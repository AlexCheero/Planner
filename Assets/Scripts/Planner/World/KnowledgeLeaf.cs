using System;

namespace GOAP
{
    public class KnowledgeLeaf<T> : IKnowledge
    {
        public T Value { get; set; }

        public KnowledgeLeaf(T value)
        {
            Value = value;
        }

        public bool ContainsAtFirstDepth(string key)
        {
            return false;
        }

        public IKnowledge this[string key]
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public bool Equals(IKnowledge otherKnowledge)
        {
            throw new NotImplementedException();
        }
    }
}