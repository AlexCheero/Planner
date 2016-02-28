﻿using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class YellowPlannerAction : PlannerAction
    {
        public Vector3 TargetPosition;
        public string TargetName = "";

        private NavMeshAgent _navAgent;

        public YellowPlannerAction(Vector3 position, int duration, byte efficiency)
            : base(new Dictionary<EGoal, float> { { EGoal.Goal, 0 } }, duration, efficiency)
        {
            TargetPosition = position;
        }

        public override EActionType Type
        {
            get { return EActionType.Yellow; }
        }

        public override void StartAction(Actor actor)
        {
            _navAgent = actor.GetComponent<NavMeshAgent>();
            _navAgent.SetDestination(TargetPosition);
        }

        public override void Perform(Actor actor) { }

        public override bool IsComplete()
        {
            return Vector3.Distance(_navAgent.transform.position, TargetPosition) <= 0.5f;
        }

        public override void AffectOnKnowledge(ref Dictionary<string, object> knowledge, float efficiency)
        {
            if (knowledge.ContainsKey("yellowed "))
                knowledge["yellowed "] = true;
            else
                knowledge.Add("yellowed ", true);
        }
    }
}
