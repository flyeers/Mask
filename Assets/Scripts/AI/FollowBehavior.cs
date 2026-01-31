using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class FollowBehavior : BaseStateBehavior
    {
        [SerializeField] private float reachedDistance = 0.1f;
        private BaseAgent agent;
        
        private Animator animator;

        bool isFollowing = false;
        
        private void Awake()
        {
            agent = GetComponent<BaseAgent>();
            animator = GetComponentInChildren<Animator>();
        }
        
        public override void StartBehavior()
        {
            base.StartBehavior();
            agent.NavMeshNavMeshAgent.isStopped = false;
            isFollowing = true;
            StartCoroutine(Follow_CO());
            animator.SetBool("Chase", true);
        }

        private IEnumerator Follow_CO()
        {
            while (isFollowing)
            {
                if (agent.CurrentTarget != null)
                {
                    NavMeshHit hit;
                    if (NavMesh.SamplePosition(agent.CurrentTarget.transform.position, out hit, 500f, NavMesh.AllAreas))
                    {
                        agent.NavMeshNavMeshAgent.ResetPath();
                        agent.NavMeshNavMeshAgent.SetDestination(hit.position);
                    }

                    yield return new WaitForSeconds(Time.fixedDeltaTime);
                }
            }
        }

        public override void StopBehavior()
        {
            isFollowing = false;
            StopAllCoroutines();
            agent.NavMeshNavMeshAgent.isStopped = true;
            base.StopBehavior();
            animator.SetBool("Chase", false);
        }

        public override bool Completed()
        {
            return agent.IsCloseToTarget(reachedDistance);
        }
    }
}
