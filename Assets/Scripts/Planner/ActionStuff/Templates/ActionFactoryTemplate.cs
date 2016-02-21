using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class ActionFactoryTemplate : IActionFactory
    {
        private static ActionFactoryTemplate _instance;

        public static ActionFactoryTemplate Instance
        {
            get { return _instance ?? (_instance = new ActionFactoryTemplate()); }
        }

        public IEnumerable<PlannerAction> GetActions(Dictionary<string, object> knowledge)
        {
            Debug.LogError(this + "error: you should implement this method before use it");
            throw new System.NotImplementedException();
        }
    }
}