using System.Collections.Generic;

namespace GOAP
{
    public interface IActionFactory
    {
        IEnumerable<PlannerAction> GetActions(KnowledgeNode knowledge);
    }

    public class InternalActionFactory : IActionFactory
    {
        private static InternalActionFactory _instance;

        public static InternalActionFactory Instance
        {
            get { return _instance ?? (_instance = new InternalActionFactory()); }
        }

        public IEnumerable<PlannerAction> GetActions(KnowledgeNode knowledge)
        {
            return knowledge.Knowledge.Count > 0
                ? new[] {new InternalPlannerAction(255, 0)}
                : new PlannerAction[0];
        }
    }

    public class GreenActionFactory : IActionFactory
    {
        private static GreenActionFactory _instance;

        public static GreenActionFactory Instance
        {
            get { return _instance ?? (_instance = new GreenActionFactory()); }
        }

        public IEnumerable<PlannerAction> GetActions(KnowledgeNode knowledge)
        {
            bool b;
            return knowledge.TryGetValue(out b, "stayed") && b
                ? new[] {new GreenPlannerAction(255, 0)}
                : new PlannerAction[0];
        }
    }

    public class YellowActionFactory : IActionFactory
    {
        private static YellowActionFactory _instance;

        public static YellowActionFactory Instance
        {
            get { return _instance ?? (_instance = new YellowActionFactory()); }
        }

        public IEnumerable<PlannerAction> GetActions(KnowledgeNode knowledge)
        {
            bool b;
            return knowledge.TryGetValue(out b, "greened") && b
                ? new[] {new YellowPlannerAction(255, 0)}
                : new PlannerAction[0];
        }
    }

    public class RedActionFactory : IActionFactory
    {
        private static RedActionFactory _instance;

        public static RedActionFactory Instance
        {
            get { return _instance ?? (_instance = new RedActionFactory()); }
        }

        public IEnumerable<PlannerAction> GetActions(KnowledgeNode knowledge)
        {
            bool b;
            return knowledge.TryGetValue(out b, "yellowed") && b
                ? new[] {new RedPlannerAction(255, 0)}
                : new PlannerAction[0];
        }
    }
}