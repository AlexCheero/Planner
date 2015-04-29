
using UnityEngine;

namespace GOAP
{
    public class Goal
    {
        public EGoal Name;
        public float Value;
        private float _previousValue;

        private float _changeSinceLastTime;
        private float _timeSinceLastChange;

        public int PriorityPower = 2;

        private float _basicChangeRate;
        public float BasicRateShare;
        public float DynamicRateShare;

        public Goal(EGoal name, float value)
        {
            Name = name;
            _previousValue = Value = value;
        }

        public Goal(Goal goal)
        {
            Name = goal.Name;
            _previousValue = Value = goal.Value;
        }

        public float GetDiscontentment(float newValue)
        {
            return (int)Mathf.Pow(newValue, PriorityPower);
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
            _timeSinceLastChange = Time.fixedDeltaTime;
            _previousValue = Value;
        }
    }
}
