using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace AI
{
    public class PatrolBehavior : BaseStateBehavior
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

        public override void StartBehavior()
        {
            base.StartBehavior();
            agent.NavMeshNavMeshAgent.isStopped = false;
            isPatrolling = true;
            StartCoroutine(Patrol_CO());
        }

        private IEnumerator Patrol_CO()
        {
            while (isPatrolling)
            {
                if (waypoints.Length == 0)
                {
                    yield break;
                }
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

        public override void StopBehavior()
        {
            isPatrolling = false;
            StopAllCoroutines();
            agent.NavMeshNavMeshAgent.isStopped = true;
            base.StopBehavior();
        }
    }
}
