using UnityEngine;

public class AutoCreditsScroll : MonoBehaviour
{
    public float scrollSpeed = 400f; // prueba con 200-800 para UI

    private RectTransform rectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // suma, no reemplaza
        rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
        // si quieres que baje en vez de subir: Vector2.down
    }
}
