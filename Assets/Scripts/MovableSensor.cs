using System.Collections.Generic;
using UnityEngine;

public class MovableSensor : MonoBehaviour
{
    public readonly List<Rigidbody> Candidates = new List<Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("movable")) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb != null && !Candidates.Contains(rb))
            Candidates.Add(rb);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("movable")) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
            Candidates.Remove(rb);
    }

    // ✅ Devuelve el rigidbody cuya "base" (bounds.min.y) sea la más baja.
    // Esto evita que pilles la caja de arriba por pivots raros.
    public Rigidbody GetLowestByBounds()
    {
        Rigidbody best = null;
        float bestMinY = float.MaxValue;

        for (int i = 0; i < Candidates.Count; i++)
        {
            Rigidbody r = Candidates[i];
            if (r == null) continue;

            Collider c = r.GetComponent<Collider>();
            if (c == null) continue;

            float minY = c.bounds.min.y;
            if (minY < bestMinY)
            {
                bestMinY = minY;
                best = r;
            }
        }

        return best;
    }
}
