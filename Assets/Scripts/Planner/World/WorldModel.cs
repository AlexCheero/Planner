using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GOAP
{
    public class WorldModel
    {
        //todo choose only few the most important goals
        public readonly Goal[] Goals;
        private int _actionIndex = 0;
        public List<Pair<Command, byte>> CommandsMembership = new List<Pair<Command, byte>>();//todo try to change all dictionarys with this structure
        public Planner Planner;
        private Dictionary<string, object> Knowledge;

        public float Discontentment { get; private set; }

        public WorldModel(Goal[] goals, Dictionary<string, object> knowledge, Planner planner)
        {
            Goals = goals;
            Knowledge = knowledge;
            Planner = planner;
            CommandsMembership = Planner.AllActions.GetActionsByKnowledge(Knowledge);
            Discontentment = 0;
            foreach (var goal in Goals)
                Discontentment += goal.GetDiscontentment();
        }

        public override int GetHashCode()
        {
            var allKeys = new StringBuilder();
            foreach (var key in Knowledge.Keys)
                allKeys.Append(key);
            //todo make proper hash function
            return allKeys.ToString().GetHashCode();
        }

        //deep copy constructor
        public WorldModel(WorldModel otherModel)
        {
            var otherGoals = otherModel.Goals;
            Goals = new Goal[otherGoals.Length];
            for (var i = 0; i < Goals.Length; i++)
                Goals[i] = new Goal(otherGoals[i]);
            Discontentment = otherModel.Discontentment;
            CommandsMembership = new List<Pair<Command, byte>>(otherModel.CommandsMembership);
            Knowledge = new Dictionary<string, object>(otherModel.Knowledge);
            Planner = otherModel.Planner;
        }

        public Pair<Command, byte> NextAction()
        {
            if (_actionIndex >= CommandsMembership.Count)
                return null;
            
            var returnValue = CommandsMembership[_actionIndex];
            _actionIndex++;
            return returnValue;
        }

        public void ApplyAction(Pair<Command, byte> action)
        {
            Discontentment = 0;
            var floatMembership = (action.Second / 255);
            foreach (var goal in Goals)
            {
                goal.Value += action.First.GetGoalChange(goal) * floatMembership;
                Discontentment += goal.GetDiscontentment();
            }

            action.First.AffectOnKnowledge(ref Knowledge, action.Second);
            CommandsMembership = Planner.AllActions.GetActionsByKnowledge(Knowledge);
        }
    }
}
