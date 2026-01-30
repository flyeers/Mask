using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0f, 10f, -10f);

    private Vector3 velocity = Vector3.zero;
    private Quaternion originalLocalRotation;

    private void Awake()
    {
        originalLocalRotation = transform.localRotation;
    }

    void LateUpdate()
    {
        if (player == null)
        {
            Debug.LogError("NO PLAYER.");
            return;
        }

        Vector3 desiredPosition = player.position + offset;

        //move the camera to the desired position
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 1f / smoothSpeed);
        transform.localRotation = originalLocalRotation;

    }

    private void OnEnable()
    {
        Vector3 desiredPosition = player.position + offset;
        transform.position = desiredPosition;
        transform.localRotation = originalLocalRotation;
    }
}
