

using System.Collections.Generic;
using System.Linq;

namespace GOAP
{
    public class ActionBoard
    {
        private List<Action> AllActions;

        public Action this[int index]
        {
            get { return AllActions[index]; }
        }

        public ActionBoard()
        {
            AllActions = new List<Action>();
        }

        public void AddActions(IEnumerable<Action> actions)
        {
            if (AllActions == null)
                AllActions = new List<Action>();

            foreach (var action in actions)
            {
                AllActions.Add(action);
                action.BoardIndex = AllActions.IndexOf(action);
            }
        }

        public List<Pair<Action, byte>> GetActionsByKnowledge(Dictionary<string, object> knowledge)
        {
            var list = new List<Pair<Action, byte>>();
            foreach (var action in AllActions)
            {
                var membership = action.GetMembership(knowledge);
                if (membership > action.MinMembershipDegree)
                    list.Add(new Pair<Action, byte>{First = action, Second = membership});
            }
            return list;
        }
    }
}