

using System.Collections.Generic;

namespace GOAP
{
    public class ActionBoard
    {
        private HashSet<PlannerAction> _allActions;

        public ActionBoard()
        {
            _allActions = new HashSet<PlannerAction>();
        }

        public List<PlannerAction> GetActionsByKnowledge(KnowledgeNode knowledge)
        {
            var resultList = new List<PlannerAction>();
            foreach (var action in _allActions)
                resultList.AddRange(action.FactoryMethod(knowledge));

            return resultList;
        } 
    }
}