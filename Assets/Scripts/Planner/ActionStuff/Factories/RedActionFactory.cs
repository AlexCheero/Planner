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

        public IEnumerable<PlannerAction> GetActions(Dictionary<string, object> knowledge)
        {
            object stayed;
            object position;
            return knowledge.TryGetValue("yellowed ", out stayed) && (bool)stayed && knowledge.TryGetValue("red position ", out position)
                ? new[] { new RedPlannerAction((Vector3)position, 0) }
                : null;
        }
    }
}