using UnityEngine;

namespace GOAP
{
    public class Performer : MonoBehaviour
    {
        private PlannerAction[] _actions;
        private WorldModel[] _models;
        private Planner _planner;
        private Actor _actor;

        void Start()
        {
            _planner = GetComponent<Planner>();
            _actor = GetComponent<Actor>();
        }

        private int _planIndex = 0;
        void Update()
        {
            if (!_planSetted || _actions.Length == 0)
                return;

            if (_planIndex >= _actions.Length/* || !CheckPlan()*/)
                Replan();
            else
                UpdateCurrentAction();
        }

        private void UpdateCurrentAction()
        {
            var currAction = _actions[_planIndex];

            if (!currAction.IsStarted)
                currAction.StartAction(_actor);
            else if (currAction.IsComplete())
                _planIndex++;
            else
                currAction.Perform(_actor);
        }

        private void Replan()
        {
            _planSetted = false;
            _planIndex = 0;
            _planner.PlanActions();
        }

        private bool CheckPlan()
        {
            var currModel = _models[_planIndex];
            var realModel = _planner.GetRealWorldModel();

//            Debug.Log("check plan curr: " + currModel + ", real: " + realModel + ", index: " + _planIndex);
//            Debug.Log("goals cur: " + currModel.Goals.Length + ", real: " + realModel.Goals.Length);
//            Debug.Log("actions cur: " + currModel._actions.Count + ", real: " + realModel._actions.Count);
//            Debug.Log("knowl: " + currModel._knowledge.CheckEquality(realModel._knowledge));
//            Debug.Log("disc cur: " + currModel.Discontentment + ", real: " + realModel.Discontentment);

//            var currKnowl = currModel._knowledge;
//            var realKnowl = realModel._knowledge;
//            Debug.Log("count curr: " + currKnowl.Count + ", real: " + realKnowl.Count);
//            
//            var currVal = "";
//            foreach (var pair in currKnowl)
//                currVal += "key: " + pair.Key + " val: " + pair.Value + "\n";
//            Debug.Log("curr data:\n" + currVal);
//
//            var realVal = "";
//            foreach (var pair in realKnowl)
//                realVal += "key: " + pair.Key + " val: " + pair.Value + "\n";
//            Debug.Log("real data:\n" + realVal);

            return realModel.ApproxEquals(currModel);
        }

        private bool _planSetted;
        public void SetPlan(PlannerAction[] actions, WorldModel[] models)
        {
            if (actions.Length == 0)
            {
                Debug.LogWarning("zero plan");
                Replan();
                return;
            }

            if (actions.Length != models.Length - 1)
            {
                Debug.LogError("wrong actions (" + actions.Length + ") and models (" + models.Length +
                               ") sizes: models count must be exactly one unit more");
                Replan();
                return;
            }

            _actions = actions;
            _models = models;
            _planIndex = 0;
            _planSetted = true;
        }
    }
}