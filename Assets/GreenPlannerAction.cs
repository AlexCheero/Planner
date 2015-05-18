using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class GreenPlannerAction : PlannerAction
{
    public Vector3 TargetPosition;
    public string TargetName = "";

    public GreenPlannerAction(Vector3 position, byte efficiency, int duration)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, 0 } }, efficiency, duration)
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

    public override void AffectOnKnowledge(ref KnowledgeNode knowledge, float membership)
    {
        knowledge.SetValue(true, "greened");
    }
}