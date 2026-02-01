using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MovableSupportWatcher : MonoBehaviour
{
    public event Action<MovableSupportWatcher> LostSupport;

    [Header("Soporte (4 raycasts)")]
    [SerializeField] private float soporteRayDist = 0.15f;
    [SerializeField] private float soporteStartOffset = 0.03f;
    [SerializeField] private float soporteCornerInset = 0.04f;
    [SerializeField] private LayerMask soporteMask = ~0;

    private Collider col;
    private bool isWatched;      // solo chequea cuando está agarrada
    private bool hadSupport = true;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    public void SetWatched(bool watched)
    {
        isWatched = watched;
        hadSupport = true; // resetea estado al empezar a vigilar
    }

    private void FixedUpdate()
    {
        if (!isWatched) return;

        bool hasSupport = HasSupport4Rays();

        // Disparamos solo en el “flanco” (cuando pasa de tener soporte a no tenerlo)
        if (hadSupport && !hasSupport)
        {
            LostSupport?.Invoke(this);
        }

        hadSupport = hasSupport;
    }

    private bool HasSupport4Rays()
    {
        if (col == null) return true;

        Bounds b = col.bounds;

        float minX = b.min.x + soporteCornerInset;
        float maxX = b.max.x - soporteCornerInset;
        float minZ = b.min.z + soporteCornerInset;
        float maxZ = b.max.z - soporteCornerInset;

        float y = b.min.y + soporteStartOffset;

        Vector3[] origins =
        {
            new Vector3(minX, y, minZ),
            new Vector3(minX, y, maxZ),
            new Vector3(maxX, y, minZ),
            new Vector3(maxX, y, maxZ),
        };

        for (int i = 0; i < origins.Length; i++)
        {
            if (Physics.Raycast(origins[i], Vector3.down, out RaycastHit hit,
                                soporteRayDist, soporteMask, QueryTriggerInteraction.Ignore))
            {
                if (hit.collider != null && !hit.collider.isTrigger)
                    return true;
            }
        }

        return false;
    }
}
