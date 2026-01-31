using Damage;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField] private float targetAngle = -90f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float waitTime = 1f;

    private Quaternion startRotation;
    private Quaternion endRotation;

    void Start()
    {
        startRotation = transform.localRotation;
        endRotation = startRotation * Quaternion.Euler(targetAngle, 0, 0);

        StartCoroutine(Rotate());
    }


    IEnumerator Rotate()
    {
        while (true) {

            float t = 0f;

            // Rotate to target angle (local)
            while (t < 1f)
            {
                t += Time.deltaTime * speed;
                transform.localRotation = Quaternion.Lerp(startRotation, endRotation, t);
                yield return null;
            }

            t = 0f;

            // Rotate back to original local rotation
            while (t < 1f)
            {
                t += Time.deltaTime * speed;
                transform.localRotation = Quaternion.Lerp(endRotation, startRotation, t);
                yield return null;
            }
            yield return new WaitForSeconds(waitTime);
        
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        { 
            if (other.GetComponentInParent<Damageable>())
            {
                //other.GetComponentInParent<Damageable>().Die();
                Debug.Log("die");
            } 
        }      
    }

}
