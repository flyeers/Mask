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
        {
            Candidates.Add(rb);

            // 🔴 activar highlight
            var h = rb.GetComponent<MovableHighlight>();
            if (h != null) h.SetHighlight(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("movable")) return;

        Rigidbody rb = other.attachedRigidbody;
        if (rb != null)
        {
            Candidates.Remove(rb);

            // ⚫ desactivar highlight
            var h = rb.GetComponent<MovableHighlight>();
            if (h != null) h.SetHighlight(false);
        }
    }

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
