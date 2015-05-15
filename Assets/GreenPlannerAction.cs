using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class GreenPlannerAction : PlannerAction
{
    public Vector3 TargetPosition;
    public string TargetName;

    public GreenPlannerAction(byte efficiency, int duration)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, 0 } }, efficiency, duration)
    {
    }

    public override string Name
    {
        get { return TargetName + "GreenAction"; }
    }

    public override void Perform()
    {
        Debug.Log("Green!");

        //move to position, do green thing
    }

    public override void AffectOnKnowledge(ref KnowledgeNode knowledge, float membership)
    {
        knowledge.SetValue(true, "greened");
    }
}