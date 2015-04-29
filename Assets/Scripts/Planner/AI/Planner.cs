using System.Collections;
using UnityEngine;

namespace GOAP
{
    public class Planner : MonoBehaviour
    {
        public ActionBoard ActionBoard;
        public delegate void PlanningFinishedCallback(WorldModelNode node);

        private int _workingPlanRoutines;
        private float _bestDiscontentment;
        private WorldModelNode _bestNode;
        private PlanningFinishedCallback _callback;

        void Start()
        {
            BroadcastMessage("GetInternalActions");
        }

        public void Subscribe(PlanningFinishedCallback callback)
        {
            _callback = callback;
        }

        public IEnumerator CoroutinePlanActions(WorldModel model, int maxDepth)
        {
            _workingPlanRoutines = 1;
            _bestDiscontentment = Mathf.Infinity;
            StartCoroutine(PlanWorldActions(model, maxDepth, 0, new WorldModelNode(model)));

            while (_workingPlanRoutines > 0)
                yield return new WaitForEndOfFrame();

            _callback(_bestNode);
        }
        
        private IEnumerator PlanWorldActions(WorldModel model, int maxDepth, int currentDepth, WorldModelNode parentNode)
        {
            if (currentDepth >= maxDepth)
            {
                if (model.Discontentment < _bestDiscontentment)
                {
                    _bestDiscontentment = model.Discontentment;
                    _bestNode = parentNode;
                }

                yield return null;
            }
            foreach (var action in model.ActionsMembership)
            {
                var node = new WorldModelNode(model, action.First.BoardIndex, parentNode);
                var newModel = new WorldModel(model);
                StartCoroutine(PlanWorldActions(newModel, maxDepth, currentDepth + 1, node));
            }

            _workingPlanRoutines--;
            yield return null;
        }
    }
}
