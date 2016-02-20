using System;
using System.Collections.Generic;

namespace GOAP
{
    public abstract class AbstractActionBoard
    {
        private static AbstractActionBoard _instance;

        public static AbstractActionBoard Instance
        {
            get { return _instance ?? (_instance = new ActionBoard()); }
        }

        private List<IActionFactory> _factories;

        protected AbstractActionBoard()
        {
            _factories = new List<IActionFactory>();
            foreach (var action in Enum.GetValues(typeof(EActionType)))
                _factories.Add(GetFactory((EActionType)action));

            _instance = this;
        }

        public List<PlannerAction> GetActions(Dictionary<string, object> knowledge)
        {
            var resultList = new List<PlannerAction>();
            foreach (var factory in _factories)
            {
                var actions = factory.GetActions(knowledge);
                if (actions != null)
                    resultList.AddRange(factory.GetActions(knowledge));
            }

            return resultList;
        }

        protected abstract IActionFactory GetFactory(EActionType type);
    }

    public class ActionBoard : AbstractActionBoard
    {
        //this shouldn't be public but if it is more closed i cant use this cunstructor in AbstractActionBoard instance generation
        public ActionBoard() { }

        protected override IActionFactory GetFactory(EActionType type)
        {
            //todo try to automate this process somehow
            switch (type)
            {
                case EActionType.Internal:
                    return InternalActionFactory.Instance;
                case EActionType.Green:
                    return GreenActionFactory.Instance;
                case EActionType.Yellow:
                    return YellowActionFactory.Instance;
                case EActionType.Red:
                    return RedActionFactory.Instance;
                default:
                    return null;
            }
        }
    }
}