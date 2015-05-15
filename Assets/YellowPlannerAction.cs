using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class YellowPlannerAction : PlannerAction {
    public YellowPlannerAction(byte efficiency, int duration)
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
}