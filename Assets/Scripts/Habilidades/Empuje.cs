using UnityEngine;
using Input; // tu namespace donde está PlayerInputController

public class Empuje : MonoBehaviour
{
    [SerializeField] private float fuerzaEmpuje = 5f;
    [SerializeField] private PlayerInputController playerInputController;

    private Rigidbody rbMovable;
    private Transform trMovable;
    private bool puedeAgarrar;
    private bool agarrando;

    private void Awake()
    {
        if (playerInputController == null)
            playerInputController = GetComponent<PlayerInputController>();

        if (playerInputController == null)
            Debug.LogError("Empuje: No se encontró PlayerInputController en el mismo GameObject.");
    }

    private void OnEnable()
    {
        if (playerInputController != null)
            playerInputController.UseAbility += PushToggle;
    }

    private void OnDisable()
    {
        if (playerInputController != null)
            playerInputController.UseAbility -= PushToggle;
    }

    private void PushToggle()
    {
        if (!puedeAgarrar || rbMovable == null) return;

        agarrando = !agarrando;

        // opcional: que no rote mientras lo empujas/arrastras
        rbMovable.freezeRotation = agarrando;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("movable")) return;

        rbMovable = collision.rigidbody;
        trMovable = collision.transform;
        puedeAgarrar = (rbMovable != null);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!collision.gameObject.CompareTag("movable")) return;

        puedeAgarrar = false;
        agarrando = false;

        if (rbMovable != null)
            rbMovable.freezeRotation = false;

        rbMovable = null;
        trMovable = null;
    }

    private void FixedUpdate()
    {
        if (!agarrando || rbMovable == null || trMovable == null || playerInputController == null) return;

        // leemos W/S (o stick) desde Input System
        Vector2 move = playerInputController.ReadMove();
        float v = move.y;

        if (Mathf.Abs(v) < 0.01f) return;

        // eje forward/back DEL OBJETO (caja)
        Vector3 axis = trMovable.forward;
        axis.y = 0f;
        axis.Normalize();

        rbMovable.AddForce(axis * (v * fuerzaEmpuje), ForceMode.Force);
    }
}
