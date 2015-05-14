using System.Collections.Generic;
using System.Linq;

namespace GOAP
{
    public class WorldModel
    {
        //todo override ==
        //todo choose only few the most important goals
        public readonly Goal[] Goals;
        private int _actionIndex = 0;
        //todo try to change all dictionarys with this structure
        public List<PlannerAction> Actions = new List<PlannerAction>();
        public Planner Planner;
        private KnowledgeNode Knowledge;

        public float Discontentment { get; private set; }

        public WorldModel(Goal[] goals, KnowledgeNode knowledge, Planner planner)
        {
            Goals = goals;
            Knowledge = knowledge;
            Planner = planner;
            Actions = Planner.AllActions.GetActionsByKnowledge(Knowledge);
            Discontentment = 0;
            foreach (var goal in Goals)
                Discontentment += goal.GetDiscontentment();
        }

        public override int GetHashCode()
        {
//            something like this
//            var allActions = new StringBuilder();
//            foreach (var action in ActionsMembership)
//                allActions.Append(action.First.Name);
//
//            return allActions.ToString().GetHashCode();
            
//            or this
            return Actions.Sum(action => action.GetHashCode()/**action.ActionEfficiency*/ / 255);
//            or you can take hashes from all knowledge values, but if values will be reference type all will be fucked up
        }

        //deep copy constructor
        public WorldModel(WorldModel otherModel)
        {
            var otherGoals = otherModel.Goals;
            Goals = new Goal[otherGoals.Length];
            for (var i = 0; i < Goals.Length; i++)
                Goals[i] = new Goal(otherGoals[i]);
            Discontentment = otherModel.Discontentment;
            Actions = new List<PlannerAction>(otherModel.Actions);
            Knowledge = new KnowledgeNode(otherModel.Knowledge);
            Planner = otherModel.Planner;
        }

        public PlannerAction NextAction()
        {
            if (_actionIndex >= Actions.Count)
                return null;
            
            var returnValue = Actions[_actionIndex];
            _actionIndex++;
            return returnValue;
        }

        public void ApplyAction(PlannerAction action)
        {
            Discontentment = 0;
            var floatMembership = 1f;//(action.ActionEfficiency / 255);
            foreach (var goal in Goals)
            {
                goal.Value += action.GetGoalChange(goal) * floatMembership;
                Discontentment += goal.GetDiscontentment();
            }

            action.AffectOnKnowledge(ref Knowledge, /*action.ActionEfficiency*/255);
            Actions = Planner.AllActions.GetActionsByKnowledge(Knowledge);
        }
    }
}
