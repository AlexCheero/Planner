using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP
{
    public class Planner : MonoBehaviour
    {
        [SerializeField]
        private float _searchRadius;
        [SerializeField]
        private int _maxDepth;
        [SerializeField]
        private float _maxAllowableDiscontentment;

        private ActionPerformer _actionPerformer;

        void Start()
        {
            _actionPerformer = GetComponent<ActionPerformer>();
            _table = new WMTranspositionTable();

            //this call should be after everything inited
            PlanActions();
        }

        public void PlanActions()
        {
            if (_planning)
            {
#if UNITY_EDITOR
                Debug.LogWarning("Try to start planning second time, while first plan isn't created");
#endif
                return;
            }
            StartCoroutine(PlanActionsCoroutine());
        }

        private bool _planReady;
        void Update()
        {
            if (!_planReady)
                return;

            _actionPerformer.SetActions(_bestActionSequence);
            _planReady = false;
        }

        private WorldModel GetInitialWorldModel()
        {
            return new WorldModel(GetGoals(), GetInitialKnowledge());
        }

        private Dictionary<string, object> GetInitialKnowledge()
        {
            var knowledge = new Dictionary<string, object>();
            
            //check internal state of player to get internal knowledge
            knowledge.Add("stayed ", false);

            var colliders = Physics.OverlapSphere(transform.position, _searchRadius);
            for (var i = 0; i < colliders.Length; i++)
            {
                var coll = colliders[i];
                switch (coll.gameObject.name)
                {
                    case "GreenActionProvider":
                        knowledge.Add("green position ", coll.transform.position);
                        break;
                    case "YellowActionProvider":
                        knowledge.Add("yellow position ", coll.transform.position);
                        break;
                    case "RedActionProvider":
                        knowledge.Add("red position ", coll.transform.position);
                        break;
                }
            }

            return knowledge;
        }

        private Goal[] GetGoals()
        {
            //still the state machine is better to choose only one main goal
            return new[] {new Goal(EGoal.Goal, 20)};
        }

        private PlannerAction[] _bestActionSequence;
        private WorldModel[] _bestModelsSequence;
        private WMTranspositionTable _table;
        private bool _planning;
        private IEnumerator PlanActionsCoroutine()
        {
            //todo think about how to quantize time for planning
            _planning = true;
            var model = GetInitialWorldModel();
            var maxDiscontentment = model.Goals.Select(goal => goal.GetDiscontentment(_maxAllowableDiscontentment)).Max();
            _table.Clear();
            _table.Add(model, 0);

            var modelsSequence = new WorldModel[_maxDepth + 1];
            var actionSequence = new PlannerAction[_maxDepth];

            _bestActionSequence = new PlannerAction[0];
            _bestModelsSequence = new WorldModel[0];

            modelsSequence[0] = model;
            var currentDepth = 0;

            var bestDiscontentment = Mathf.Infinity;

            while (currentDepth >= 0)
            {
                currentDepth = Search(modelsSequence, currentDepth, actionSequence, maxDiscontentment,
                    ref bestDiscontentment);
            }

            _planReady = true;
            _planning = false;
            yield return 0;
        }

        //probably this is not a good idea to extract this code out of while loop into method body
        private int Search(WorldModel[] modelsSequence, int currentDepth, PlannerAction[] actionSequence,
            float maxDiscontentment, ref float bestDiscontentment)
        {
            var currentDiscontentment = modelsSequence[currentDepth].Discontentment;
            //try to generate plans not for the whole depth, if goal is achieved, than its not necessary to keep searching
            if (currentDepth >= _maxDepth)
            {
                if (currentDiscontentment < bestDiscontentment)
                {
                    bestDiscontentment = currentDiscontentment;
                    _bestActionSequence = (PlannerAction[]) actionSequence.Clone();
                    _bestModelsSequence = modelsSequence;
                }

                currentDepth--;
                return currentDepth;
            }

            var nextAction = modelsSequence[currentDepth].NextAction();

            if (nextAction != null)
            {
                actionSequence[currentDepth] = nextAction;
                var nextDepth = currentDepth + 1;
                var nextModel = modelsSequence[nextDepth] = new WorldModel(modelsSequence[currentDepth]);
                nextModel.ApplyAction(nextAction);

                //todo check logick
                if (!_table.Has(nextModel) && nextModel.Discontentment <= maxDiscontentment)
                    currentDepth++;

                _table.Add(nextModel, nextDepth);
            }
            else
                currentDepth--;
            return currentDepth;
        }
    }
}