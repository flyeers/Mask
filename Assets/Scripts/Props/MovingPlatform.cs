using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField] private Transform[] points;   // Array of points to follow
    [SerializeField] private float speed = 2f;      // Movement speed
    [SerializeField] private float waitBetweenPoints= 0.5f;      

    private int currentPointIndex = 0;
    private CharacterController characterController = null;

    private bool isWaiting = false;


    void Update()
    {
        if (points.Length == 0 || isWaiting)
            return;

        // Move towards the current point
        transform.position = Vector3.MoveTowards(
            transform.position,
            points[currentPointIndex].position,
            speed * Time.deltaTime
        );
        if(characterController) characterController.Move((points[currentPointIndex].position - transform.position).normalized * speed * Time.deltaTime);
        
        // Check if the object reached the point
        if (Vector3.Distance(transform.position, points[currentPointIndex].position) < 0.1f)
        {
            StartCoroutine(WaitAndGoNext());
        }
    }

    IEnumerator WaitAndGoNext()
    {
        isWaiting = true;

        yield return new WaitForSeconds(waitBetweenPoints);

        currentPointIndex++;

        // Loop back to the first point
        if (currentPointIndex >= points.Length)
        {
            currentPointIndex = 0;
        }

        isWaiting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //other.gameObject.transform.parent.SetParent(transform);
            other.gameObject.TryGetComponent<CharacterController>(out characterController);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //other.gameObject.transform.parent.SetParent(null);
            characterController = null;
        }
    }
}
