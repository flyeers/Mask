using UnityEngine;

namespace AI.StateMachine
{
    public class StateMachine : MonoBehaviour,IStateMachine
    {
        [SerializeField] 
        protected State_SO initialState;
        
        protected IState currentState;
        protected StateContext currentContext;
        public void Initialize()
        {
            currentContext = new StateContext();
            ChangeState(initialState);
        }

        public void ChangeState(IState newState)
        {
            if (currentState != null)
            {
                currentState.Exit(currentContext);
                currentContext.Reset();
            }

            if (newState != null)
            {
                currentState = newState;
                currentState.Enter(currentContext);
            }
        }

        public GameObject GetOwnerGameObject()
        {
            return gameObject;
        }
    }
}
