using Damage;
using UnityEngine;

namespace AI.StateMachine.States
{
    [CreateAssetMenu(fileName = "AttackState_SO", menuName = "Scriptable Objects/AttackState_SO")]
    public class AttackState_SO : State_SO
    {
        public override void Enter(StateContext context)
        {
            Debug.Log("Enter Attack");
            context.StateMachine.GetOwnerAgent().AttackBehavior.AttackTarget(context.StateMachine.GetOwnerAgent().CurrentTarget.GetComponentInParent<IDamageable>());
            context.RequestChangeState?.Invoke(nextState);
        }

        public override void Execute(StateContext context, float deltaTime)
        {
            
        }

        public override void Exit(StateContext context)
        {
            Debug.Log("Exit Attack");
        }
    }
}
