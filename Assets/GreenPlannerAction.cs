using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class GreenPlannerAction : PlannerAction
{
    public Vector3 TargetPosition;
    public string TargetName;

    public GreenPlannerAction(int change)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, change } })
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

    public override int GetDuration(KnowledgeNode knowledge)
    {
        return 0;
    }

    public override void AffectOnKnowledge(ref KnowledgeNode knowledge, float membership)
    {
        knowledge.SetValue(true, "greened");
    }

    public override byte GetMembership(KnowledgeNode knowledge)
    {
        bool b;
        return (byte)(knowledge.TryGetValue(out b, "stayed") && b ? 255 : 0);
    }

    public override List<PlannerAction> FactoryMethod(KnowledgeNode knowledge)
    {
        bool b;
        return knowledge.TryGetValue(out b, "stayed") && b
            ? new List<PlannerAction> { new GreenPlannerAction(0) }
            : new List<PlannerAction>();
    }
}