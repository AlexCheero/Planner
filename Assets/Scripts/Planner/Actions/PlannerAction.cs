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

        public PlannerAction(Dictionary<EGoal, int> changes, byte membershipDegree = 77)//77 for 30%
        {
            //todo crutch also, actually whole constructor is crutch
            _goalChanges = changes;
            MinMembershipDegree = membershipDegree;
        }

        public int GetGoalChange(Goal goal)
        {
            //todo consider membership, duration etc. (something affects on the goal change, and something affects only on membersgip)
            var name = goal.Name;
            return _goalChanges.ContainsKey(name) ? _goalChanges[name] : 0;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public abstract void Perform();

        public abstract int GetDuration(KnowledgeNode knowledge);

        public abstract void AffectOnKnowledge(ref KnowledgeNode knowledge, float membership);

        public abstract byte GetMembership(KnowledgeNode knowledge);

        public abstract List<PlannerAction> FactoryMethod(KnowledgeNode knowledge);
    }
}