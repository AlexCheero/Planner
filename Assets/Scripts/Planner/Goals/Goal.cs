
using UnityEngine;

namespace GOAP
{
    [System.Serializable]
    public class Goal
    {
        [SerializeField]
        private EGoal _type;
        [SerializeField]
        private float _value;
        [SerializeField]
        private int _priorityPower;

        private float  _previousValue;
        private float _changeSinceLastTime;
        private float _timeSinceLastChange;

        private float _basicChangeRate;
        private float _basicRateShare;
        private float _dynamicRateShare;

        public float Value
        {
            get { return _value; }
            set { _value = Mathf.Max(0, value); }
        }

        public EGoal Type { get { return _type; } }

        public Goal(EGoal type, float value, int priorityPow = 2)
        {
            _type = type;
            _previousValue = Value = value;
            _priorityPower = priorityPow;
        }

        public Goal(Goal goal)
        {
            _type = goal._type;
            _previousValue = Value = goal.Value;
            _priorityPower = goal._priorityPower;
        }

        public float GetDiscontentment(float newValue)
        {
            return Mathf.Pow(newValue, _priorityPower);
        }

        public float GetDiscontentment()
        {
            return GetDiscontentment(Value);
        }

        public float GetChangeOverTime()
        {
            var rateSinceLastTime = _changeSinceLastTime / _timeSinceLastChange;
            _basicChangeRate = _basicRateShare * _basicChangeRate + _dynamicRateShare * rateSinceLastTime;

            return _basicChangeRate;
        }

        public void Update()
        {
            _changeSinceLastTime = Value - _previousValue;
            _timeSinceLastChange = Time.deltaTime;
            _previousValue = Value;
        }
    }
}
