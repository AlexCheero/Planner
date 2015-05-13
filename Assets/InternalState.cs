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