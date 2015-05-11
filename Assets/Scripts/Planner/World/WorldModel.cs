using System.Collections.Generic;
using System.Text;

namespace GOAP
{
    public class WorldModel
    {
        //todo choose only few the most important goals
        public readonly Goal[] Goals;
        private int _actionIndex = 0;
        public List<Pair<PlannerAction, byte>> ActionsMembership = new List<Pair<PlannerAction, byte>>();//todo try to change all dictionarys with this structure
        public Planner Planner;
//        private Dictionary<string, object> Knowledge;
        private KnowledgeNode Knowledge;

        public float Discontentment { get; private set; }

        public WorldModel(Goal[] goals, KnowledgeNode knowledge, Planner planner)
        {
            Goals = goals;
            Knowledge = knowledge;
            Planner = planner;
            ActionsMembership = Planner.AllActions.GetActionsByKnowledge(Knowledge);
            Discontentment = 0;
            foreach (var goal in Goals)
                Discontentment += goal.GetDiscontentment();
        }

        public override int GetHashCode()
        {
            //todo make proper hash function
            var allActions = new StringBuilder();
            foreach (var action in ActionsMembership)
                allActions.Append(action.First.Name);

            return allActions.ToString().GetHashCode(); ;
        }

        //deep copy constructor
        public WorldModel(WorldModel otherModel)
        {
            var otherGoals = otherModel.Goals;
            Goals = new Goal[otherGoals.Length];
            for (var i = 0; i < Goals.Length; i++)
                Goals[i] = new Goal(otherGoals[i]);
            Discontentment = otherModel.Discontentment;
            ActionsMembership = new List<Pair<PlannerAction, byte>>(otherModel.ActionsMembership);
            Knowledge = new KnowledgeNode(otherModel.Knowledge);
            Planner = otherModel.Planner;
        }

        public Pair<PlannerAction, byte> NextAction()
        {
            if (_actionIndex >= ActionsMembership.Count)
                return null;
            
            var returnValue = ActionsMembership[_actionIndex];
            _actionIndex++;
            return returnValue;
        }

        public void ApplyAction(Pair<PlannerAction, byte> action)
        {
            Discontentment = 0;
            var floatMembership = (action.Second / 255);
            foreach (var goal in Goals)
            {
                goal.Value += action.First.GetGoalChange(goal) * floatMembership;
                Discontentment += goal.GetDiscontentment();
            }

            action.First.AffectOnKnowledge(ref Knowledge, action.Second);
            ActionsMembership = Planner.AllActions.GetActionsByKnowledge(Knowledge);
        }
    }
}
