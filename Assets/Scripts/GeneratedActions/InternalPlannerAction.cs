using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class InternalPlannerAction : PlannerAction
    {
        public InternalPlannerAction(int duration, byte efficiency = 255)
            : base(new Dictionary<EGoal, float> { { EGoal.Goal, 0 } }, duration, efficiency)
        {
        }

        public override EActionType Type
        {
            get { return EActionType.Internal; }
        }

        public override void StartAction(Actor actor)
        {
            base.StartAction(actor);
            Debug.Log("Start Internal!");
        }

        public override void Perform(Actor actor)
        {
            Debug.Log("Performing Internal!");
        }

        public override bool IsComplete()
        {
            return true;
        }

        public override void AffectOnKnowledge(ref Dictionary<string, object> knowledge, float efficiency)
        {
            if (knowledge.ContainsKey("stayed "))
                knowledge["stayed "] = true;
            else
                knowledge.Add("stayed ", true);
        }
    }
}
