using System;
using System.Collections.Generic;

namespace GOAP
{
    public class WorldModel
    {
        public Goal[] Goals;
        public List<Action> Actions;
        private ActionBoard _actionBoard;
        private Dictionary<string, object> Knowledge;

        public float Discontentment { get; private set; }

        public WorldModel(Goal[] goals, Dictionary<string, object> knowledge, ActionBoard actionBoard)
        {
            Goals = goals;
            Knowledge = knowledge;
            _actionBoard = actionBoard;
            Actions = _actionBoard.GetActionsByKnowledge(Knowledge);
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
            Actions = new List<Action>(otherModel.Actions);
            Knowledge = new Dictionary<string, object>(otherModel.Knowledge);
        }

        public Action NextAction()
        {
            throw new NotImplementedException();
        }

        public void ApplyAction(Action action)
        {
            Discontentment = 0;
            foreach (var goal in Goals)
            {
                goal.Value += action.GetGoalChange(goal);
                Discontentment += goal.GetDiscontentment();
            }
            action.AffectOnKnowledge(ref Knowledge);
            Actions = _actionBoard.GetActionsByKnowledge(Knowledge);
        }
    }
}
