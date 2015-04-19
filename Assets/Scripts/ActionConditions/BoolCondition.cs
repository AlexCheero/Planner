using System.Collections.Generic;

namespace GOAP
{
    public class BoolCondition : ActionCondition
    {
        public BoolCondition(string conditionPath, ETest test)
        {
            ConditionPath = conditionPath;
            _test = test;
        }

        public override bool TestCondition(Dictionary<string, object> knowledge)
        {
            if (!knowledge.ContainsKey(ConditionPath) || !(knowledge[ConditionPath] is bool))
                return false;
            var valueToTest = (bool)knowledge[ConditionPath];
            return TestBool(valueToTest, _test);
        }

        private bool TestBool(bool value, ETest testSign)
        {
            var success = true;
            switch (testSign)
            {
                case ETest.IsTrue:
                    success = value;
                    break;
                case ETest.IsFalse:
                    success = !value;
                    break;
                default:
                    success = false;
                    break;
            }

            return success;
        }
    }
}