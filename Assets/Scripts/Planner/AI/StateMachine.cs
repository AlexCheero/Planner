using UnityEngine;

namespace GOAP
{
    public class StateMachine : MonoBehaviour
    {
        public delegate bool State(StateMachine machine);

        private PlannerAction[] _actionSequence;
        public PlannerAction[] ActionSequence
        {
            get { return _actionSequence; }
            set { _actionSequence = value; }
        }

        void Start()
        {
            _actionSequence = new PlannerAction[0];
        }

        private int _actionIndex = 0;
        void Update()
        {
            if (ActionSequence.Length == 0 || _actionIndex >= ActionSequence.Length)
                return;

            var done = ActionSequence[_actionIndex].Perform(this);

            if (done)
                _actionIndex++;
        }
    }
}