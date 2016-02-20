using UnityEngine;

namespace GOAP
{
    public class ActionPerformer : MonoBehaviour
    {
        public delegate bool State(ActionPerformer machine);

        private PlannerAction[] _actionSequence;
        private Planner _planner;
        private Actor _actor;

        void Start()
        {
            _planner = GetComponent<Planner>();
            _actor = GetComponent<Actor>();
        }

        private int _actionIndex = 0;
        void Update()
        {
            if (!_actionsSetted || _actionSequence.Length == 0)
                return;

            if (_actionSequence[_actionIndex].Perform(_actor))
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
        }
    }
}