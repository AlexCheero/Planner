using System.Collections.Generic;

namespace GOAP
{
    public class WorldModel
    {
        public Goal Goal;
        private int _actionIndex = 0;
        public List<Pair<Action, byte>> ActionsMembership;//todo try to change all dictionarys with this structure
        public Planner Planner;
        private Dictionary<string, object> Knowledge;

        public WorldModel(Goal goal, Dictionary<string, object> knowledge, Planner planner)
        {
            Goal = goal;
            Knowledge = knowledge;
            Planner = planner;
            ActionsMembership = Planner.AllActions.GetActionsByKnowledge(Knowledge);
        }

        //deep copy constructor
        public WorldModel(WorldModel otherModel)
        {
            var otherGoal = otherModel.Goal;
            Goal = new Goal(otherGoal);
            ActionsMembership = new List<Pair<Action, byte>>(otherModel.ActionsMembership);
            Knowledge = new Dictionary<string, object>(otherModel.Knowledge);
            Planner = otherModel.Planner;
        }

        public Pair<Action, byte> NextAction()
        {
            if (_actionIndex >= ActionsMembership.Count)
                return null;
            
            var returnValue = ActionsMembership[_actionIndex];
            _actionIndex++;
            return returnValue;
        }

        public void ApplyAction(Pair<Action, byte> action)
        {
            var floatMembership = (action.Second / 255);
            Goal.Value += action.First.GetGoalChange(Goal) * floatMembership;

            action.First.AffectOnKnowledge(ref Knowledge, action.Second);
            ActionsMembership = Planner.AllActions.GetActionsByKnowledge(Knowledge);
        }
    }
}
