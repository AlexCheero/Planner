using System.Collections.Generic;

namespace GOAP
{
    public class ActionTable
    {
        private List<ActionsCheck> _actionChecks;

        public ActionTable()
        {
            //todo use reflection/attributes, and add checks in loop, or make check classes
            _actionChecks = new List<ActionsCheck>
            {
                InternalCheck,
                GreenCheck,
                YellowCheck,
                RedCheck
            };
        }

        public List<PlannerAction> GetActions(KnowledgeNode knowledge)
        {
            var actions = new List<PlannerAction>();
            foreach (var check in _actionChecks)
                foreach (var action in check(knowledge))
                    actions.Add(action);
            return actions;
        } 

        private static List<PlannerAction> InternalCheck(KnowledgeNode knowledge)
        {
            return knowledge.Knowledge.Count > 0
                ? new List<PlannerAction> { new InternalPlannerAction(0) }
                : new List<PlannerAction>();
        }

        private static List<PlannerAction> GreenCheck(KnowledgeNode knowledge)
        {
            bool b;
            return knowledge.TryGetValue(out b, "stayed") && b
                ? new List<PlannerAction> { new GreenPlannerAction(0) }
                : new List<PlannerAction>();
        }

        private static List<PlannerAction> YellowCheck(KnowledgeNode knowledge)
        {
            bool b;
            return knowledge.TryGetValue(out b, "greened") && b
                ? new List<PlannerAction> { new YellowPlannerAction(0) }
                : new List<PlannerAction>();
        }

        private static List<PlannerAction> RedCheck(KnowledgeNode knowledge)
        {
            bool b;
            return knowledge.TryGetValue(out b, "yellowed") && b
                ? new List<PlannerAction> { new RedPlannerAction(0) }
                : new List<PlannerAction>();
        }
    }
}