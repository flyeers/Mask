using System;
using UnityEngine;

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
    }
}
