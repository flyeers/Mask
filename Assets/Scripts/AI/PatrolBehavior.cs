using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class PatrolBehavior : MonoBehaviour
    {
        [SerializeField]
        Transform[] waypoints;
        [SerializeField]
        float timeBetweenWaypoints = 1f;
        
        int currentWaypointIndex = 0;
        private BaseAgent agent;

        private bool isPatrolling = false;
        private void Awake()
        {
            agent = GetComponent<BaseAgent>();
            
        }

        private void Start()
        {
            enabled = false;
        }

        public void StartPatrolling()
        {
            enabled = true;
            agent.NavMeshNavMeshAgent.isStopped = false;
            isPatrolling = true;
            StartCoroutine(Patrol_CO());
        }

        private IEnumerator Patrol_CO()
        {
            while (isPatrolling)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(waypoints[currentWaypointIndex].position, out hit, 500f, NavMesh.AllAreas))
                {
                    agent.NavMeshNavMeshAgent.ResetPath();
                    agent.NavMeshNavMeshAgent.SetDestination(hit.position);
                }

                ++currentWaypointIndex;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = 0;
                }
                yield return new WaitForSeconds(Time.fixedDeltaTime);
                yield return new WaitUntil(()=> agent.NavMeshNavMeshAgent.remainingDistance <= agent.NavMeshNavMeshAgent.stoppingDistance);
                yield return new WaitForSeconds(timeBetweenWaypoints);
            }
        }

        public void StopPatrolling()
        {
            isPatrolling = false;
            StopAllCoroutines();
            agent.NavMeshNavMeshAgent.isStopped = true;
            enabled = false;
        }
    }
}
