using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class InternalPlannerAction : PlannerAction
{
    public InternalPlannerAction(byte efficiency, int duration)
        : base(new Dictionary<EGoal, float> { { EGoal.Goal, 0 } }, duration)
    {
    }

    public override string Name
    {
        get { return "InternalAction"; }
    }

    public override bool Perform(ActionPerformer machine)
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