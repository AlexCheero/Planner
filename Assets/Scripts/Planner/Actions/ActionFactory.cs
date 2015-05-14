using System.Collections.Generic;

namespace GOAP
{
    public delegate List<PlannerAction> ActionsCheck(KnowledgeNode knowledge);

    public class ActionFactory
    {
        private Dictionary<EActionType, ActionsCheck> _conditions;

        public ActionFactory()
        {
            _conditions = new Dictionary<EActionType, ActionsCheck>();
        }

        public void AddAction(EActionType type, ActionsCheck check)
        {
            _conditions.Add(type, check);
        }

        public bool Contains(EActionType type)
        {
            return _conditions.ContainsKey(type);
        }

        public List<PlannerAction> GetActionsByKnowledge(KnowledgeNode knowledge)
        {
            var result = new List<PlannerAction>();
            foreach (var condition in _conditions.Values)
                result.AddRange(condition(knowledge));
            return result;
        }
    }
}