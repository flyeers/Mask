using System;
using Damage;
using UnityEngine;

namespace AI
{
    public class AttackBehavior : BaseStateBehavior
    {
        private BaseAgent agent;

        private void Awake()
        {
            agent = GetComponent<BaseAgent>();
        }

        public void AttackTarget(IDamageable target)
        {
            if (target == null) return;
        
            target.Die();
        }

        public override void StartBehavior()
        {
            base.StartBehavior();
            AttackTarget(agent.CurrentTarget.GetComponentInParent<IDamageable>());
        }

        public override void StopBehavior()
        {
            base.StopBehavior();
        }
    }
}
