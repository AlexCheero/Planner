using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class InternalPlannerAction : PlannerAction
{
    private InternalPlannerAction(byte efficiency, int duration)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, 0 } }, efficiency, duration)
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

    public override void AffectOnKnowledge(ref KnowledgeNode knowledge, float membership)
    {
        knowledge.SetValue(true, "stayed");
    }

    public override IEnumerable<PlannerAction> FactoryMethod(KnowledgeNode knowledge)
    {
        return knowledge.Knowledge.Count > 0
                ? new List<PlannerAction> { new InternalPlannerAction(255, 0) }
                : new List<PlannerAction>();
    }
}