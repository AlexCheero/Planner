﻿using UnityEngine;

namespace GOAP
{
    public class Performer : MonoBehaviour
    {
        private PlannerAction[] _actions;
        private WorldModel[] _models;
        private Planner _planner;
        private Actor _actor;

        void Start()
        {
            _planner = GetComponent<Planner>();
            _actor = GetComponent<Actor>();
        }

        private int _actionIndex = 0;
        void Update()
        {
            if (!_actionsSetted || _actions.Length == 0)
                return;

            if (_actionIndex >= _actions.Length)
                Replan();
            else
                UpdateCurrentAction();
        }

        private void UpdateCurrentAction()
        {
            var currAction = _actions[_actionIndex];

            if (!currAction.IsStarted)
                currAction.StartAction(_actor);
            else if (currAction.IsComplete())
                _actionIndex++;
            else
                currAction.Perform(_actor);
        }

        private void Replan()
        {
            _actionsSetted = false;
            _actionIndex = 0;
            _planner.PlanActions();
        }

        private bool _actionsSetted;
        public void SetPlan(PlannerAction[] actions, WorldModel[] models)
        {
            _actions = actions;
            _models = models;
            _actionIndex = 0;
            _actionsSetted = true;
        }
    }
}