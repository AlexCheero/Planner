using System.Collections.Generic;

namespace GOAP
{
    public abstract class PlannerAction
    {
        //todo think about identical actions with different positions
        private readonly Dictionary<EGoal, int> _goalChanges;

        public abstract string Name { get; }

        public byte ActionEfficiency { get; private set; }
        public int Duration { get; private set; }
        public byte MinMembershipDegree { get; private set; }

        protected PlannerAction(Dictionary<EGoal, int> changes, byte efficiency, int duration, byte membershipDegree = 77)//77 for 30%
        {
            //todo crutch also, actually whole constructor is crutch
            _goalChanges = changes;
            MinMembershipDegree = membershipDegree;
            ActionEfficiency = efficiency;
            Duration = duration;
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

        public abstract void AffectOnKnowledge(ref KnowledgeNode knowledge, float membership);
    }
}