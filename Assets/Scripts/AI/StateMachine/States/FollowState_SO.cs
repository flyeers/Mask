using UnityEngine;

namespace AI.StateMachine.States
{
    [CreateAssetMenu(fileName = "FollowState_SO", menuName = "Scriptable Objects/FollowState_SO")]
    public class FollowState_SO : State_SO
    {
        [SerializeField] private State_SO onTargetReached;
        [SerializeField] private float reachedDistance = 0.1f;
        public override void Enter(StateContext context)
        {
            Debug.Log("Entered FollowState_SO");
            context.StateMachine.GetOwnerAgent().FollowBehavior.StartFollowing();
        }

        public override void Execute(StateContext context, float deltaTime)
        {
            if (context.StateMachine.GetOwnerAgent().CurrentTarget == null)
            {
                context.RequestChangeState?.Invoke(nextState);
                return;
            }

            if (context.StateMachine.GetOwnerAgent().IsCloseToTarget(reachedDistance))
            {
                context.RequestChangeState?.Invoke(onTargetReached);
                return;
            }
        }

        public override void Exit(StateContext context)
        {
            context.StateMachine.GetOwnerAgent().FollowBehavior.StopFollowing();
            Debug.Log("Exiting FollowState_SO");
        }
    }
}
