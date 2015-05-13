using System;
using System.Collections.Generic;

namespace GOAP
{
    public class ActionFactory
    {
        public HashSet<EActionType> AllActions;

        public List<PlannerAction> GetActions(KnowledgeNode knowledge)
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
                    return checkInternalActions(knowledge);
                case EActionType.Green:
                    return checkInternalActions(knowledge);
                case EActionType.Yellow:
                    return checkInternalActions(knowledge);
                case EActionType.Red:
                    return checkInternalActions(knowledge);
                default:
                    return new PlannerAction[0];
            }
        }

        #region Action checking
        private InternalPlannerAction[] checkInternalActions(KnowledgeNode knowledge)
        {
            //check knowledge, return actions
            throw new NotImplementedException();
        }

        private GreenPlannerAction[] checkGreenlActions(KnowledgeNode knowledge)
        {
            //check knowledge, return actions
            throw new NotImplementedException();
        }

        private YellowPlannerAction[] checkYellowActions(KnowledgeNode knowledge)
        {
            //check knowledge, return actions
            throw new NotImplementedException();
        }

        private RedPlannerAction[] checkRedActions(KnowledgeNode knowledge)
        {
            //check knowledge, return actions
            throw new NotImplementedException();
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