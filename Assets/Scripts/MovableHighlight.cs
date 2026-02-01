using UnityEngine;

public class MovableHighlight : MonoBehaviour
{
    [Header("Ajustes")]
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private Material highlightMaterial; // material rojo/emissive
    [SerializeField] private bool useMaterialSwap = true;

    private Material[][] originalMats;
    private bool isHighlighted;

    private void Awake()
    {
        if (renderers == null || renderers.Length == 0)
            renderers = GetComponentsInChildren<Renderer>();

        // Guardamos materiales originales
        originalMats = new Material[renderers.Length][];
        for (int i = 0; i < renderers.Length; i++)
            originalMats[i] = renderers[i].materials;
    }

    public void SetHighlight(bool on)
    {
        if (isHighlighted == on) return;
        isHighlighted = on;

        if (!useMaterialSwap) return;

        for (int i = 0; i < renderers.Length; i++)
        {
            if (on)
            {
                // Añadimos un material extra al final (efecto “outline fake” / brillo)
                var mats = renderers[i].materials;
                var newMats = new Material[mats.Length + 1];
                for (int m = 0; m < mats.Length; m++) newMats[m] = mats[m];
                newMats[newMats.Length - 1] = highlightMaterial;
                renderers[i].materials = newMats;
            }
            else
            {
                renderers[i].materials = originalMats[i];
            }
        }
    }
}
