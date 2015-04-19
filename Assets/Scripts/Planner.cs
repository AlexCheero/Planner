using System.Collections.Generic;
using UnityEngine;

namespace GOAP
{
    public class Planner
    {
        //todo try use multithreading to increase speed
        private int[] PlanActions(WorldModel model, int maxDepth)
        {
            var models = new WorldModel[maxDepth + 1];
            var actionSequences = new List<int[]>();
            var canCreateNewSequence = true;
            var currentSequenceIndex = 0;
            var bestActionSequence = new int[0];

            models[0] = model;
            var currentDepth = 0;

            var bestDiscontentment = Mathf.Infinity;

            while (currentDepth >= 0)
            {
                var currentDiscontentment = models[currentDepth].Discontentment;
                if (currentDepth >= maxDepth)
                {
                    if (currentDiscontentment < bestDiscontentment)
                    {
                        bestDiscontentment = currentDiscontentment;
                        bestActionSequence = actionSequences[currentSequenceIndex];
                    }

                    canCreateNewSequence = true;
                    currentDepth--;
                    continue;
                }

                var nextAction = models[currentDepth].NextAction();

                if (nextAction != null)
                {
                    if (canCreateNewSequence)
                    {
                        actionSequences.Add((int[])actionSequences[currentSequenceIndex].Clone());
                        currentSequenceIndex++;
                        canCreateNewSequence = false;
                    }
                    actionSequences[currentSequenceIndex][currentDepth] = nextAction.BoardIndex;
                    models[currentDepth + 1] = new WorldModel(models[currentDepth]);
                    models[currentDepth + 1].ApplyAction(nextAction);

                    currentDepth++;
                }
                else
                {
                    currentDepth--;
                }
            }

            return bestActionSequence;
        }
    }
}
