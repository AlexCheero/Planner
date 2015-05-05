using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP
{
    public class Planner : MonoBehaviour
    {
        public ActionBoard AllActions;
        public int[] Actions;
        public WorldModel[] Models;

        void Start()
        {
            AllActions = new ActionBoard();
            BroadcastMessage("GetInternalActions");
            FindActionsInWorld();

            var initialWorldModel = GetInitialWorldModel();
            const float discTestValue = 100;
            var discontentment = initialWorldModel.Goals.Select(goal => goal.GetDiscontentment(discTestValue)).Max();
            PlanActions(initialWorldModel, 5, discontentment, out Actions, out Models);
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

        private void PlanActions(WorldModel model, int maxDepth, float maxDiscontentment, out int[] actions, out WorldModel[] models)
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
    }
}