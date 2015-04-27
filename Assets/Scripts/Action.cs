using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public abstract class Action
    {
//        private List<ActionCondition> _requiredConditions;
//        private List<KnowledgeImpaction> _knowledgeImpactions;
        private readonly Dictionary<EGoal, int> _goalChanges;
        private string _action;
        public byte MinMembershipDegree { get; protected set; }

        public int BoardIndex { get; private set; }//todo crutch
        
        public Action(string actionDescription, Dictionary<EGoal, int> changes, byte membershipDegree = 0)
        {
            BoardIndex = 0;//todo crutch also

            _action = actionDescription;
            _goalChanges = changes;
            MinMembershipDegree = membershipDegree;
        }

        public int GetGoalChange(Goal goal)
        {
            var name = goal.Name;
            return _goalChanges.ContainsKey(name) ? _goalChanges[name] : 0;
        }

        public void Perform()
        {
            Debug.Log(_action);
        }

        public int GetDuration()
        {
            return 1;
        }

        public abstract void AffectOnKnowledge(ref Dictionary<string, object> knowledge);

        public abstract byte CheckConditions(Dictionary<string, object> knowledge);
    }

    public class ActionDummy : Action
    {
        public ActionDummy(string actionDescription, Dictionary<EGoal, int> changes) : base(actionDescription, changes)
        {
        }

        public override void AffectOnKnowledge(ref Dictionary<string, object> knowledge)
        {
            throw new System.NotImplementedException();
        }

        public override byte CheckConditions(Dictionary<string, object> knowledge)
        {
            throw new System.NotImplementedException();
        }
    }

    public class TakeCoverAction : Action
    {
        public TakeCoverAction(string actionDescription, Dictionary<EGoal, int> changes) : base(actionDescription, changes)
        {
        }

        public override void AffectOnKnowledge(ref Dictionary<string, object> knowledge)
        {
            //change knowledge that certanly will be changed and try to predict dynamic knowledge
        }

        public override byte CheckConditions(Dictionary<string, object> knowledge)
        {
            var enemyVisionLength = 20;//or something like that
            var positionArray = knowledge["position"] as float[];//with checks and etc.
            var position = new Vector3(positionArray[0], positionArray[1], positionArray[2]);
            var enemies = knowledge["enemies"] as Dictionary<string, object>;
            var sightlessEnemiesCount = 0;
            foreach (var enemy in enemies)
            {
                var enemyPositionArray = (enemy.Value as Dictionary<string, object>)["position"] as float[];//with checks and etc.
                var enemyPosition = new Vector3(enemyPositionArray[0], enemyPositionArray[1], enemyPositionArray[2]);
                RaycastHit hit;
                if (Physics.Raycast(enemyPosition, position, out hit, enemyVisionLength) && hit.collider.gameObject.tag == "me")//check only as example
                    sightlessEnemiesCount++;
            }

            //change goals with conditions memberships
            return (byte)(255 * (sightlessEnemiesCount / enemies.Count));
        }
    }
}