using System.Collections;
using UnityEngine;

namespace AI
{
    public class RotatePatrolBehavior : BaseStateBehavior
    {
        [SerializeField] 
        public float minAngle = -90f;
        [SerializeField] 
        public float maxAngle = 90f;
        [SerializeField] 
        [Tooltip("Degrees per second")]
        public float rotationSpeed = 45f;
        [SerializeField]
        float timeBetweenWaypoints = 1f;
        
        private BaseAgent agent;
        private bool isPatrolling = false;
        
        private float currentAngle = 0f;
        private bool movingRight = true;
        
        int currentWaypointIndex = 0;
        private Vector3 initialEulers;
        private void Awake()
        {
            agent = GetComponent<BaseAgent>();
            initialEulers = transform.localEulerAngles;
        }
        
        public override void StartBehavior()
        {
            base.StartBehavior();
            isPatrolling = true;
            StartCoroutine(Patrol_CO());
        }

        public override void StopBehavior()
        {
            isPatrolling = false;
            StopAllCoroutines();
            base.StopBehavior();
        }
        private IEnumerator Patrol_CO()
        {
            while (isPatrolling)
            {
                float direction = movingRight ? 1f : -1f;
                currentAngle += direction * rotationSpeed * Time.deltaTime;
                
                if (currentAngle >= maxAngle)
                {
                    currentAngle = maxAngle;
                    movingRight = false;
                    yield return new WaitForSeconds(timeBetweenWaypoints);
                }
                else if (currentAngle <= minAngle)
                {
                    currentAngle = minAngle;
                    movingRight = true;
                    yield return new WaitForSeconds(timeBetweenWaypoints);
                }

                float patrolY = Mathf.Clamp(currentAngle, minAngle, maxAngle);
                Quaternion targetLocal = Quaternion.Euler(initialEulers.x, patrolY, initialEulers.z);
                transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetLocal, rotationSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }
}
