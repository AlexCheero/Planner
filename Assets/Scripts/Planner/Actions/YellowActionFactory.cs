using System.Collections.Generic;

namespace GOAP
{
    public class YellowActionFactory : IActionFactory
    {
        private static YellowActionFactory _instance;

        public static YellowActionFactory Instance
        {
            get { return _instance ?? (_instance = new YellowActionFactory()); }
        }

        public IEnumerable<PlannerAction> GetActions(KnowledgeNode knowledge)
        {
            bool b;
            return knowledge.TryGetValue(out b, "greened") && b
                ? new[] {new YellowPlannerAction(255, 0)}
                : new PlannerAction[0];
        }
    }
}