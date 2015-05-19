﻿using System.Collections.Generic;

namespace GOAP
{
    public class WorldModel
    {
        //todo choose only few the most important goals
        public readonly Goal[] Goals;
        private int _actionIndex = 0;
        public List<PlannerAction> Actions = new List<PlannerAction>();
        public Planner Planner;
        public Dictionary<string, object> Knowledge;

        public float Discontentment { get; private set; }

        public WorldModel(Goal[] goals, Dictionary<string, object> knowledge, Planner planner)
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
            //in order for this to work all knowledge values must be the reference type
            var hash = 0;
            foreach (var knowledge in Knowledge)
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
            Actions = new List<PlannerAction>(otherModel.Actions);
            Knowledge = new Dictionary<string, object>(otherModel.Knowledge);
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
                Discontentment += goal.GetDiscontentment() /** action.Duration*/;//todo think about how to properly use duration
            }

            action.AffectOnKnowledge(ref Knowledge, /*action.ActionEfficiency*/255);
            Actions = Planner.ActionBoard.GetActions(Knowledge);
        }
    }
}
