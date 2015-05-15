using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class InternalPlannerAction : PlannerAction
{
    public InternalPlannerAction(byte efficiency, int duration)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, 0 } }, efficiency, duration)
    {
    }

    public override string Name
    {
        get { return "InternalAction"; }
    }

    public override void Perform()
    {
        Debug.Log("Internal!");
    }

    public override void AffectOnKnowledge(ref KnowledgeNode knowledge, float membership)
    {
        knowledge.SetValue(true, "stayed");
    }
}