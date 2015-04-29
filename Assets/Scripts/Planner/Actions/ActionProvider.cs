using UnityEngine;

namespace GOAP
{

    //class-ZALUPA
    public class ActionProvider : MonoBehaviour
    {
        public EGoal Goal;
        public int Value;
        public string Action;

        public Action ProvidedAction;

        void Awake()//todo code, to init provider before initing planner in start method, because of null actions
        {
//            ProvidedAction = new Action(Action, new Dictionary<EGoal, int>() { { Goal, Value } });
        }

        public Action[] GetPossibleActions()
        {
            return new Action[1];
        }
    }
}
