using System.Collections.Generic;

namespace GOAP
{
    public class ActionBoard
    {
        private static ActionBoard _instance;

        public static ActionBoard Instance
        {
            get { return _instance ?? (_instance = new ActionBoard()); }
        }

        private List<IActionFactory> _factories = new List<IActionFactory>
        {
            InternalActionFactory.Instance,
            GreenActionFactory.Instance,
            YellowActionFactory.Instance,
            RedActionFactory.Instance
        };

        protected ActionBoard()
        {
            _instance = this;
        }

        public List<PlannerAction> GetActions(Dictionary<string, object> knowledge)
        {
            var resultList = new List<PlannerAction>();
            foreach (var factory in _factories)
            {
                var actions = factory.GetActions(knowledge);
                if (actions != null)
                    resultList.AddRange(actions);
            }

            return resultList;
        }
    }
}