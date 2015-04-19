using System.Collections.Generic;

namespace GOAP
{
    public abstract class ActionCondition
    {
        protected string ConditionPath;
        protected ETest _test;
        public abstract bool TestCondition(Dictionary<string, object> knowledge);
    }
}