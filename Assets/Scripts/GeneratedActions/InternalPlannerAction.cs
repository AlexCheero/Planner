using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class InternalPlannerAction : PlannerAction
{
    public InternalPlannerAction(int duration, byte efficiency)
        : base(new Dictionary<EGoal, float> { { EGoal.Goal, 0 } }, duration, efficiency)
    {
    }

    public override EActionType Type
    {
        get { return EActionType.Internal; }
    }

    public override bool Perform(Actor machine)
    {
        Debug.Log("Internal!");
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