using System.Collections;
using System.Linq;
using UnityEngine;

namespace GOAP
{
    public class Planner : MonoBehaviour
    {
        public int MaxDepth;
        public float DiscTestValue;

        public ActionBoard AllActions;

        void Start()
        {
            AllActions = new ActionBoard();
            BroadcastMessage("GetInternalActions");
            FindActionsInWorld();

            StartCoroutine(PlanActions());
        }

        void Update()
        {
            
        }

        private WorldModel GetInitialWorldModel()
        {
            return new WorldModel(new []{new Goal(EGoal.Goal, 20)}, new KnowledgeNode(), this);
        }

        private void FindActionsInWorld()
        {
            var colliders = Physics.OverlapSphere(transform.position, 20f).Where(collider => collider.GetComponent<ActionProvider>());
            foreach (var actionProvider in colliders.Select(col => col.GetComponent<ActionProvider>()))
                AllActions.AddActions(actionProvider.GetActions());
        }

        private IEnumerator PlanActions()
        {
            var model = GetInitialWorldModel();
            var maxDiscontentment = model.Goals.Select(goal => goal.GetDiscontentment(DiscTestValue)).Max();
            var table = new TranspositionTable();
            table.Add(model, 0);

            var modelsSequence = new WorldModel[MaxDepth + 1];
            var actionSequence = new PlannerAction[MaxDepth];

            var bestActionSequence = new PlannerAction[0];
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
                        bestActionSequence = (PlannerAction[])actionSequence.Clone();
                        bestModelsSequence = modelsSequence;
                    }

                    currentDepth--;
                    continue;
                }

                var nextAction = modelsSequence[currentDepth].NextAction();

                if (nextAction != null)
                {
                    actionSequence[currentDepth] = nextAction.First;
                    var nextDepth = currentDepth + 1;
                    var nextModel = modelsSequence[nextDepth] = new WorldModel(modelsSequence[currentDepth]);
                    nextModel.ApplyAction(nextAction);

                    //todo check logick
                    if (!table.Has(nextModel) && nextModel.Discontentment <= maxDiscontentment)
                        currentDepth++;

                    table.Add(nextModel, nextDepth);

                }
                else
                    currentDepth--;
            }

            foreach (var action in bestActionSequence)
                action.Perform();

            yield return 0;
        }
    }
}