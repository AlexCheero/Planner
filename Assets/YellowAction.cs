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
        Debug.Log("Yellow!");
    }

    public override int GetDuration(Dictionary<string, object> knowledge)
    {
        return 0;
    }

    public override void AffectOnKnowledge(ref Dictionary<string, object> knowledge, float membership)
    {
        knowledge.Remove("greened");
        if (knowledge.ContainsKey("yellowed"))
            return;
        knowledge.Add("yellowed", null);
    }

    public override byte GetMembership(Dictionary<string, object> knowledge)
    {
        return (byte)(knowledge.ContainsKey("greened") ? 255 : 0);
    }
}