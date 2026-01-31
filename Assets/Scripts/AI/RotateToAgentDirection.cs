using UnityEngine;
using UnityEngine.AI;

public class RotateToAgentDirection : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent;
    [SerializeField]
    private float rotationSpeed = 2f;
    
    private void Update()
    {
        if (navMeshAgent.velocity.sqrMagnitude > 0 && navMeshAgent.velocity != Vector3.zero) {
            Vector3 direction = navMeshAgent.velocity.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 
                rotationSpeed * Time.deltaTime);
        }
    }
}
