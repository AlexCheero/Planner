using System.Collections.Generic;

namespace GOAP
{
    public abstract class Action
    {
        private readonly Dictionary<EGoal, int> _goalChanges;
        public byte MinMembershipDegree { get; protected set; }

        public int BoardIndex { get; private set; }//todo crutch
        
        public Action(Dictionary<EGoal, int> changes, byte membershipDegree = 0)
        {
            //todo crutch also, actually whole constructor is crutch
            BoardIndex = 0;
            _goalChanges = changes;
            MinMembershipDegree = membershipDegree;
        }

        public int GetGoalChange(Goal goal)
        {
            var name = goal.Name;
            return _goalChanges.ContainsKey(name) ? _goalChanges[name] : 0;
        }

        public abstract void Perform();

        public abstract int GetDuration(Dictionary<string, object> knowledge);

        public abstract void AffectOnKnowledge(ref Dictionary<string, object> knowledge, float membership);

        public abstract byte GetMembership(Dictionary<string, object> knowledge);
    }
}