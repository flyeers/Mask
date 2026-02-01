using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovableSnap : MonoBehaviour
{
    [Header("Snap")]
    [SerializeField] private float snapMaxOffset = 0.25f; // cu�nto puede estar desalineada y a�n hacer snap
    [SerializeField] private float snapCooldown = 0.15f;  // evita snaps repetidos cada frame
    [SerializeField] private bool snapToGrid = false;
    [SerializeField] private float gridSize = 1f;

    private Rigidbody rb;
    private float nextSnapTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision col)
    {
        if (Time.time < nextSnapTime) return;

        // Solo nos interesa si chocamos con otra movable
        if (!col.collider.CompareTag("movable")) return;

        // Tiene que ser colisi�n "desde arriba" (la normal apunta hacia arriba)
        // Esto evita que haga snap cuando rozas de lado.
        bool fromAbove = false;
        for (int i = 0; i < col.contactCount; i++)
        {
            if (col.GetContact(i).normal.y > 0.5f)
            {
                fromAbove = true;
                break;
            }
        }
        if (!fromAbove) return;

        Rigidbody otherRb = col.collider.attachedRigidbody;
        if (otherRb == null) return;

        // Objetivo: centrar XZ respecto al objeto de abajo
        Vector3 target = rb.position;
        target.x = otherRb.position.x;
        target.z = otherRb.position.z;

        // Distancia en XZ respecto al centro
        Vector2 off = new Vector2(rb.position.x - target.x, rb.position.z - target.z);

        // Si est� demasiado lejos, no hacemos snap (para que no teletransporte raro)
        if (off.magnitude > snapMaxOffset) return;

        // Opcional: snap a grid
        if (snapToGrid && gridSize > 0.0001f)
        {
            target.x = Mathf.Round(target.x / gridSize) * gridSize;
            target.z = Mathf.Round(target.z / gridSize) * gridSize;
        }

        // Hacemos snap SOLO en XZ, manteniendo Y
        rb.position = new Vector3(target.x, rb.position.y, target.z);

        // Cortamos velocidad lateral para que no �patine�
        Vector3 v = rb.linearVelocity;
        rb.linearVelocity = new Vector3(0f, v.y, 0f);

        nextSnapTime = Time.time + snapCooldown;
    }
}
