using System.Collections.Generic;

namespace GOAP
{
    public abstract class AbstractActionBoard
    {
        private HashSet<EActionType> _actions;
        private HashSet<IActionFactory> _factories;

        protected AbstractActionBoard()
        {
            _factories = new HashSet<IActionFactory>();
            _actions = new HashSet<EActionType>
            {
                EActionType.Internal,
                EActionType.Green,
                EActionType.Yellow,
                EActionType.Red
            };
            foreach (var action in _actions)
                _factories.Add(GetFactory(action));
        }

        public List<PlannerAction> GetActions(Dictionary<string, object> knowledge)
        {
            var resultList = new List<PlannerAction>();
            foreach (var factory in _factories)
                resultList.AddRange(factory.GetActions(knowledge));

            return resultList;
        }

        protected abstract IActionFactory GetFactory(EActionType type);
    }

    public class ActionBoard : AbstractActionBoard
    {
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