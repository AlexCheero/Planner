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

        public IEnumerable<PlannerAction> GetActions(Dictionary<string, object> knowledge)
        {
            object stayed;
            object position;
            return knowledge.TryGetValue("stayed ", out stayed) && (bool)stayed && knowledge.TryGetValue("green position ", out position)
                ? new[] { new GreenPlannerAction((Vector3)position, 255, 0) }
                : new PlannerAction[0];
        }
    }
}