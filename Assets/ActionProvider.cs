using GOAP;
using UnityEngine;

public class ActionProvider : MonoBehaviour
{
    public string Action;
    public int GoalChange;

    public Command[] GetActions()
    {
        switch (Action)
        {
            case "green":
                return new[] { new GreenCommand(GoalChange) };
                break;
            case "red":
                return new[] { new RedCommand(GoalChange),  };
                break;
            case "yellow":
                return new[] { new YellowCommand(GoalChange),  };
                break;
            default:
                return new Command[0];
        }
        
    }
}