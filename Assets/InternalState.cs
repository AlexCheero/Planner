﻿using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class InternalState : MonoBehaviour
{

    public int GoalChange;

    void GetInternalActions()
    {
        GetComponent<CoroutinePlanner>().AllActions.AddActions(new[] { new InternalAction(GoalChange) });
    }
}

public class InternalAction : Action
{
    public InternalAction(int change)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, change } })
    {
    }

    public override void Perform()
    {
        Debug.Log("Stay!");
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