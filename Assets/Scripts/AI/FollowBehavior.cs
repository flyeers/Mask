using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class FollowBehavior : MonoBehaviour
    {
        private BaseAgent agent;

        bool isFollowing = false;
        
        private void Awake()
        {
            agent = GetComponent<BaseAgent>();
        }

        private void Start()
        {
            enabled = false;
        }

        public void StartFollowing()
        {
            enabled = true;
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

        public void StopFollowing()
        {
            isFollowing = false;
            StopAllCoroutines();
            agent.NavMeshNavMeshAgent.isStopped = true;
            enabled = false;
        }
    }
}
