using UnityEngine;

namespace AI.StateMachine.States
{
    [CreateAssetMenu(fileName = "PatrolState_SO", menuName = "Scriptable Objects/PatrolState_SO")]
    public class PatrolState_SO : State_SO
    {
        public override void Enter(StateContext context)
        {
            Debug.Log("Entering PatrolState");
            context.StateMachine.GetOwnerAgent().PatrolBehavior.StartPatrolling();
        }

        public override void Execute(StateContext context, float deltaTime)
        {
            if (context.StateMachine.GetOwnerAgent().CurrentTarget != null)
            {
                context.RequestChangeState?.Invoke(nextState);
            }
        }

        public override void Exit(StateContext context)
        {
            Debug.Log("Exit PatrolState");
            context.StateMachine.GetOwnerAgent().PatrolBehavior.StopPatrolling();
        }
    }
}
