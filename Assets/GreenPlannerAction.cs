using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class GreenPlannerAction : PlannerAction
{
    public GreenPlannerAction(int change)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, change } })
    {
    }

    public override string Name
    {
        get { return "GreenAction"; }
    }

    public override void Perform()
    {
        Debug.Log("Green!");
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
}