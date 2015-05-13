using System.Collections.Generic;

namespace GOAP
{
    public class ActionFactory
    {
        public HashSet<EActionType> AllActions;

        public ActionFactory()
        {
            AllActions = new HashSet<EActionType>();
        }

        public List<PlannerAction> GetAvailableActions(KnowledgeNode knowledge)
        {
            var resultActions = new List<PlannerAction>();
            foreach (var actionType in AllActions)
                foreach (var action in GenerateActionsByType(actionType, knowledge))
                    resultActions.Add(action);

            return resultActions;
        }

        
        private PlannerAction[] GenerateActionsByType(EActionType type, KnowledgeNode knowledge)
        {
            switch (type)
            {
                case EActionType.Internal:
                    return CheckInternalActions(knowledge);
                case EActionType.Green:
                    return CheckInternalActions(knowledge);
                case EActionType.Yellow:
                    return CheckInternalActions(knowledge);
                case EActionType.Red:
                    return CheckInternalActions(knowledge);
                default:
                    return new PlannerAction[0];
            }
        }

        #region Action checking
        //todo probably should realize this with dictionary of delegates
        private PlannerAction[] CheckInternalActions(KnowledgeNode knowledge)
        {
            return knowledge.Knowledge.Count > 0
                ? new PlannerAction[] {new InternalPlannerAction(0)}
                : new PlannerAction[0];
        }

        private PlannerAction[] CheckGreenlActions(KnowledgeNode knowledge)
        {
            bool b;
            return knowledge.TryGetValue(out b, "stayed") && b
                ? new PlannerAction[] {new GreenPlannerAction(0)}
                : new PlannerAction[0];
        }

        private PlannerAction[] CheckYellowActions(KnowledgeNode knowledge)
        {
            bool b;
            return knowledge.TryGetValue(out b, "greened") && b
                ? new PlannerAction[] {new YellowPlannerAction(0)}
                : new PlannerAction[0];
        }

        private PlannerAction[] CheckRedActions(KnowledgeNode knowledge)
        {
            bool b;
            return knowledge.TryGetValue(out b, "yellowed") && b
                ? new PlannerAction[] {new RedPlannerAction(0)}
                : new PlannerAction[0];
        }
        #endregion
    }

    public enum EActionType
    {
        Internal,
        Green,
        Yellow,
        Red
    }
}