using System.Collections.Generic;

namespace GOAP
{
    public class WorldModel
    {
        //todo choose only few the most important goals
        public readonly Goal[] Goals;
        private int _actionIndex = 0;
        public List<PlannerAction> Actions = new List<PlannerAction>();
        public Planner Planner;
        public KnowledgeNode Knowledge ;

        public float Discontentment { get; private set; }

        public WorldModel(Goal[] goals, KnowledgeNode knowledge, Planner planner)
        {
            Goals = goals;
            Knowledge = knowledge;
            Planner = planner;
            Actions = Planner.ActionBoard.GetActions(Knowledge);
            Discontentment = 0;
            for (var i = 0; i < Goals.Length; i++)
            {
                var goal = Goals[i];
                Discontentment += goal.GetDiscontentment();
            }
        }

        public bool Equals(WorldModel otherModel)
        {
            //todo make approximate equality method
            if (otherModel == null)
                return false;
            if (Goals.Length != otherModel.Goals.Length || Actions.Count != otherModel.Actions.Count)
                return false;
            if (!Knowledge.Equals(otherModel.Knowledge))
                return false;

            return Discontentment == otherModel.Discontentment;
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
            var hash = 0;
            for (var i = 0; i < Actions.Count; i++)
            {
                var action = Actions[i];
                hash += action.GetHashCode() /**action.ActionEfficiency / 255*/;
            }
            hash += Discontentment.GetHashCode();

            return hash;
//            or you can take hashes from all knowledge values, but if values will be reference type all will be fucked up
//            also you have to take in account the discontentement value
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
            for (var i = 0; i < Goals.Length; i++)
            {
                var goal = Goals[i];
                goal.Value += action.GetGoalChange(goal) * floatMembership;
                Discontentment += goal.GetDiscontentment() * action.Duration;
            }

            action.AffectOnKnowledge(ref Knowledge, /*action.ActionEfficiency*/255);
            Actions = Planner.ActionBoard.GetActions(Knowledge);
        }
    }
}
