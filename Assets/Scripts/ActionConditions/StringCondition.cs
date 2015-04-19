using System.Collections.Generic;

namespace GOAP
{
    public class StringCondition : ActionCondition
    {
        private readonly string _testValue;

        public StringCondition(string conditionPath, string testValue, ETest test)
        {
            ConditionPath = conditionPath;
            _testValue = testValue;
            _test = test;
        }

        public override bool TestCondition(Dictionary<string, object> knowledge)
        {
            if (!knowledge.ContainsKey(ConditionPath) || !(knowledge[ConditionPath] is string))
                return false;
            var valueToTest = (string)knowledge[ConditionPath];
            return TestString(valueToTest, _test);
        }

        private bool TestString(string num, ETest testSign)
        {
            bool success;
            switch (testSign)
            {
                case ETest.Equal:
                    success = num.Equals(_testValue);
                    break;
                case ETest.NotEqual:
                    success = !num.Equals(_testValue);
                    break;
                default:
                    success = false;
                    break;
            }

            return success;
        }
    }
}