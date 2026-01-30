using UnityEngine;

namespace AI.StateMachine.States
{
    [CreateAssetMenu(fileName = "IdleState_SO", menuName = "Scriptable Objects/IdleState_SO")]
    public class IdleState_SO : State_SO
    {
        [SerializeField] private float timeInIdle = 2f;

        public override void Enter(StateContext context)
        {
        }

        public override void Execute(StateContext context, float deltaTime)
        {
            if (context.StateTime >= timeInIdle)
            {
                context.RequestChangeState?.Invoke(nextState);
            }
        }

        public override void Exit(StateContext context)
        {
            Debug.Log("Exiting");
        }
    }
}
