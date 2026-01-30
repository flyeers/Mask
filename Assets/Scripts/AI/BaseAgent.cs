using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BaseAgent : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent navMeshAgent;
        [SerializeField]
        private float rotationSpeed = 2f;
        private GameObject currentTarget;
        private PatrolBehavior patrolBehavior;
        private FollowBehavior followBehavior;
        
        public GameObject CurrentTarget => currentTarget;
        public NavMeshAgent NavMeshNavMeshAgent => navMeshAgent;
        public PatrolBehavior PatrolBehavior => patrolBehavior;
        public FollowBehavior FollowBehavior => followBehavior;

        bool isForgetting = false;
        private void Awake()
        {
            patrolBehavior = GetComponent<PatrolBehavior>();
            followBehavior = GetComponent<FollowBehavior>();
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
            if(Physics.Raycast(transform.position, Vector3.forward, out RaycastHit hit))
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
    }
}
