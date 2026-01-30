using System;
using UnityEngine;
using UnityEngine.AI;

namespace AI.StateMachine
{
    public class StateMachine : MonoBehaviour,IStateMachine
    {
        [SerializeField] 
        protected State_SO initialState;
        
        protected IState currentState;
        protected StateContext currentContext;
        
        bool isTransitioning = false;

        BaseAgent agent;
        private void Awake()
        {
            agent = GetComponent<BaseAgent>();
        }

        public void Start()
        {
            Initialize();
        }

        private void Update()
        {
            if (currentState == null)
            {
                return;
            }
            currentState.Execute(currentContext, Time.deltaTime);
            currentContext.StateTime +=  Time.deltaTime;
        }

        public void Initialize()
        {
            currentContext = new StateContext(this);
            currentContext.RequestChangeState = ChangeState;
            ChangeState(initialState);
        }

        public void ChangeState(IState newState)
        {
            if (isTransitioning) return;
            isTransitioning = true;
            if (currentState != null)
            {
                currentState.Exit(currentContext);
                currentContext.Reset();
            }

            currentState = newState;
            if (newState != null)
            {
                currentState.Enter(currentContext);
            }
            isTransitioning = false;
        }

        public BaseAgent GetOwnerAgent()
        {
            return agent;
        }
    }
}
