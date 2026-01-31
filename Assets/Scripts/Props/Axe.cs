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
        startRotation = transform.rotation;
        endRotation = Quaternion.Euler(0, 0, targetAngle);

        StartCoroutine(Rotate());
    }


    IEnumerator Rotate()
    {
        while (true) { 

            float t = 0f;

            // Rotate to target angle
            while (t < 1f)
            {
                t += Time.deltaTime * speed;
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
                yield return null;
            }

            t = 0f;

            // Rotate back to original rotation
            while (t < 1f)
            {
                t += Time.deltaTime * speed;
                transform.rotation = Quaternion.Lerp(endRotation, startRotation, t);
                yield return null;
            }
            yield return new WaitForSeconds(waitTime);
        
        }

    }

    void OnTriggerEnter(Collider other)
    {
       /* if (other.CompareTag("Player"))

        other.GetComponentInParent<Damageable>();
        {
            if ()
            {
                damageable.Die();
                Debug.Log("die");
            }
        }*/
    }

    /*void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.TryGetComponent<Damageable>(out Damageable damageable))
            {
                damageable.Die();
                Debug.Log("die");
            }
        }
    }*/

}
