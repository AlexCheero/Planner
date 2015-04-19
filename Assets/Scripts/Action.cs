using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP
{
    public class Action
    {
        private List<ActionCondition> _requiredConditions;
        private List<KnowledgeImpaction> _knowledgeImpactions;
        private readonly Dictionary<EGoal, int> _goalChanges;
        private string _action;

        public Action()
        {
            //stub
        }

        public Action(string actionDescription, Dictionary<EGoal, int> changes)
        {
            _action = actionDescription;
            _goalChanges = changes;
        }

        public int GetGoalChange(Goal goal)
        {
            var name = goal.Name;
            return _goalChanges.ContainsKey(name) ? _goalChanges[name] : 0;
        }

        public void Perform()
        {
            Debug.Log(_action);
        }

        public int GetDuration()
        {
            return 1;
        }

        public void AffectOnKnowledge(ref Dictionary<string, object> knowledge)
        {
            foreach (var knowledgeImpaction in _knowledgeImpactions)
                knowledgeImpaction.ImpactKnowledge(ref knowledge);
        }

        public bool CheckConditions(Dictionary<string, object> knowledge)
        {
            return _requiredConditions.Aggregate(true,
                (current, condition) => current & condition.TestCondition(knowledge));
        }
    }
}