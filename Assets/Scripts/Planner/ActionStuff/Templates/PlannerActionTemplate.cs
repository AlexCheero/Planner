using System.Collections.Generic;

namespace GOAP
{
    public class PlannerActionTemplate : PlannerAction
    {
        public PlannerActionTemplate(Dictionary<EGoal, float> changes, int duration, byte actionEfficiency = 77) : base(changes, duration, actionEfficiency)
        {
        }

        public override EActionType Type
        {
            get { throw new System.NotImplementedException(); }
        }

        public override bool Perform(Actor machine)
        {
            throw new System.NotImplementedException();
        }

        public override void AffectOnKnowledge(ref Dictionary<string, object> knowledge, float efficiency)
        {
            throw new System.NotImplementedException();
        }
    }
}