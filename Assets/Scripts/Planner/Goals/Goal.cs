
using UnityEngine;

namespace GOAP
{
    public class Goal
    {
        public readonly EGoal Type;
        private float _value;
        public float Value
        {
            get { return _value; }
            set { _value = Mathf.Max(0, value); }
        }

        private float _previousValue;
        private float _changeSinceLastTime;
        private float _timeSinceLastChange;

        private readonly int _priorityPower;

        private float _basicChangeRate;
        public float BasicRateShare;
        public float DynamicRateShare;

        public Goal(EGoal type, float value, int priorityPow = 2)
        {
            Type = type;
            _previousValue = Value = value;
            _priorityPower = priorityPow;
        }

        public Goal(Goal goal)
        {
            Type = goal.Type;
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
            _basicChangeRate = BasicRateShare * _basicChangeRate + DynamicRateShare * rateSinceLastTime;

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
