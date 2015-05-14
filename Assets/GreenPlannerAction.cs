using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class GreenPlannerAction : PlannerAction
{
    public Vector3 TargetPosition;
    public string TargetName;

    private GreenPlannerAction(byte efficiency, int duration)
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

    public override IEnumerable<PlannerAction> FactoryMethod(KnowledgeNode knowledge)
    {
        bool b;
        return knowledge.TryGetValue(out b, "stayed") && b
            ? new List<PlannerAction> { new GreenPlannerAction(255, 0) }
            : new List<PlannerAction>();
    }
}