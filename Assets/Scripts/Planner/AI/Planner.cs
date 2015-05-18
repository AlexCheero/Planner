using System.Collections;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

namespace GOAP
{
    public class Planner : MonoBehaviour
    {
        public float SearchRadius;
        public int MaxDepth;
        public float DiscTestValue;

        public AbstractActionBoard ActionBoard;

        private StateMachine _machine;

        void Start()
        {
            ActionBoard = new ActionBoard();
            GetKnowledge();

            StartCoroutine(PlanActions());

            _machine = GetComponent<StateMachine>();
        }

        private bool _done = false;
        void Update()
        {
            if (_bestActionSequence == null || _bestActionSequence.Length == 0 || _done)
                return;

            _machine.ActionSequence = _bestActionSequence;
            _done = true;
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

        private PlannerAction[] _bestActionSequence;
        private WorldModel[] _bestModelsSequence;
        private IEnumerator PlanActions()
        {
            //todo think about how to quantize time for planning
            var model = GetInitialWorldModel();
            var maxDiscontentment = model.Goals.Select(goal => goal.GetDiscontentment(DiscTestValue)).Max();
            var table = new TranspositionTable();
            table.Add(model, 0);

            var modelsSequence = new WorldModel[MaxDepth + 1];
            var actionSequence = new PlannerAction[MaxDepth];

            _bestActionSequence = new PlannerAction[0];
            _bestModelsSequence = new WorldModel[0];

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
                        _bestActionSequence = (PlannerAction[])actionSequence.Clone();
                        _bestModelsSequence = modelsSequence;
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

            yield return 0;
        }
    }
}