using System;
using System.Collections;
using AI.Perception;
using Damage;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BaseAgent : MonoBehaviour,IDamageable
    {
        [SerializeField]
        private NavMeshAgent navMeshAgent;
        [SerializeField]
        private PerceptionBrain perceptionBrain;
        [SerializeField]
        private float rotationSpeed = 2f;
        private GameObject currentTarget;
        private PatrolBehavior patrolBehavior;
        private FollowBehavior followBehavior;
        private AttackBehavior attackBehavior;
        
        public GameObject CurrentTarget => perceptionBrain.GetCurrentTarget();
        public NavMeshAgent NavMeshNavMeshAgent => navMeshAgent;
        public PatrolBehavior PatrolBehavior => patrolBehavior;
        public FollowBehavior FollowBehavior => followBehavior;
        public AttackBehavior AttackBehavior => attackBehavior;

        bool isForgetting = false;
        private void Awake()
        {
            patrolBehavior = GetComponent<PatrolBehavior>();
            followBehavior = GetComponent<FollowBehavior>();
            attackBehavior = GetComponent<AttackBehavior>();
        }

        private void Update()
        {
            if (navMeshAgent.velocity.sqrMagnitude > 0 && navMeshAgent.velocity != Vector3.zero) {
                Vector3 direction = navMeshAgent.velocity.normalized;
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 
                    rotationSpeed * Time.deltaTime);
            }
        }

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
