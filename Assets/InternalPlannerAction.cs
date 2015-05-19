using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class InternalPlannerAction : PlannerAction
{
    public InternalPlannerAction(byte efficiency, int duration)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, 0 } }, duration)
    {
    }

    public override string Name
    {
        get { return "InternalAction"; }
    }

    public override bool Perform(StateMachine machine)
    {
        Debug.Log("Internal!");
        return true;
    }

    public override void AffectOnKnowledge(ref KnowledgeNode knowledge, float membership)
    {
        knowledge.SetValue(true, "stayed");
    }
}