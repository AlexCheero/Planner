using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class RedAction : Action
{
    public RedAction(int change)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, change } })
    {
    }

    public override void Perform()
    {
        Debug.LogError("Red!");
    }

    public override int GetDuration(Dictionary<string, object> knowledge)
    {
        return 2;
    }

    public override void AffectOnKnowledge(ref Dictionary<string, object> knowledge, float membership)
    {

    }

    public override byte GetMembership(Dictionary<string, object> knowledge)
    {
        return 230;
    }
}