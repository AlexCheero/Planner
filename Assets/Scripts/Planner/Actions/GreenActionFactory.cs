using System.Collections.Generic;
using UnityEngine;

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
            bool stayed;
            Vector3 position;
            return knowledge.TryGetValue(out stayed, "stayed") && stayed && knowledge.TryGetValue(out position, "green", "position")
                ? new[] { new GreenPlannerAction(position, 255, 0) }
                : new PlannerAction[0];
        }
    }
}