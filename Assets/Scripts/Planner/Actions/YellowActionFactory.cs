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

        public IEnumerable<PlannerAction> GetActions(Dictionary<string, object> knowledge)
        {
            object stayed;
            object position;
            return knowledge.TryGetValue("greened ", out stayed) && (bool)stayed && knowledge.TryGetValue("yellow position ", out position)
                ? new[] { new YellowPlannerAction((Vector3)position, 0, 255) }
                : null;
        }
    }
}