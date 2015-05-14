

using System.Collections.Generic;
using System.Linq;

namespace GOAP
{
    public class ActionBoard
    {
        private HashSet<PlannerAction> _allActions;

        public ActionBoard()
        {
            _allActions = new HashSet<PlannerAction>();
        }

        public void AddActions(IEnumerable<PlannerAction> actions)
        {
            if (_allActions == null)
                _allActions = new HashSet<PlannerAction>();

            foreach (var action in actions)
                _allActions.Add(action);
        }

        //todo deprecated
        public List<Pair<PlannerAction, byte>> GetActionsByKnowledge(KnowledgeNode knowledge)
        {
            var list = new List<Pair<PlannerAction, byte>>();
            foreach (var action in _allActions)
            {
                var membership = action.GetMembership(knowledge);
                if (membership > action.MinMembershipDegree)
                    list.Add(new Pair<PlannerAction, byte>{First = action, Second = membership});
            }
            return list;
        }

        public List<PlannerAction> GetActions/*ByKnowledge*/(KnowledgeNode knowledge)
        {
            var resultList = new List<PlannerAction>();
            foreach (var action in _allActions)
                resultList.AddRange(action.FactoryMethod(knowledge));

            return resultList;
        } 
    }
}