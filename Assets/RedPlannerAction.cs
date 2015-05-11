using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class RedPlannerAction : PlannerAction
{
    public RedPlannerAction(int change)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, change } })
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

    public override int GetDuration(KnowledgeNode knowledge)
    {
        return 0;
    }

    public override void AffectOnKnowledge(ref KnowledgeNode knowledge, float membership)
    {

    }

    public override byte GetMembership(KnowledgeNode knowledge)
    {
        bool b;
        return (byte)(knowledge.TryGetValue(out b, "yellowed") && b ? 255 : 0);
    }
}