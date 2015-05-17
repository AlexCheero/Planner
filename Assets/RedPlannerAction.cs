using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class RedPlannerAction : PlannerAction
{
    public RedPlannerAction(byte efficiency, int duration)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, -10 } }, efficiency, duration)
    {
    }

    public override string Name
    {
        get { return "RedAction"; }
    }

    public override void Perform()
    {
        Debug.Log("Red!");
    }

    public override void AffectOnKnowledge(ref KnowledgeNode knowledge, float membership)
    {

    }
}