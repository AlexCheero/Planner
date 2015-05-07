

using System.Collections.Generic;
using System.Linq;

namespace GOAP
{
    public class ActionBoard
    {
        private List<Command> AllActions;

        public Command this[int index]
        {
            get { return AllActions[index]; }
        }

        public ActionBoard()
        {
            AllActions = new List<Command>();
        }

        public void AddActions(IEnumerable<Command> actions)
        {
            if (AllActions == null)
                AllActions = new List<Command>();

            foreach (var action in actions)
            {
                AllActions.Add(action);
                action.BoardIndex = AllActions.IndexOf(action);
            }
        }

        public List<Pair<Command, byte>> GetActionsByKnowledge(Dictionary<string, object> knowledge)
        {
            var list = new List<Pair<Command, byte>>();
            foreach (var action in AllActions)
            {
                var membership = action.GetMembership(knowledge);
                if (membership > action.MinMembershipDegree)
                    list.Add(new Pair<Command, byte>{First = action, Second = membership});
            }
            return list;
        }
    }
}