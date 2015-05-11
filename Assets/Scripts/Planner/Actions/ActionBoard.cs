

using System.Collections.Generic;

namespace GOAP
{
    public class ActionBoard
    {
        private List<PlannerAction> AllActions;

        public PlannerAction this[int index]
        {
            get { return AllActions[index]; }
        }

        public ActionBoard()
        {
            AllActions = new List<PlannerAction>();
        }

        public void AddActions(IEnumerable<PlannerAction> actions)
        {
            if (AllActions == null)
                AllActions = new List<PlannerAction>();

            foreach (var action in actions)
            {
                AllActions.Add(action);
                action.BoardIndex = AllActions.IndexOf(action);
            }
        }

        public List<Pair<PlannerAction, byte>> GetActionsByKnowledge(KnowledgeNode knowledge)
        {
            var list = new List<Pair<PlannerAction, byte>>();
            foreach (var action in AllActions)
            {
                var membership = action.GetMembership(knowledge);
                if (membership > action.MinMembershipDegree)
                    list.Add(new Pair<PlannerAction, byte>{First = action, Second = membership});
            }
            return list;
        }
    }
}