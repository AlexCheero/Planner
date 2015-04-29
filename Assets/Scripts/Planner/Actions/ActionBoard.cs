

using System.Collections.Generic;
using System.Linq;

namespace GOAP
{
    public class ActionBoard
    {
        public List<Action> AllActions;

        public ActionBoard(List<Action> actions)
        {
            AllActions = actions;
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