using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP
{
    public class Planner : MonoBehaviour
    {
        public int MaxDepth;
        public float DiscTestValue;

        public ActionBoard AllActions;
        public int[] Actions;
        public WorldModel[] Models;

        void Start()
        {
            AllActions = new ActionBoard();
            BroadcastMessage("GetInternalActions");
            FindActionsInWorld();

            var initialWorldModel = GetInitialWorldModel();
            var maxDiscontentment = initialWorldModel.Goals.Select(goal => goal.GetDiscontentment(DiscTestValue)).Max();
            PlanActions(initialWorldModel, maxDiscontentment, out Actions, out Models);
            foreach (var action in Actions)
                AllActions[action].Perform();
        }

        private WorldModel GetInitialWorldModel()
        {
            return new WorldModel(new []{new Goal(EGoal.Goal, 20)}, new Dictionary<string, object>(), this);
        }

        private void FindActionsInWorld()
        {
            var colliders = Physics.OverlapSphere(transform.position, 20f).Where(collider => collider.GetComponent<ActionProvider>());
            foreach (var col in colliders)
            {
                var actionProvider = col.GetComponent<ActionProvider>();
                AllActions.AddActions(actionProvider.GetActions());
            }
        }

        private void PlanActions(WorldModel model, float maxDiscontentment, out int[] actions, out WorldModel[] models)
        {
            var modelsSequence = new WorldModel[MaxDepth + 1];
            var actionSequence = new int[MaxDepth];

            var bestActionSequence = new int[0];
            var bestModelsSequence = new WorldModel[0];

            modelsSequence[0] = model;
            var currentDepth = 0;

            var bestDiscontentment = Mathf.Infinity;

            while (currentDepth >= 0)
            {
                var currentDiscontentment = modelsSequence[currentDepth].Discontentment;
                if (currentDepth >= MaxDepth)
                {
                    if (currentDiscontentment < bestDiscontentment)
                    {
                        bestDiscontentment = currentDiscontentment;
                        bestActionSequence = (int[])actionSequence.Clone();
                        bestModelsSequence = modelsSequence;
                    }

                    currentDepth--;
                    continue;
                }

                var nextAction = modelsSequence[currentDepth].NextAction();

                if (nextAction != null)
                {
                    actionSequence[currentDepth] = nextAction.First.BoardIndex;
                    WorldModel nextModel;
                    nextModel = modelsSequence[currentDepth + 1] = new WorldModel(modelsSequence[currentDepth]);
                    nextModel.ApplyAction(nextAction);

                    if (nextModel.Discontentment <= maxDiscontentment)
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
    }
}