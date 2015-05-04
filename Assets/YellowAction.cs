using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class YellowAction : Action {
    public YellowAction(int change)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, change } })
    {
    }

    public override void Perform()
    {
        Debug.LogError("Yellow!");
    }

    public override int GetDuration(Dictionary<string, object> knowledge)
    {
        return 1;
    }

    public override void AffectOnKnowledge(ref Dictionary<string, object> knowledge, float membership)
    {

    }

    public override byte GetMembership(Dictionary<string, object> knowledge)
    {
        return 125;
    }
}