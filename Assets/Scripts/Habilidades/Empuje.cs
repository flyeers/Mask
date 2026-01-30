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

    [Header("Movimiento (cinemático)")]
    [SerializeField] private float velocidadArrastre = 0.8f;   // “peso” = más lento
    [SerializeField] private float aceleracionArrastre = 10f;  // suavidad al arrancar/frenar

    private bool agarrando;
    private Rigidbody rb;
    private Transform tr;

    // lado donde está el player (desde centro de caja hacia el player)
    private Vector3 agarreAxisWorld;

    // para suavizar velocidad
    private float velActual;

    // para restaurar estado
    private bool rbEraKinematic;

    private bool agarreEsLateral;


    private void Awake()
    {
        if (playerInputController == null) playerInputController = GetComponent<PlayerInputController>();
        if (sensor == null) sensor = GetComponentInChildren<MovableSensor>();
        if (characterController == null) characterController = GetComponent<CharacterController>();

        if (playerInputController == null) Debug.LogError("No hay PlayerInputController");
        if (sensor == null) Debug.LogError("No hay MovableSensor en hijos");
        if (characterController == null) Debug.LogError("No hay CharacterController");

        if (thirdPersonController == null)
            thirdPersonController = GetComponent<ThirdPersonController>();

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

            if (thirdPersonController != null) thirdPersonController.SetCanMove(false);

            CalcularLadoMasCercano();

            // Guardar y forzar modo “cinemático” mientras arrastras
            rbEraKinematic = rb.isKinematic;
            //rb.isKinematic = true;
            //rb.freezeRotation = true;

            velActual = 0f;

            Debug.Log("Agarrado: " + rb.name);
        }
        else
        {
            agarrando = false;

            if (rb != null)
            {
                rb.isKinematic = rbEraKinematic;
                rb.freezeRotation = false;
            }

            rb = null;
            tr = null;

            if (thirdPersonController != null) thirdPersonController.SetCanMove(true);

            velActual = 0f;

            Debug.Log("Soltado");
        }
    }

    private void CalcularLadoMasCercano()
    {
        Vector3 local = tr.InverseTransformPoint(transform.position);
        local.y = 0f;

        float ax = Mathf.Abs(local.x);
        float az = Mathf.Abs(local.z);

        if (ax > az)
        {
            agarreEsLateral = true;
            float sign = Mathf.Sign(local.x);
            Vector3 axis = tr.right * sign;
            axis.y = 0f;
            agarreAxisWorld = axis.normalized;
        }
        else
        {
            agarreEsLateral = false;
            float sign = Mathf.Sign(local.z);
            Vector3 axis = tr.forward * sign;
            axis.y = 0f;
            agarreAxisWorld = axis.normalized;
        }
    }

    private void FixedUpdate()
    {
        if (!agarrando || rb == null || tr == null || playerInputController == null) return;

        // Eje de agarre
        Vector3 objetivoPlayer = rb.position + agarreAxisWorld * distanciaAgarre;
        objetivoPlayer.y = transform.position.y;

        Vector3 delta = objetivoPlayer - transform.position;
        delta.y = 0f;

        // Desplazamiento de recolocación (step)
        Vector3 step = Vector3.zero;
        if (delta.magnitude > tolerancia)
        {
            step = delta.normalized * (velocidadRecolocar * Time.fixedDeltaTime);
            if (step.magnitude > delta.magnitude) step = delta;
        }

        // Input
        Vector2 move = playerInputController.ReadMove();

        // eje permitido (hacia "dentro" del cubo)
        Vector3 dir = -agarreAxisWorld;

        // input en mundo según orientación del player
        Vector3 worldInput = transform.right * move.x + transform.forward * move.y;
        worldInput.y = 0f;

        // cuánto del input va en el eje permitido
        float v = Vector3.Dot(worldInput, dir);

        // opcional: clampa para que no se pase si vas en diagonal
        v = Mathf.Clamp(v, -1f, 1f);

        float velObjetivo = v * velocidadArrastre;
        velActual = Mathf.MoveTowards(velActual, velObjetivo, aceleracionArrastre * Time.fixedDeltaTime);

        Vector3 desplazamiento = Vector3.zero;
        if (Mathf.Abs(velActual) >= 0.001f)
        {
            desplazamiento = dir * (velActual * Time.fixedDeltaTime);

            // Mover caja
            rb.MovePosition(rb.position + desplazamiento);
        }

        // ✅ Mover player una sola vez
        characterController.Move(step + desplazamiento);
    }

}
