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
        {

            playerInputController.UseAbility += ToggleAgarrar;
        Debug.Log("Empuje suscrito a UseAbility");
        }
    }

    private void OnDisable()
    {
        if (playerInputController != null)
            playerInputController.UseAbility -= ToggleAgarrar;
    }

    private void ToggleAgarrar()
    {
        Debug.Log("ToggleAgarrar llamado. sensor.Current = " +
                  (sensor != null ? (sensor.Current != null ? sensor.Current.name : "NULL") : "sensor NULL"));

        if (!agarrando)
        {
            if (sensor.Current == null) return;

            rb = sensor.Current;
            tr = sensor.CurrentTransform;
            if (rb == null || tr == null) return;

            agarrando = true;

            if (thirdPersonController != null) thirdPersonController.SetCanMove(false);

            CalcularLadoMasCercano();

            // --- MODO ARRASTRE (controlado por script) ---
            rbEraKinematic = rb.isKinematic;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.isKinematic = true;                 // la moveremos con MovePosition
            rb.useGravity = false;                 // opcional mientras arrastras (evita rarezas)
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            velActual = 0f;

            Debug.Log("Agarrado: " + rb.name);
        }
        else
        {
            // --- SOLTAR ---
            agarrando = false;

            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                rb.isKinematic = false;            // vuelve a física normal
                rb.useGravity = true;

                // ✅ No se mueve en el suelo por empujones: bloquea X/Z, pero deja Y para gravedad
                rb.constraints = RigidbodyConstraints.FreezePositionX |
                                 RigidbodyConstraints.FreezePositionZ |
                                 RigidbodyConstraints.FreezeRotation;
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

        // 1) Recolocar player al punto de agarre
        Vector3 objetivoPlayer = rb.position + agarreAxisWorld * distanciaAgarre;
        objetivoPlayer.y = transform.position.y;

        Vector3 delta = objetivoPlayer - transform.position;
        delta.y = 0f;

        Vector3 step = Vector3.zero;
        if (delta.magnitude > tolerancia)
        {
            step = delta.normalized * (velocidadRecolocar * Time.fixedDeltaTime);
            if (step.magnitude > delta.magnitude) step = delta;
        }

        // 2) Input -> velocidad deseada a lo largo del eje permitido
        Vector2 move = playerInputController.ReadMove();

        // eje permitido (hacia dentro del cubo)
        Vector3 dir = -agarreAxisWorld;

        // input a mundo según orientación del player
        Vector3 worldInput = transform.right * move.x + transform.forward * move.y;
        worldInput.y = 0f;

        float v = Vector3.Dot(worldInput, dir);
        v = Mathf.Clamp(v, -1f, 1f);

        // Si no hay input, NO muevas la caja, solo recoloca player
        float dead = 0.05f;
        if (Mathf.Abs(v) < dead)
        {
            velActual = 0f;
            characterController.Move(step);
            return;
        }

        float velObjetivo = v * velocidadArrastre;
        velActual = Mathf.MoveTowards(velActual, velObjetivo, aceleracionArrastre * Time.fixedDeltaTime);

        // 3) Proponer desplazamiento
        Vector3 desplazamiento = dir * (velActual * Time.fixedDeltaTime);

        // 4) SweepTest para NO empujar otras cosas
        Vector3 desplazSeguro = desplazamiento;

        if (desplazamiento.sqrMagnitude > 0f)
        {
            Vector3 dirNorm = desplazamiento.normalized;
            float dist = desplazamiento.magnitude;

            if (rb.SweepTest(dirNorm, out RaycastHit hit, dist))
            {
                // Ignora triggers (sensor, etc.)
                if (!hit.collider.isTrigger)
                {
                    // opcional: si quieres ignorar al player o su sensor por layers, hazlo aquí

                    float margen = 0.02f;
                    float safeDist = Mathf.Max(0f, hit.distance - margen);

                    desplazSeguro = dirNorm * safeDist;

                    // Si no puedes avanzar, para
                    if (safeDist <= 0.001f)
                        velActual = 0f;
                }
            }
        }

        // 5) Mover caja y player con el mismo desplazamiento "seguro"
        rb.MovePosition(rb.position + desplazSeguro);
        characterController.Move(step + desplazSeguro);
    }


}
