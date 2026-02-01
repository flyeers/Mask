using System.Collections;
using UnityEngine;

namespace AI
{
    public class RotateFollowBehavior : BaseStateBehavior
    {
        [SerializeField] 
        private float angleReachThreshold = 10f;
        [SerializeField]
        float rotationSpeed = 45;

        public float angleClamp = 45f;
        private BaseAgent agent;
        
        private bool isFollowing = false;
        private Quaternion initialRotation;
        private Vector3 initialForward;
        private float previousAngle = 0;
        
        private void Awake()
        {
            agent = GetComponent<BaseAgent>();
            initialRotation = transform.rotation;
            initialForward = transform.forward.normalized;
        }

        public override void StartBehavior()
        {
            base.StartBehavior();
            isFollowing = true;
            previousAngle = AngleToTarget();
            StartCoroutine(RotateTowards_CO(agent.CurrentTarget));
        }

        public override void StopBehavior()
        {
            isFollowing = false;
            StopAllCoroutines();
            base.StopBehavior();
        }

        private IEnumerator RotateTowards_CO(GameObject target)
        {
            while (isFollowing)
            {
                if (agent.CurrentTarget != null)
                {
                    Vector3 directionToTarget = target.transform.position - transform.position;
                    directionToTarget = directionToTarget.normalized;
                    Vector3 projectedDir = Vector3.ProjectOnPlane(directionToTarget, Vector3.up).normalized;
                    
                    float angle = Vector3.SignedAngle(initialForward, projectedDir, Vector3.up);
                    angle = Mathf.Clamp(angle, -angleClamp, angleClamp);
                    Quaternion targetRotation = initialRotation * Quaternion.AngleAxis(angle, Vector3.up);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    yield return null;
                }
            }
        }

        
        public override bool Completed()
        {
            // float angle = AngleToTarget();
            // if (angle < angleReachThreshold || angle - previousAngle < 0.1f)
            // {
            //     return true;
            // }
            // previousAngle = angle;
            return true;
        }

        private float AngleToTarget()
        {
            Vector3 directionToTarget = agent.CurrentTarget.transform.position - transform.position;
            directionToTarget = directionToTarget.normalized;
            Vector3 projectedDir = Vector3.ProjectOnPlane(directionToTarget, Vector3.up).normalized;
                    
            float angle = Vector3.SignedAngle(transform.forward, projectedDir, Vector3.up);
            return angle;
        }
    }
}
