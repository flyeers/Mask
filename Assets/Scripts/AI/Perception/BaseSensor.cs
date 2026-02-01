using System;
using UnityEngine;
using UnityEngine.AI;

namespace AI.Perception
{
    public abstract class BaseSensor : MonoBehaviour,ISensor
    {
        public event Action<ISensor> Sensed;
        public event Action<ISensor> Forgotten;

        protected enum EMaskMode
        {
            Any = 0,
            All = 1
        }
        [SerializeField]
        protected EMaskMode maskMode = EMaskMode.Any;
        [SerializeField] 
        protected Perceivable.EEntityType sensedTypesMask;

        [Header("ONLY DEBUG")]
        [SerializeField]
        protected GameObject currentTarget = null;
        [Space]
        [SerializeField]
        protected int priority = 0;
        [SerializeField]
        protected float minYOffset = -0.5f;
        [SerializeField]
        protected float maxYOffset = 1.5f;
        [SerializeField]
        private NavMeshAgent navMeshAgent;
        

        public int GetPriority()
        {
            return priority;
        }

        public GameObject GetCurrentTarget()
        {
            return currentTarget;
        }

        protected void NotifySensed()
        {
            Sensed?.Invoke(this);
        }

        protected void NotifyForgotten()
        {
            Forgotten?.Invoke(this);
        }
        
        protected bool CheckHeight(Vector3 targetPosition)
        {
            return targetPosition.y > transform.position.y + minYOffset && targetPosition.y < transform.position.y + maxYOffset;
        }
        protected bool CanIgnore(GameObject sensedObject)
        {
            Perceivable perceivable = sensedObject.GetComponent<Perceivable>();
            if (perceivable == null || !perceivable.enabled)
            {
                return true;
            }
            switch (maskMode)
            {
                case EMaskMode.All:
                    return (sensedTypesMask & perceivable.TypeMask) != sensedTypesMask;
                case EMaskMode.Any:
                default:
                    return (sensedTypesMask & perceivable.TypeMask) == 0;
            }
        }
        protected bool CheckLineOfSight(GameObject targetCandidate)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, targetCandidate.transform.position - transform.position, out hit))
            {
                if (hit.collider.gameObject == targetCandidate)
                {
                    if (navMeshAgent != null)
                    {
                        return IsPointAccessible(targetCandidate.transform.position);
                    }
                    return true;
                }
            }
            return false;
        }

        protected bool IsPointAccessible(Vector3 targetPosition)
        {
            NavMeshHit hit;
            bool foundPosition = NavMesh.SamplePosition(targetPosition, out hit, 50f, NavMesh.AllAreas);
    
            if (!foundPosition)
                return false;
    
            NavMeshPath path = new NavMeshPath();
            bool pathFound = NavMesh.CalculatePath(transform.position, hit.position, NavMesh.AllAreas, path);
    
            return pathFound && path.status == NavMeshPathStatus.PathComplete;
        }
    }
}
