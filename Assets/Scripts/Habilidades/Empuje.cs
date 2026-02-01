using System.Collections.Generic;
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
    [SerializeField] private float velocidadArrastre = 0.8f;
    [SerializeField] private float aceleracionArrastre = 10f;

    [Header("Stack (pila)")]
    [SerializeField] private int maxStack = 8;
    [SerializeField] private float rayUpMaxDist = 0.8f;
    [SerializeField] private float rayUpStartOffset = 0.02f;

    [Header("Fricción al soltar (para que no se deslicen)")]
    [SerializeField] private float dragSoltado = 8f;
    [SerializeField] private float angularDragSoltado = 8f;

    [Header("Auto-suelta si está en el aire")]
    [Tooltip("Qué cuenta como 'soporte' debajo: suelo y/o otras cajas. Pon aquí los layers correctos.")]
    [SerializeField] private LayerMask soporteMask = ~0; // por defecto: Everything

    [Tooltip("Distancia máxima para considerar que hay suelo debajo.")]
    [SerializeField] private float distSoporte = 0.25f;

    [Tooltip("Empieza el rayo un poco por encima de la base para evitar fallos por penetración.")]
    [SerializeField] private float startOffsetY = 0.05f;

    [Tooltip("Mete un pelín las esquinas hacia dentro para no raycastear fuera si hay bordes.")]
    [SerializeField] private float cornerInset = 0.02f;

    [Tooltip("Tiempo mínimo sin soporte antes de soltar (evita falsos positivos por 1 frame).")]
    [SerializeField] private float tiempoSinSoporteParaSoltar = 0.12f;

    private float noSupportTimer;

    private bool agarrando;

    private Rigidbody rbBase;
    private Transform trBase;

    private readonly List<Rigidbody> stack = new List<Rigidbody>();

    private Vector3 agarreAxisWorld;
    private float velActual;

    private struct RBState
    {
        public bool isKinematic;
        public bool useGravity;
        public RigidbodyConstraints constraints;
        public float drag;
        public float angularDrag;
    }
    private readonly Dictionary<Rigidbody, RBState> prev = new Dictionary<Rigidbody, RBState>();

    private void Awake()
    {
        if (playerInputController == null) playerInputController = GetComponent<PlayerInputController>();
        if (sensor == null) sensor = GetComponentInChildren<MovableSensor>();
        if (characterController == null) characterController = GetComponent<CharacterController>();
        if (thirdPersonController == null) thirdPersonController = GetComponent<ThirdPersonController>();

        if (playerInputController == null) Debug.LogError("Empuje: No hay PlayerInputController");
        if (sensor == null) Debug.LogError("Empuje: No hay MovableSensor en hijos");
        if (characterController == null) Debug.LogError("Empuje: No hay CharacterController");
        if (thirdPersonController == null) Debug.LogError("Empuje: No hay ThirdPersonController");
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
            Rigidbody basePick = (sensor != null) ? sensor.GetLowestByBounds() : null;
            Debug.Log("ToggleAgarrar: basePick = " + (basePick != null ? basePick.name : "NULL"));
            if (basePick == null) return;

            rbBase = basePick;
            trBase = rbBase.transform;

            agarrando = true;
            noSupportTimer = 0f;

            if (thirdPersonController != null) thirdPersonController.SetCanMove(false);

            CalcularLadoMasCercano(trBase);

            BuildStackFromBase(rbBase);

            prev.Clear();
            for (int i = 0; i < stack.Count; i++)
            {
                Rigidbody r = stack[i];
                if (r == null) continue;

                prev[r] = new RBState
                {
                    isKinematic = r.isKinematic,
                    useGravity = r.useGravity,
                    constraints = r.constraints,
                    drag = r.linearDamping,
                    angularDrag = r.angularDamping
                };

                // OJO: si ya es kinematic, no le seteamos velocity (Unity avisa).
                if (!r.isKinematic)
                {
                    r.linearVelocity = Vector3.zero;
                    r.angularVelocity = Vector3.zero;
                }

                r.isKinematic = true;
                r.useGravity = false;
                r.constraints = RigidbodyConstraints.FreezeRotation;
            }

            velActual = 0f;
            Debug.Log("Agarrado stack size = " + stack.Count);
        }
        else
        {
            ForceRelease("Manual");
        }
    }

    private void ForceRelease(string reason)
    {
        if (!agarrando) return;

        Debug.Log("ForceRelease: " + reason);

        agarrando = false;

        for (int i = 0; i < stack.Count; i++)
        {
            Rigidbody r = stack[i];
            if (r == null) continue;

            // IMPORTANTE: si es kinematic, no intentes setear velocities.
            // simplemente lo devolvemos a dinámico y ya.

            r.isKinematic = false;
            r.useGravity = true;
            r.constraints = RigidbodyConstraints.FreezeRotation;

            r.linearDamping = dragSoltado;
            r.angularDamping = angularDragSoltado;
        }

        stack.Clear();
        prev.Clear();

        rbBase = null;
        trBase = null;

        if (thirdPersonController != null) thirdPersonController.SetCanMove(true);

        velActual = 0f;
        noSupportTimer = 0f;
    }

    private void BuildStackFromBase(Rigidbody baseRb)
    {
        stack.Clear();
        stack.Add(baseRb);

        Rigidbody current = baseRb;
        for (int i = 0; i < maxStack - 1; i++)
        {
            Rigidbody next = GetBoxOnTop(current);
            if (next == null) break;
            if (stack.Contains(next)) break;

            stack.Add(next);
            current = next;
        }
    }

    private Rigidbody GetBoxOnTop(Rigidbody bottom)
    {
        Collider c = bottom.GetComponent<Collider>();
        if (c == null) return null;

        Vector3 origin = new Vector3(c.bounds.center.x, c.bounds.max.y + rayUpStartOffset, c.bounds.center.z);

        if (Physics.Raycast(origin, Vector3.up, out RaycastHit hit, rayUpMaxDist, ~0, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider != null && hit.collider.CompareTag("movable"))
                return hit.collider.attachedRigidbody;
        }

        return null;
    }

    private void CalcularLadoMasCercano(Transform tr)
    {
        Vector3 local = tr.InverseTransformPoint(transform.position);
        local.y = 0f;

        float ax = Mathf.Abs(local.x);
        float az = Mathf.Abs(local.z);

        if (ax > az)
        {
            float sign = Mathf.Sign(local.x);
            Vector3 axis = tr.right * sign;
            axis.y = 0f;
            agarreAxisWorld = axis.normalized;
        }
        else
        {
            float sign = Mathf.Sign(local.z);
            Vector3 axis = tr.forward * sign;
            axis.y = 0f;
            agarreAxisWorld = axis.normalized;
        }
    }

    // ✅ 4 raycasts desde esquinas. Devuelve true si hay AL MENOS un soporte debajo.
    private bool HasAnySupport4Corners(Rigidbody r)
    {
        Collider c = r.GetComponent<Collider>();
        if (c == null) return true;

        Bounds b = c.bounds;

        float inset = Mathf.Min(cornerInset, Mathf.Min(b.extents.x, b.extents.z) * 0.45f);

        float minX = b.min.x + inset;
        float maxX = b.max.x - inset;
        float minZ = b.min.z + inset;
        float maxZ = b.max.z - inset;

        // 🔧 origen justo en la base + un pelín arriba
        float y = b.min.y + startOffsetY;

        Vector3[] origins =
        {
        new Vector3(minX, y, minZ),
        new Vector3(minX, y, maxZ),
        new Vector3(maxX, y, minZ),
        new Vector3(maxX, y, maxZ),
    };

        bool anyHit = false;

        for (int i = 0; i < origins.Length; i++)
        {
            Vector3 o = origins[i];
            Debug.DrawRay(o, Vector3.down * distSoporte, Color.red);

            if (Physics.Raycast(o, Vector3.down, out RaycastHit hit, distSoporte, soporteMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider == null) continue;
                if (hit.collider.isTrigger) continue;
                if (hit.collider == c) continue;
                if (hit.rigidbody == r) continue;

                Debug.DrawRay(o, Vector3.down * hit.distance, Color.green);

                // ✅ soporte encontrado
                anyHit = true;

                // DEBUG: solo cuando estás agarrando, imprime 1 vez cada cierto tiempo si quieres
                // Debug.Log($"Corner {i} hit: {hit.collider.name} dist={hit.distance} layer={hit.collider.gameObject.layer}");
            }
            else
            {
                // Debug.Log($"Corner {i} NO HIT (distSoporte={distSoporte})");
            }
        }

        return anyHit;
    }


    private void FixedUpdate()
    {
        if (!agarrando || rbBase == null || trBase == null || playerInputController == null) return;

        // ✅ Auto-suelta SOLO si NO hay soporte en NINGUNA esquina durante X segundos
        if (!HasAnySupport4Corners(rbBase))
        {
            noSupportTimer += Time.fixedDeltaTime;
            if (noSupportTimer >= tiempoSinSoporteParaSoltar)
            {
                ForceRelease("Base completamente en el aire");
                return;
            }
        }
        else
        {
            noSupportTimer = 0f;
        }

        // 1) Recolocar player al punto de agarre (respecto a BASE)
        Vector3 objetivoPlayer = rbBase.position + agarreAxisWorld * distanciaAgarre;
        objetivoPlayer.y = transform.position.y;

        Vector3 delta = objetivoPlayer - transform.position;
        delta.y = 0f;

        Vector3 step = Vector3.zero;
        if (delta.magnitude > tolerancia)
        {
            step = delta.normalized * (velocidadRecolocar * Time.fixedDeltaTime);
            if (step.magnitude > delta.magnitude) step = delta;
        }

        // 2) Input proyectado sobre el eje permitido
        Vector2 move = playerInputController.ReadMove();

        Vector3 dir = -agarreAxisWorld;

        Vector3 worldInput = transform.right * move.x + transform.forward * move.y;
        worldInput.y = 0f;

        float v = Vector3.Dot(worldInput, dir);
        v = Mathf.Clamp(v, -1f, 1f);

        float dead = 0.05f;
        if (Mathf.Abs(v) < dead)
        {
            velActual = 0f;
            characterController.Move(step);
            return;
        }

        float velObjetivo = v * velocidadArrastre;
        velActual = Mathf.MoveTowards(velActual, velObjetivo, aceleracionArrastre * Time.fixedDeltaTime);

        Vector3 desplazamiento = dir * (velActual * Time.fixedDeltaTime);

        // 3) SweepTest con la BASE para no empujar otras cosas
        Vector3 desplazSeguro = desplazamiento;

        if (desplazamiento.sqrMagnitude > 0f)
        {
            Vector3 dirNorm = desplazamiento.normalized;
            float dist = desplazamiento.magnitude;

            if (rbBase.SweepTest(dirNorm, out RaycastHit hit, dist))
            {
                if (!hit.collider.isTrigger)
                {
                    float margen = 0.02f;
                    float safeDist = Mathf.Max(0f, hit.distance - margen);
                    desplazSeguro = dirNorm * safeDist;

                    if (safeDist <= 0.001f)
                        velActual = 0f;
                }
            }
        }

        // 4) Mover TODA la pila
        for (int i = 0; i < stack.Count; i++)
        {
            Rigidbody r = stack[i];
            if (r == null) continue;
            r.MovePosition(r.position + desplazSeguro);
        }

        // 5) Mover player
        characterController.Move(step + desplazSeguro);
    }
}
