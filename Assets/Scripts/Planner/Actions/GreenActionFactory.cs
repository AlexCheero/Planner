using System.Collections.Generic;

namespace GOAP
{
    public class GreenActionFactory : IActionFactory
    {
        private static GreenActionFactory _instance;

        public static GreenActionFactory Instance
        {
            get { return _instance ?? (_instance = new GreenActionFactory()); }
        }

        public IEnumerable<PlannerAction> GetActions(KnowledgeNode knowledge)
        {
            bool b;
            return knowledge.TryGetValue(out b, "stayed") && b
                ? new[] {new GreenPlannerAction(255, 0)}
                : new PlannerAction[0];
        }
    }
}