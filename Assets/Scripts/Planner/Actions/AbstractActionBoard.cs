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

        private HashSet<EActionType> _actions;
        private HashSet<IActionFactory> _factories;

        protected AbstractActionBoard()
        {
            _factories = new HashSet<IActionFactory>();
            //i think it could be done without set of actions (if actions should contain every value of enum),
            //using Enum.GetValues, but it throws error
            _actions = new HashSet<EActionType>
            {
                EActionType.Internal,
                EActionType.Green,
                EActionType.Yellow,
                EActionType.Red
            };
            foreach (var action in _actions)
                _factories.Add(GetFactory(action));

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