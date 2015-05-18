using System.Collections.Generic;
using UnityEngine;

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
            bool stayed;
            Vector3 position;
            return knowledge.TryGetValue(out stayed, "yellowed") && stayed && knowledge.TryGetValue(out position, "red", "position")
                ? new[] { new RedPlannerAction(position, 255, 0) }
                : new PlannerAction[0];
        }
    }
}