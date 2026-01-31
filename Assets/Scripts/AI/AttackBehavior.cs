using Damage;
using UnityEngine;

namespace AI
{
    public class AttackBehavior : BaseStateBehavior
    {
        private BaseAgent agent;
        private Animator animator;

        private void Awake()
        {
            agent = GetComponent<BaseAgent>();
            animator = GetComponentInChildren<Animator>();
        }

        public void AttackTarget(IDamageable target)
        {
            if (target == null) return;
        
            target.Die();
        }

        public override void StartBehavior()
        {
            base.StartBehavior();
            AttackTarget(agent.CurrentTarget.GetComponent<IDamageable>());

            if (animator != null)
            {
                animator.SetBool("Chase", true);
            }
        }

        public override void StopBehavior()
        {
            base.StopBehavior();

            if (animator != null)
            {
                animator.SetBool("Chase", false);
            }
        }
    }
}
