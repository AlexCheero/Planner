using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class RedPlannerAction : PlannerAction
{
    private RedPlannerAction(byte efficiency, int duration)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, 0 } }, efficiency, duration)
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

    public override IEnumerable<PlannerAction> FactoryMethod(KnowledgeNode knowledge)
    {
        bool b;
        return knowledge.TryGetValue(out b, "yellowed") && b
            ? new List<PlannerAction> { new RedPlannerAction(255, 0) }
            : new List<PlannerAction>();
    }
}