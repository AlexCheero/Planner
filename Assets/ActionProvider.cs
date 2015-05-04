using System.Collections.Generic;
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

public class GreenAction : Action
{
    public GreenAction(int change)
        : base(new Dictionary<EGoal, int> { { EGoal.Goal, change } })
    {
    }

    public override void Perform()
    {
        Debug.LogError("Green!");
    }

    public override int GetDuration(Dictionary<string, object> knowledge)
    {
        return 0;
    }

    public override void AffectOnKnowledge(ref Dictionary<string, object> knowledge, float membership)
    {

    }

    public override byte GetMembership(Dictionary<string, object> knowledge)
    {
        return 255;
    }
}
