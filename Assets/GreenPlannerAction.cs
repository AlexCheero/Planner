using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class GreenPlannerAction : PlannerAction
{
    public Vector3 TargetPosition;
    public string TargetName = "";

    public GreenPlannerAction(Vector3 position, byte efficiency, int duration)
        : base(new Dictionary<EGoal, float> { { EGoal.Goal, 0 } }, duration)
    {
        TargetPosition = position;
    }

    public override string Name
    {
        get { return TargetName + "GreenAction"; }
    }

    public override bool Perform(StateMachine machine)
    {
        var navAgent = machine.GetComponent<NavMeshAgent>();
        navAgent.SetDestination(TargetPosition);

        return Vector3.Distance(navAgent.transform.position, TargetPosition) <= 0.5f;
    }

    public override void AffectOnKnowledge(ref Dictionary<string, object> knowledge, float efficiency)
    {
        if (knowledge.ContainsKey("greened "))
            knowledge["greened "] = true;
        else
            knowledge.Add("greened ", true);
    }
}