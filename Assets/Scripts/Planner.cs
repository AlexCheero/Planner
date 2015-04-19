using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP
{
    public class Planner : MonoBehaviour
    {
        public float ActionSearchDistance = 10;

        public Goal[] Goals = { new Goal(EGoal.Goal, 100) };
        public List<Action> PossibleActions = new List<Action>(); 

        void Start()
        {
            FindPossibleActions();
        }

        void FixedUpdate()
        {
            foreach (var goal in Goals)
                goal.Update();
        }
        
        void Update()
        {
            ChooseAction().Perform();
        }

        private void FindPossibleActions()
        {
            PossibleActions.Clear();
            foreach (var action in GetInternalActions())
                PossibleActions.Add(action);

            var actionProviders = GameObject.FindGameObjectsWithTag("ActionProvider");
            foreach (var provider in actionProviders.Where(o => 
                Vector3.Distance(transform.position, o.transform.position) <= ActionSearchDistance))
            {
                RaycastHit rayHit;
                var hitted = Physics.Raycast(transform.position, provider.transform.position - transform.position, out rayHit,
                    ActionSearchDistance);
                if (hitted && !rayHit.collider.gameObject.Equals(provider.gameObject))
                    continue;
                PossibleActions.Add(provider.GetComponent<ActionProvider>().ProvidedAction);
            }
        }

        private Action ChooseAction()
        {
            var bestGoal = Goals[0];
            for (var i = 1; i < Goals.Length; i++)
            {
                if (Goals[i].Value > bestGoal.Value)
                    bestGoal = Goals[i];
            }

            var bestAction = PossibleActions[0];
            var bestDiscontentment = CalculateDiscontentment(bestAction);

            for (var i = 1; i < PossibleActions.Count; i++)
            {
                var discontentment = CalculateDiscontentment(PossibleActions[i]);
                if (discontentment < bestDiscontentment)
                {
                    bestDiscontentment = discontentment;
                    bestAction = PossibleActions[i];
                }
            }

            return bestAction;
        }

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

        private float CalculateDiscontentment(Action action)
        {
            var discontentment = 0f;

            foreach (var goal in Goals)
            {
                var newGoalValue = goal.Value + action.GetGoalChange(goal);
                newGoalValue += action.GetDuration() * goal.GetChangeOverTime();

                discontentment += goal.GetDiscontentment(newGoalValue);
            }

            return discontentment;
        }

        private Action[] GetInternalActions()
        {
            return new Action[]
            {
                new Action("Shoot", new Dictionary<EGoal, int>() {{EGoal.Goal, -35}}),
                new Action("Heal", new Dictionary<EGoal, int>() {{EGoal.Goal, -50}})
            };
        }
    }
}
