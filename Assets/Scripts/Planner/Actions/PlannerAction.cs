using System.Collections.Generic;

namespace GOAP
{
    public abstract class PlannerAction
    {
        private readonly Dictionary<EGoal, int> _goalChanges;

        public abstract string Name { get; }

        public int Duration { get; private set; }
        public byte MinMembershipDegree { get; private set; }

        protected PlannerAction(Dictionary<EGoal, int> changes, int duration, byte membershipDegree = 77)//77 for 30%
        {
            _goalChanges = changes;
            MinMembershipDegree = membershipDegree;
            Duration = duration;
        }

        public int GetGoalChange(Goal goal)
        {
            var type = goal.Type;
            return _goalChanges.ContainsKey(type) ? _goalChanges[type] : 0;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public abstract bool Perform(StateMachine machine);

        public abstract void AffectOnKnowledge(ref KnowledgeNode knowledge, float membership);
    }
}