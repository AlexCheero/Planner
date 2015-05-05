using GOAP;
using UnityEngine;

public class ActionProvider : MonoBehaviour
{
    public string Action;
    public int GoalChange;

    public Action[] GetActions()
    {
        switch (Action)
        {
            case "green":
                return new[] { new GreenAction(GoalChange) };
                break;
            case "red":
                return new[] { new RedAction(GoalChange),  };
                break;
            case "yellow":
                return new[] { new YellowAction(GoalChange),  };
                break;
            default:
                return new Action[0];
        }
        
    }
}