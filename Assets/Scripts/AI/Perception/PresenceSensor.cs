using System;
using UnityEngine;

namespace AI.Perception
{
    [RequireComponent(typeof(SphereCollider))]
    public class PresenceSensor : BaseSensor
    {
        [SerializeField] 
        private string perceivedTag = "Player";
        
        private void OnTriggerEnter(Collider other)
        {
            if (currentTarget == null && other.CompareTag(perceivedTag) && CheckHeight(other.transform.position))
            {
                currentTarget = other.gameObject;
                NotifySensed();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject == currentTarget)
            {
                if (!CheckHeight(currentTarget.transform.position))
                {
                    currentTarget = null;
                    NotifyForgotten();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == currentTarget)
            {
                currentTarget = null;
                NotifyForgotten();
            }
        }
    }
}
