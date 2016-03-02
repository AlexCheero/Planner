using System;
using System.Collections.Generic;

namespace GOAP
{
    public class WorldModel
    {
        //todo choose only few the most important goals
        public readonly Goal[] Goals;
        private int _actionIndex = 0;
        public List<PlannerAction> _actions;
        public Dictionary<string, object> _knowledge;

        public float Discontentment { get; private set; }

        public WorldModel(Goal[] goals, Dictionary<string, object> knowledge)
        {
            Goals = goals;
            _knowledge = knowledge;
            _actions = ActionBoard.Instance.GetActions(_knowledge);
            Discontentment = 0;
            for (var i = 0; i < Goals.Length; i++)
            {
                var goal = Goals[i];
                Discontentment += goal.GetDiscontentment();
            }
        }

        public bool ApproxEquals(WorldModel otherModel)
        {
            if (otherModel == null)
                return false;
            if (Goals.Length != otherModel.Goals.Length || _actions.Count != otherModel._actions.Count)
                return false;
            //this is not fits into conception of approximate equality
            if (!_knowledge.CheckEquality(otherModel._knowledge))
                return false;


            return Math.Abs(Discontentment - otherModel.Discontentment) < Discontentment * 0.1f;
        }

        public override int GetHashCode()
        {
            //in order for this to work all knowledge values must be the value type
            var hash = 0;
            foreach (var knowledge in _knowledge)
                hash += knowledge.Key.GetHashCode() + knowledge.Value.GetHashCode();

            return hash;
        }

        //deep copy constructor
        public WorldModel(WorldModel otherModel)
        {
            var otherGoals = otherModel.Goals;
            Goals = new Goal[otherGoals.Length];
            for (var i = 0; i < Goals.Length; i++)
                Goals[i] = new Goal(otherGoals[i]);
            Discontentment = otherModel.Discontentment;
            _actions = new List<PlannerAction>(otherModel._actions);
            _knowledge = new Dictionary<string, object>(otherModel._knowledge);
        }

        public PlannerAction NextAction()
        {
            if (_actionIndex >= _actions.Count)
                return null;
            
            var returnValue = _actions[_actionIndex];
            _actionIndex++;
            return returnValue;
        }

        public void ApplyAction(PlannerAction action)
        {
            Discontentment = 0;
            var floatEfficiency = 1f;//(action.Efficiency / 255);
            for (var i = 0; i < Goals.Length; i++)
            {
                var goal = Goals[i];
                goal.Value += action.GetGoalChange(goal) * floatEfficiency;
                Discontentment += goal.GetDiscontentment() /** action.Duration*/;//todo think about how to properly use duration
            }

            action.AffectOnKnowledge(ref _knowledge, /*action.Efficiency*/255);
            _actions = ActionBoard.Instance.GetActions(_knowledge);
        }
    }
}
