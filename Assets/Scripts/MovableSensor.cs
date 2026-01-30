using UnityEngine;

public class MovableSensor : MonoBehaviour
{
    public Rigidbody Current { get; private set; }
    public Transform CurrentTransform { get; private set; }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("movable")) return;

        Current = other.attachedRigidbody;
        CurrentTransform = other.transform;

        Debug.Log("Sensor detecta movable: " + other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("movable")) return;

        if (other.attachedRigidbody == Current)
        {
            Current = null;
            CurrentTransform = null;
            Debug.Log("Sensor salió de movable: " + other.name);
        }
    }
}
