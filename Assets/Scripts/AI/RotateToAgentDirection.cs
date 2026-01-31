using UnityEngine;
using UnityEngine.AI;

public class RotateToAgentDirection : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent navMeshAgent;
    [SerializeField]
    private float rotationSpeed = 2f;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    
    private void Update()
    {
        var isMoving = navMeshAgent.velocity.sqrMagnitude > 0f;
        
        if (isMoving && navMeshAgent.velocity != Vector3.zero) {
            Vector3 direction = navMeshAgent.velocity.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 
                rotationSpeed * Time.deltaTime);
        }
        
        _animator.SetBool("Move", isMoving);

        if (navMeshAgent.velocity.x > 0f)
        {
            _spriteRenderer.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (navMeshAgent.velocity.x < 0f)
        {
            _spriteRenderer.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
