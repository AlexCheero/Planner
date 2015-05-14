using System.Collections.Generic;

namespace GOAP
{
    public class ActionTable
    {
        public Dictionary<EActionType, ActionsCheck> Actions { get; private set; }

        public ActionTable()
        {
            Actions = new Dictionary<EActionType, ActionsCheck>();

            //todo use reflection/attributes, and add checks in loop
            Actions.Add(EActionType.Internal, InternalCheck);
            Actions.Add(EActionType.Green, GreenCheck);
            Actions.Add(EActionType.Yellow, YellowCheck);
            Actions.Add(EActionType.Red, RedCheck);
        }

        private static List<PlannerAction> InternalCheck(KnowledgeNode knowledge)
        {
            return new List<PlannerAction>();
        }

        private static List<PlannerAction> GreenCheck(KnowledgeNode knowledge)
        {
            return new List<PlannerAction>();
        }

        private static List<PlannerAction> YellowCheck(KnowledgeNode knowledge)
        {
            return new List<PlannerAction>();
        }

        private static List<PlannerAction> RedCheck(KnowledgeNode knowledge)
        {
            return new List<PlannerAction>();
        }
    }
}