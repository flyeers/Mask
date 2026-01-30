using UnityEngine;
using Input;

public class Empuje : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private PlayerInputController playerInputController;
    [SerializeField] private MovableSensor sensor;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private ThirdPersonController thirdPersonController;

    [Header("Agarre")]
    [SerializeField] private float distanciaAgarre = 0.9f;
    [SerializeField] private float velocidadRecolocar = 6f;
    [SerializeField] private float tolerancia = 0.05f;

    [Header("Movimiento de caja")]
    [SerializeField] private float fuerzaEmpuje = 10f;

    private bool agarrando;
    private Rigidbody rb;
    private Transform tr;

    // Dirección desde el centro de la caja hacia el lado donde está el player (en mundo)
    private Vector3 agarreAxisWorld;

    private void Awake()
    {
        if (playerInputController == null) playerInputController = GetComponent<PlayerInputController>();
        if (sensor == null) sensor = GetComponentInChildren<MovableSensor>();
        if (characterController == null) characterController = GetComponent<CharacterController>();

        if (playerInputController == null) Debug.LogError("No hay PlayerInputController");
        if (sensor == null) Debug.LogError("No hay MovableSensor en hijos");
        if (characterController == null) Debug.LogError("No hay CharacterController");
    }

    private void OnEnable()
    {
        if (playerInputController != null)
            playerInputController.UseAbility += ToggleAgarrar;
    }

    private void OnDisable()
    {
        if (playerInputController != null)
            playerInputController.UseAbility -= ToggleAgarrar;
    }

    private void ToggleAgarrar()
    {
        if (!agarrando)
        {
            if (sensor.Current == null) return;

            rb = sensor.Current;
            tr = sensor.CurrentTransform;
            if (rb == null || tr == null) return;

            agarrando = true;
            rb.freezeRotation = true;

            if (thirdPersonController != null) thirdPersonController.enabled = false;

            // ✅ Calcular el lado más cercano según por dónde se acercó el player
            CalcularLadoMasCercano();

            Debug.Log("Agarrado: " + rb.name);
        }
        else
        {
            agarrando = false;

            if (rb != null) rb.freezeRotation = false;
            rb = null; tr = null;

            if (thirdPersonController != null) thirdPersonController.enabled = true;

            Debug.Log("Soltado");
        }
    }

    private void CalcularLadoMasCercano()
    {
        // Posición del player en espacio local de la caja
        Vector3 local = tr.InverseTransformPoint(transform.position);
        local.y = 0f;

        float ax = Mathf.Abs(local.x);
        float az = Mathf.Abs(local.z);

        if (ax > az)
        {
            // Más cerca de una cara lateral: +right o -right
            float sign = Mathf.Sign(local.x);
            Vector3 axis = tr.right * sign;
            axis.y = 0f;
            agarreAxisWorld = axis.normalized;
        }
        else
        {
            // Más cerca de una cara frontal/trasera: +forward o -forward
            float sign = Mathf.Sign(local.z);
            Vector3 axis = tr.forward * sign;
            axis.y = 0f;
            agarreAxisWorld = axis.normalized;
        }
    }

    private void FixedUpdate()
    {
        if (!agarrando || rb == null || tr == null || playerInputController == null) return;

        // 1) Recolocar al player en el lado elegido (pegado al cubo)
        Vector3 objetivoPlayer = rb.position + agarreAxisWorld * distanciaAgarre;
        objetivoPlayer.y = transform.position.y;

        Vector3 delta = objetivoPlayer - transform.position;
        delta.y = 0f;

        if (delta.magnitude > tolerancia)
        {
            Vector3 step = delta.normalized * (velocidadRecolocar * Time.fixedDeltaTime);
            if (step.magnitude > delta.magnitude) step = delta;
            characterController.Move(step);
        }

        // 2) Empujar/arrastrar la caja con W/S (sin inercia)
        float v = playerInputController.ReadMove().y;

        if (Mathf.Abs(v) < 0.01f)
        {
            Vector3 vel = rb.linearVelocity;
            rb.linearVelocity = new Vector3(0f, vel.y, 0f);
            rb.angularVelocity = Vector3.zero;
            return;
        }

        // Empuje hacia "dentro" de la caja (opuesto al lado donde estás)
        Vector3 empujeDir = -agarreAxisWorld;
        rb.AddForce(empujeDir * (v * fuerzaEmpuje), ForceMode.Force);
    }
}
