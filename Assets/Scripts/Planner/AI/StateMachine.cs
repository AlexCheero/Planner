using UnityEngine;

namespace GOAP
{
    public class StateMachine : MonoBehaviour
    {
        public delegate bool State(StateMachine machine);

        private bool _haveActions;

        private PlannerAction[] _actionSequence;
        public PlannerAction[] ActionSequence
        {
            get { return _actionSequence; }
            set
            {
                _actionSequence = value;
                _haveActions = true;
            }
        }

        void Start()
        {

        }

        private int _stateIndex = 0;
        void Update()
        {
            if (!_haveActions || _stateIndex >= ActionSequence.Length)
                return;

            var done = ActionSequence[_stateIndex].Perform(this);

            if (done)
                _stateIndex++;
        }
    }
}