

using System.Collections.Generic;
using System.Linq;

namespace GOAP
{
    public class ActionBoard
    {
        private List<Action> AllActions;

        public ActionBoard(Action[] actions)
        {
            AllActions = new List<Action>();
            foreach (var action in actions)
                AddActions(actions);
        }

        public void AddActions(IEnumerable<Action> actions)
        {
            //todo init board index somehow
            if (AllActions == null)
                AllActions = new List<Action>();
            foreach (var action in actions)
            {
                AllActions.Add(action);
            }
        }

        public List<Pair<Action, byte>> GetActionsByKnowledge(Dictionary<string, object> knowledge)
        {
            return (from action in AllActions
                let membership = action.GetMembership(knowledge)
                where membership > action.MinMembershipDegree
                select new Pair<Action, byte> {First = action, Second = membership}).ToList();
        }
    }
}