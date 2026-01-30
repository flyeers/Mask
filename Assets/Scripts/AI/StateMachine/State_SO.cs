using UnityEngine;

namespace AI.StateMachine
{
    public abstract class State_SO : ScriptableObject,IState
    {
     
        [SerializeField]
        protected State_SO nextState;
        
        public abstract void Enter(StateContext context);

        public abstract void Execute(StateContext context, float deltaTime);

        public abstract void Exit(StateContext context);
    }
}
