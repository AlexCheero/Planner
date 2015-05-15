using System.Collections.Generic;

namespace GOAP
{
    public class InternalActionFactory : IActionFactory
    {
        private static InternalActionFactory _instance;

        public static InternalActionFactory Instance
        {
            get { return _instance ?? (_instance = new InternalActionFactory()); }
        }

        public IEnumerable<PlannerAction> GetActions(KnowledgeNode knowledge)
        {
            bool b;
            return knowledge.TryGetValue(out b, "stayed") && !b
                ? new[] { new InternalPlannerAction(255, 0),  }
                : new PlannerAction[0];
        }
    }
}