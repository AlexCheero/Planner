using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class InternalPlannerAction : PlannerAction
{
    public InternalPlannerAction(int change)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, change } })
    {
    }

    public override string Name
    {
        get { return "InternalAction"; }
    }

    public override void Perform()
    {
        Debug.Log("Stay!");
    }

    public override int GetDuration(KnowledgeNode knowledge)
    {
        return 0;
    }

    public override void AffectOnKnowledge(ref KnowledgeNode knowledge, float membership)
    {
        knowledge.SetValue(true, "stayed");
    }

    public override byte GetMembership(KnowledgeNode knowledge)
    {
        return (byte)(knowledge.Knowledge.Count == 0 ? 255 : 0);
    }

    public override List<PlannerAction> FactoryMethod(KnowledgeNode knowledge)
    {
        return knowledge.Knowledge.Count > 0
                ? new List<PlannerAction> { new InternalPlannerAction(0) }
                : new List<PlannerAction>();
    }
}