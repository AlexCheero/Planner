using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class YellowPlannerAction : PlannerAction {
    public YellowPlannerAction(int change)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, change } })
    {
    }

    public override string Name
    {
        get { return "YellowAction"; }
    }

    public override void Perform()
    {
        Debug.Log("Yellow!");
    }

    public override int GetDuration(KnowledgeNode knowledge)
    {
        return 0;
    }

    public override void AffectOnKnowledge(ref KnowledgeNode knowledge, float membership)
    {
        knowledge.SetValue(false, "greened");
        knowledge.SetValue(true, "yellowed");
    }

    public override byte GetMembership(KnowledgeNode knowledge)
    {
        bool b;
        return (byte)(knowledge.TryGetValue(out b, "greened") && b ? 255 : 0);
    }

    public override List<PlannerAction> FactoryMethod(KnowledgeNode knowledge)
    {
        bool b;
        return knowledge.TryGetValue(out b, "greened") && b
            ? new List<PlannerAction> { new YellowPlannerAction(0) }
            : new List<PlannerAction>();
    }
}