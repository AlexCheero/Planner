using System.Collections;
using UnityEngine;

namespace GOAP
{
    public class Planner : MonoBehaviour
    {
        public delegate void PlanningFinishedCallback(WorldModelNode node);

        private int _workingPlanRoutines;
        private float _bestDiscontentment;
        private WorldModelNode _bestNode;
        private PlanningFinishedCallback _callback;

        public void Subscribe(PlanningFinishedCallback callback)
        {
            _callback = callback;
        }

        public void PlanActions(WorldModel model, int maxDepth, out int[] actions, out WorldModel[] models)
        {
            var modelsSequence = new WorldModel[maxDepth + 1];
            var actionSequence = new int[maxDepth];

            var bestActionSequence = new int[0];
            var bestModelsSequence = new WorldModel[0];

            modelsSequence[0] = model;
            var currentDepth = 0;

            var bestDiscontentment = Mathf.Infinity;

            while (currentDepth >= 0)
            {
                var currentDiscontentment = modelsSequence[currentDepth].Discontentment;
                if (currentDepth >= maxDepth)
                {
                    if (currentDiscontentment < bestDiscontentment)
                    {
                        bestDiscontentment = currentDiscontentment;
                        bestActionSequence = actionSequence;
                        bestModelsSequence = modelsSequence;
                    }

                    currentDepth--;
                    continue;
                }

                var nextAction = modelsSequence[currentDepth].NextAction();

                if (nextAction.First != null)
                {
                    actionSequence[currentDepth] = nextAction.First.BoardIndex;
                    modelsSequence[currentDepth + 1] = new WorldModel(modelsSequence[currentDepth]);
                    modelsSequence[currentDepth + 1].ApplyAction(nextAction);

                    currentDepth++;
                }
                else
                {
                    currentDepth--;
                }
            }

            actions = bestActionSequence;
            models = bestModelsSequence;
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
