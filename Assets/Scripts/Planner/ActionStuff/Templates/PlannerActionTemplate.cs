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

        public override void StartAction(Actor actor)
        {
            throw new System.NotImplementedException();
        }

        public override void Perform(Actor actor)
        {
            throw new System.NotImplementedException();
        }

        public override bool IsComplete()
        {
            throw new System.NotImplementedException();
        }

        public override void AffectOnKnowledge(ref Dictionary<string, object> knowledge, float efficiency)
        {
            throw new System.NotImplementedException();
        }
    }
}