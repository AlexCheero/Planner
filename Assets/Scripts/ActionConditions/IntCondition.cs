using System.Collections.Generic;

namespace GOAP
{
    public class IntCondition : ActionCondition
    {
        private readonly int _testValue;

        public IntCondition(string conditionPath, int testValue, ETest test)
        {
            ConditionPath = conditionPath;
            _testValue = testValue;
            _test = test;
        }

        public override bool TestCondition(Dictionary<string, object> knowledge)
        {
            if (!knowledge.ContainsKey(ConditionPath) || !(knowledge[ConditionPath] is int))
                return false;
            var valueToTest = (int)knowledge[ConditionPath];
            return TestInt(valueToTest, _test);
        }

        private bool TestInt(int num, ETest testSign)
        {
            bool success;
            switch (testSign)
            {
                case ETest.Equal:
                    success = num == _testValue;
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
                    success = num != _testValue;
                    break;
                default:
                    success = false;
                    break;
            }

            return success;
        }
    }
}