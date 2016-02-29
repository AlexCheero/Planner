using System.Collections.Generic;

namespace GOAP
{
    public abstract class PlannerAction
    {
        //probably goal changes can be int
        private readonly Dictionary<EGoal, float> _goalChanges;

        public abstract EActionType Type { get; }

        //Duration and Efficiency are calculated in action factory depending on knowledge.
        //this factors influence on new knowledge generation, of next step worl model
        public int Duration { get; private set; }
        public byte Efficiency { get; private set; }

        public bool IsStarted { get; private set; }

        protected PlannerAction(Dictionary<EGoal, float> changes, int duration, byte efficiency = 255)
        {
            _goalChanges = changes;
            Efficiency = efficiency;
            Duration = duration;
        }

        public float GetGoalChange(Goal goal)
        {
            var type = goal.Type;
            return _goalChanges.ContainsKey(type) ? _goalChanges[type] : 0;
        }

        public override int GetHashCode()
        {
            return Type.ToString().GetHashCode();
        }

        public virtual void StartAction(Actor actor)
        {
            IsStarted = true;
        }

        public abstract void Perform(Actor actor);

        public abstract bool IsComplete();

        public abstract void AffectOnKnowledge(ref Dictionary<string, object> knowledge, float efficiency);
    }
}