using System;
using System.Collections.Generic;

namespace GOAP
{
    public class WorldModel
    {
        public Goal[] Goals;
        public List<Pair<Action, byte>> ActionsMembership;//todo try to change all dictionarys with this structure
        private CoroutinePlanner _planner;
        private Dictionary<string, object> Knowledge;

        public float Discontentment { get; private set; }

        public WorldModel(Goal[] goals, Dictionary<string, object> knowledge, CoroutinePlanner planner)
        {
            Goals = goals;
            Knowledge = knowledge;
            _planner = planner;
            ActionsMembership = _planner.AllActions.GetActionsByKnowledge(Knowledge);
            Discontentment = 0;
            foreach (var goal in Goals)
                Discontentment += goal.GetDiscontentment();
        }

        //deep copy constructor
        public WorldModel(WorldModel otherModel)
        {
            var otherGoals = otherModel.Goals;
            Goals = new Goal[otherGoals.Length];
            for (var i = 0; i < Goals.Length; i++)
                Goals[i] = new Goal(otherGoals[i]);
            Discontentment = otherModel.Discontentment;
            ActionsMembership = new List<Pair<Action, byte>>(otherModel.ActionsMembership);
            Knowledge = new Dictionary<string, object>(otherModel.Knowledge);
        }

        public Pair<Action, byte> NextAction()
        {
            throw new NotImplementedException();
        }

        public void ApplyAction(Pair<Action, byte> action)
        {
            Discontentment = 0;
            var floatMembership = (action.Second / 255);
            foreach (var goal in Goals)
            {
                goal.Value += action.First.GetGoalChange(goal) * floatMembership;
                Discontentment += goal.GetDiscontentment();
            }

            action.First.AffectOnKnowledge(ref Knowledge, action.Second);
            ActionsMembership = _planner.AllActions.GetActionsByKnowledge(Knowledge);
        }
    }
}
