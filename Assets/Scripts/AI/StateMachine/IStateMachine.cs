using UnityEngine;

namespace AI.StateMachine
{
    public interface IStateMachine
    {
        public void Initialize();
        public void ChangeState(IState newState);
        public BaseAgent GetOwnerAgent();
    }
}
