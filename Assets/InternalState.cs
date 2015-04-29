using System.Collections.Generic;
using GOAP;
using UnityEngine;

public class InternalState : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void GetInternalActions()
    {
        GetComponent<Planner>().ActionBoard.AddActions(new Action[0]);
    }
}

public class InternalAction : Action
{
    public InternalAction(Dictionary<EGoal, int> changes, byte membershipDegree = 0) : base(changes, membershipDegree)
    {
    }

    public override void Perform()
    {
        Debug.Log("Stay!");
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
