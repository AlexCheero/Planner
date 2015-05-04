﻿using UnityEngine;

namespace GOAP
{
    public class Planner : MonoBehaviour
    {
        private void PlanActions(WorldModel model, int maxDepth, out int[] actions, out WorldModel[] models)
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
    }
}