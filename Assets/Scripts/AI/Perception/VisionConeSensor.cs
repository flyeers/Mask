using System;
using UnityEngine;

namespace AI.Perception
{
    [RequireComponent(typeof(SphereCollider))]
    public class VisionConeSensor : BaseSensor
    {
        [SerializeField]
        private float fovAngle = 60.0f;
        
        [SerializeField] 
        private string perceivedTag = "Player";

        
        private float cosHalfFov;
        private void Awake()
        {
            cosHalfFov = Mathf.Cos(fovAngle * Mathf.Deg2Rad * 0.5f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (currentTarget == null && other.CompareTag(perceivedTag))
            {
                if (CheckCone(other.gameObject.transform.position))
                {
                    currentTarget = other.gameObject;
                    NotifySensed();
                }
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject == currentTarget)
            {
                if (!CheckCone(currentTarget.transform.position))
                {
                    currentTarget = null;
                    NotifyForgotten();
                    return;
                }
            }
            if (currentTarget == null && other.CompareTag(perceivedTag))
            {
                if (CheckCone(other.gameObject.transform.position))
                {
                    currentTarget = other.gameObject;
                    NotifySensed();
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
        
        private bool CheckCone(Vector3 targetPosition)
        {
            if (!CheckHeight(targetPosition)) return false;
            Vector3 direction = targetPosition - transform.position;
            float dot = Vector3.Dot(transform.forward, direction.normalized);
            return dot >= cosHalfFov;
        }
        
    }
}
