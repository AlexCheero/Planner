using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class GreenAction : Action
{
    public GreenAction(int change)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, change } })
    {
    }

    public override void Perform()
    {
        Debug.Log("Green!");
    }

    public override int GetDuration(Dictionary<string, object> knowledge)
    {
        return 0;
    }

    public override void AffectOnKnowledge(ref Dictionary<string, object> knowledge, float membership)
    {

    }

    public override byte GetMembership(Dictionary<string, object> knowledge)
    {
        return 255;
    }
}