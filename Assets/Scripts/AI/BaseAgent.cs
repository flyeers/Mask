using System;
using System.Collections;
using AI.Perception;
using Damage;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class BaseAgent : MonoBehaviour,IDamageable
    {
        [SerializeField]
        private NavMeshAgent navMeshAgent;
        [SerializeField]
        private PerceptionBrain perceptionBrain;
        [SerializeField]
        private BaseStateBehavior patrolBehavior;
        [SerializeField]
        private BaseStateBehavior followBehavior;
        [SerializeField]
        private BaseStateBehavior attackBehavior;
        
        public GameObject CurrentTarget => perceptionBrain.GetCurrentTarget();
        public NavMeshAgent NavMeshNavMeshAgent => navMeshAgent;
        public IStateBehavior PatrolBehavior => patrolBehavior;
        public IStateBehavior FollowBehavior => followBehavior;
        public IStateBehavior AttackBehavior => attackBehavior;

        private void Awake()
        {
            if (navMeshAgent != null)
            {
                navMeshAgent.updateRotation = false;
            }
        }

        public bool IsCloseToTarget(float minDistance)
        {
            if (CurrentTarget == null) return false;
            return (CurrentTarget.transform.position - transform.position).sqrMagnitude <= minDistance * minDistance;
        }

        public void Die()
        {
            Destroy(gameObject);
            OnDeath?.Invoke();  
        }

        public event Action OnDeath;
    }
}
