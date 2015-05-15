using System.Collections;
using System.Linq;
using UnityEngine;

namespace GOAP
{
    public class Planner : MonoBehaviour
    {
        public float SearchRadius;
        public int MaxDepth;
        public float DiscTestValue;

        public AbstractActionBoard ActionBoard;

        void Start()
        {
            ActionBoard = new ActionBoard();
            GetKnowledge();

            StartCoroutine(PlanActions());
        }

        void Update()
        {
            
        }

        private WorldModel GetInitialWorldModel()
        {
            return new WorldModel(GetGoals(), GetKnowledge(), this);
        }

        private KnowledgeNode GetKnowledge()
        {
            var knowledge = new KnowledgeNode();
            
            //chechk internal state of player to get internal knowledge
            knowledge.SetValue(false, "stayed");

            var colliders = Physics.OverlapSphere(transform.position, SearchRadius);
            for (var i = 0; i < colliders.Length; i++)
            {
                var coll = colliders[i];
                switch (coll.gameObject.name)
                {
                    case "GreenActionProvider":
                        knowledge.SetValue(coll.transform.position, "green", "position");
                        break;
                    case "YellowActionProvider":
                        knowledge.SetValue(coll.transform.position, "yellow", "position");
                        break;
                    case "RedActionProvider":
                        knowledge.SetValue(coll.transform.position, "red", "position");
                        break;
                }
            }

            return knowledge;
        }

        private Goal[] GetGoals()
        {
            return new[] {new Goal(EGoal.Goal, 20)};
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
                    actionSequence[currentDepth] = nextAction;
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

            for (var i = 0; i < bestActionSequence.Length; i++)
            {
                var action = bestActionSequence[i];
                action.Perform();
            }

            yield return 0;
        }
    }
}