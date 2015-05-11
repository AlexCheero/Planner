using System.Collections.Generic;

namespace GOAP
{
    public abstract class PlannerAction
    {
        //todo think about identical actions with different positions
        private readonly Dictionary<EGoal, int> _goalChanges;
        public byte MinMembershipDegree { get; private set; }

        //todo consider somehow membership of action
        public abstract string Name { get; }

        public int BoardIndex = -1;
        
        public PlannerAction(Dictionary<EGoal, int> changes, byte membershipDegree = 77)//77 for 30%
        {
            //todo crutch also, actually whole constructor is crutch
            _goalChanges = changes;
            MinMembershipDegree = membershipDegree;
        }

        public int GetGoalChange(Goal goal)
        {
            var name = goal.Name;
            return _goalChanges.ContainsKey(name) ? _goalChanges[name] : 0;
        }

        public abstract void Perform();

        public abstract int GetDuration(KnowledgeNode knowledge);

        public abstract void AffectOnKnowledge(ref KnowledgeNode knowledge, float membership);

        public abstract byte GetMembership(KnowledgeNode knowledge);
    }
}