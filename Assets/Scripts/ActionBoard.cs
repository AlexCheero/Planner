

using System.Collections.Generic;

namespace GOAP
{
    public class ActionBoard
    {
        public List<Action> AllActions;

        public ActionBoard(List<Action> actions)
        {
            AllActions = actions;
        }

        public List<Action> GetActionsByKnowledge(Dictionary<string, object> knowledge)
        {
            var resultActions = new List<Action>();
            foreach (var action in AllActions)
            {
                if (action.CheckConditions(knowledge))
                    resultActions.Add(action);
            }

            return resultActions;
        }
    }
}