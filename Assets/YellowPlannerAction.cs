using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class YellowPlannerAction : PlannerAction {
    private YellowPlannerAction(byte efficiency, int duration)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, 0 } }, efficiency, duration)
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

    public override void AffectOnKnowledge(ref KnowledgeNode knowledge, float membership)
    {
        knowledge.SetValue(false, "greened");
        knowledge.SetValue(true, "yellowed");
    }

    public override IEnumerable<PlannerAction> FactoryMethod(KnowledgeNode knowledge)
    {
        bool b;
        return knowledge.TryGetValue(out b, "greened") && b
            ? new List<PlannerAction> { new YellowPlannerAction(255, 0) }
            : new List<PlannerAction>();
    }
}