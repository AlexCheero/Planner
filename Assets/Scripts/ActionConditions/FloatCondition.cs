using System;
using System.Collections.Generic;

namespace GOAP
{
    public class FloatCondition : ActionCondition
    {
        private readonly float _testValue;

        public FloatCondition(string conditionPath, float testValue, ETest test)
        {
            ConditionPath = conditionPath;
            _testValue = testValue;
            _test = test;
        }

        public override bool TestCondition(Dictionary<string, object> knowledge)
        {
            if (!knowledge.ContainsKey(ConditionPath) || !(knowledge[ConditionPath] is float))
                return false;
            var valueToTest = (float)knowledge[ConditionPath];
            return TestFloat(valueToTest, _test);
        }

        private bool TestFloat(float num, ETest testSign)
        {
            bool success;
            switch (testSign)
            {
                case ETest.Equal:
                    success = Math.Abs(num - _testValue) < float.Epsilon;
                    break;
                case ETest.MoreOrEqual:
                    success = num >= _testValue;
                    break;
                case ETest.LessOrEqual:
                    success = num <= _testValue;
                    break;
                case ETest.More:
                    success = num > _testValue;
                    break;
                case ETest.Less:
                    success = num < _testValue;
                    break;
                case ETest.NotEqual:
                    success = Math.Abs(num - _testValue) > float.Epsilon;
                    break;
                default:
                    success = false;
                    break;
            }

            return success;
        }
    }
}