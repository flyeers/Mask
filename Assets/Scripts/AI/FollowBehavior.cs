using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class FollowBehavior : BaseStateBehavior
    {
        private BaseAgent agent;

        bool isFollowing = false;
        
        private void Awake()
        {
            agent = GetComponent<BaseAgent>();
        }
        
        public override void StartBehavior()
        {
            base.StartBehavior();
            agent.NavMeshNavMeshAgent.isStopped = false;
            isFollowing = true;
            StartCoroutine(Follow_CO());
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
        }
    }
}
