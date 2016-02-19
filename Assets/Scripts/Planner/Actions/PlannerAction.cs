using System.Collections.Generic;

namespace GOAP
{
    public abstract class PlannerAction
    {
        //probably goal changes can be int
        private readonly Dictionary<EGoal, float> _goalChanges;

        public abstract string Name { get; }

        public int Duration { get; private set; }
        public byte ActionEfficiency { get; private set; }

        protected PlannerAction(Dictionary<EGoal, float> changes, int duration, byte actionEfficiency = 77)//77 for 30%
        {
            _goalChanges = changes;
            ActionEfficiency = actionEfficiency;
            Duration = duration;
        }

        public float GetGoalChange(Goal goal)
        {
            var type = goal.Type;
            return _goalChanges.ContainsKey(type) ? _goalChanges[type] : 0;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public abstract bool Perform(ActionPerformer machine);

        public abstract void AffectOnKnowledge(ref Dictionary<string, object> knowledge, float efficiency);
    }
}