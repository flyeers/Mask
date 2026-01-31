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
        
        private GameObject currentTarget;
        
        public GameObject CurrentTarget => perceptionBrain.GetCurrentTarget();
        public NavMeshAgent NavMeshNavMeshAgent => navMeshAgent;
        public IStateBehavior PatrolBehavior => patrolBehavior;
        public IStateBehavior FollowBehavior => followBehavior;
        public IStateBehavior AttackBehavior => attackBehavior;

        bool isForgetting = false;

        private void FixedUpdate()
        {
            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit))
            {
                StopAllCoroutines();
                isForgetting = false;
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    currentTarget = hit.collider.gameObject;
                    return;
                }
            }

            if (!isForgetting)
            {
                StartCoroutine(ForgetTarget());
            }
        }

        private IEnumerator ForgetTarget()
        {
            isForgetting = true;
            yield return new WaitForSeconds(2f);
            currentTarget = null;
            isForgetting = false;
        }

        public bool IsCloseToTarget(float minDistance)
        {
            if (currentTarget == null) return false;
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
