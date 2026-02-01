using System.Collections;
using UnityEngine;

public class DestroyPopUp : MonoBehaviour
{
    [SerializeField] float timeToDestro = 5f;

    void OnEnable()
    {
        StartCoroutine(StartCountDown());
    }

    IEnumerator StartCountDown() 
    { 
        yield return new WaitForSeconds(timeToDestro);
        Destroy(gameObject);
    }
}
