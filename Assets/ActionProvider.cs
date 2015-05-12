using GOAP;
using UnityEngine;

public class ActionProvider : MonoBehaviour
{
    public string Action;
    public int GoalChange;

    public PlannerAction[] GetActions()
    {
        switch (Action)
        {
            case "green":
                return new PlannerAction[] { new GreenPlannerAction(GoalChange) };
                break;
            case "red":
                return new PlannerAction[] { new RedPlannerAction(GoalChange) };
                break;
            case "yellow":
                return new PlannerAction[] { new YellowPlannerAction(GoalChange) };
                break;
            default:
                return new PlannerAction[0];
        }
        
    }
}