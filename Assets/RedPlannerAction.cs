﻿using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class RedPlannerAction : PlannerAction
{
    public Vector3 TargetPosition;
    public string TargetName = "";

    public RedPlannerAction(Vector3 position, int duration, byte efficiency)
        : base(new Dictionary<EGoal, float> { { EGoal.Goal, -10 } }, duration, efficiency)
    {
        TargetPosition = position;
    }

    public override string Name
    {
        get { return TargetName + "RedAction"; }
    }

    public override bool Perform(Actor machine)
    {
        var navAgent = machine.GetComponent<NavMeshAgent>();
        navAgent.SetDestination(TargetPosition);

        return false;
    }

    public override void AffectOnKnowledge(ref Dictionary<string, object> knowledge, float efficiency)
    {

    }
}