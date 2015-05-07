using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class ExampleCommand : Command
    {
        public ExampleCommand(Dictionary<EGoal, int> changes)
            : base(changes)
        {
        }

        public override void Perform()
        {
            throw new System.NotImplementedException();
        }

        public override int GetDuration(Dictionary<string, object> knowledge)
        {
            throw new System.NotImplementedException();
        }

        public override void AffectOnKnowledge(ref Dictionary<string, object> knowledge, float membership)
        {
            //change knowledge that certanly will be changed and try to predict dynamic knowledge
        }

        public override byte GetMembership(Dictionary<string, object> knowledge)
        {
            var enemyVisionLength = 20;//or something like that
            var positionArray = knowledge.GetObjByKeyPath("position") as float[];
            var position = new Vector3(positionArray[0], positionArray[1], positionArray[2]);
            var enemies = knowledge.GetObjByKeyPath("enemies") as Dictionary<string, object>;
            var sightlessEnemiesCount = 0;
            foreach (var enemy in enemies)
            {
                var enemyPositionArray = (enemy.Value as Dictionary<string, object>).GetObjByKeyPath("position") as float[];
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