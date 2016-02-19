using UnityEngine;

namespace GOAP
{
    public class ActionPerformer : MonoBehaviour
    {
        public delegate bool State(ActionPerformer machine);

        private PlannerAction[] _actionSequence;
        private Planner _planner;

        void Start()
        {
            _planner = GetComponent<Planner>();
        }

        private int _actionIndex = 0;
        void Update()
        {
            if (!_actionsSetted || _actionSequence.Length == 0)
                return;

            Debug.Log("PerformerUpdate _actionIndex: " + _actionIndex + ", _actionSequence length: " + _actionSequence.Length);
            if (_actionSequence[_actionIndex].Perform(this))
                _actionIndex++;

            if (_actionIndex < _actionSequence.Length)
                return;

            _actionsSetted = false;
            _actionIndex = 0;
            _planner.PlanActions();
        }

        private bool _actionsSetted;
        public void SetActions(PlannerAction[] actions)
        {
            _actionSequence = actions;
            _actionIndex = 0;
            _actionsSetted = true;
            Debug.Log("SetActions: action sequence length: " + _actionSequence.Length + ", index: " + _actionIndex);
        }
    }
}