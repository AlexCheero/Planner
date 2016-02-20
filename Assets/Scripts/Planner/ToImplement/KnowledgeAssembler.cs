using UnityEngine;
using System.Collections.Generic;

namespace GOAP
{
    //this class and method GetInitialKnowledge should be implemented for specific usages
    public class KnowledgeAssembler : MonoBehaviour
    {
        [SerializeField]
        private float _searchRadius;
        public Dictionary<string, object> GetInitialKnowledge()
        {
            var knowledge = new Dictionary<string, object>();

            //check internal state of player to get internal knowledge
            knowledge.Add("stayed ", false);

            var colliders = Physics.OverlapSphere(transform.position, _searchRadius);
            for (var i = 0; i < colliders.Length; i++)
            {
                var coll = colliders[i];
                switch (coll.gameObject.name)
                {
                    case "GreenActionProvider":
                        knowledge.Add("green position ", coll.transform.position);
                        break;
                    case "YellowActionProvider":
                        knowledge.Add("yellow position ", coll.transform.position);
                        break;
                    case "RedActionProvider":
                        knowledge.Add("red position ", coll.transform.position);
                        break;
                }
            }

            return knowledge;
        }
    }
}
