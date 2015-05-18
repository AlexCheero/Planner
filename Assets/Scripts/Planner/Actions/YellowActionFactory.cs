using System.Collections.Generic;
using UnityEngine;

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
            bool stayed;
            Vector3 position;
            return knowledge.TryGetValue(out stayed, "greened") && stayed && knowledge.TryGetValue(out position, "yellow", "position")
                ? new[] { new YellowPlannerAction(position, 255, 0) }
                : new PlannerAction[0];
        }
    }
}