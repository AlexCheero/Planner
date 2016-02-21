using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class YellowPlannerAction : PlannerAction 
{
    public Vector3 TargetPosition;
    public string TargetName = "";

    public YellowPlannerAction(Vector3 position, int duration, byte efficiency)
        : base(new Dictionary<EGoal, float> { { EGoal.Goal, 0 } }, duration, efficiency)
    {
        TargetPosition = position;
    }

    public override EActionType Type
    {
        get { return EActionType.Yellow;}
    }

    public override bool Perform(Actor machine)
    {
        var navAgent = machine.GetComponent<NavMeshAgent>();
        navAgent.SetDestination(TargetPosition);

        return Vector3.Distance(navAgent.transform.position, TargetPosition) <= 0.5f;
    }

    public override void AffectOnKnowledge(ref Dictionary<string, object> knowledge, float efficiency)
    {
        if (knowledge.ContainsKey("yellowed "))
            knowledge["yellowed "] = true;
        else
            knowledge.Add("yellowed ", true);
    }
}