using System;

namespace AI.StateMachine
{
    public class StateContext
    {
        public IStateMachine StateMachine { get; set; }
        public float StateTime { get; set; }

        public Action<IState> RequestChangeState;

        public StateContext(IStateMachine stateMachine)
        {
            StateMachine = stateMachine;
        }
        public void Reset()
        {
            StateTime = 0;
        }
    }
}
