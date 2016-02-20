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

        public IEnumerable<PlannerAction> GetActions(Dictionary<string, object> knowledge)
        {
            object b;
            return knowledge.TryGetValue("stayed ", out b) && !(bool)b
                ? new[] { new InternalPlannerAction(0, 255),  }
                : null;
        }
    }
}