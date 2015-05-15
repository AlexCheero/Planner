using System.Collections.Generic;

namespace GOAP
{
    public class RedActionFactory : IActionFactory
    {
        private static RedActionFactory _instance;

        public static RedActionFactory Instance
        {
            get { return _instance ?? (_instance = new RedActionFactory()); }
        }

        public IEnumerable<PlannerAction> GetActions(KnowledgeNode knowledge)
        {
            bool b;
            return knowledge.TryGetValue(out b, "yellowed") && b
                ? new[] {new RedPlannerAction(255, 0)}
                : new PlannerAction[0];
        }
    }
}