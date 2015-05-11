﻿using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class InternalState : MonoBehaviour
{

    public int GoalChange;

    void GetInternalActions()
    {
        GetComponent<Planner>().AllActions.AddActions(new[] { new InternalPlannerAction(GoalChange) });
    }
}

public class InternalPlannerAction : PlannerAction
{
    public InternalPlannerAction(int change)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, change } })
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

    public override int GetDuration(KnowledgeNode knowledge)
    {
        return 0;
    }

    public override void AffectOnKnowledge(ref KnowledgeNode knowledge, float membership)
    {
        knowledge.SetValue(true, "stayed");
    }

    public override byte GetMembership(KnowledgeNode knowledge)
    {
        return (byte)(knowledge.Knowledge.Count == 0 ? 255 : 0);
    }
}
