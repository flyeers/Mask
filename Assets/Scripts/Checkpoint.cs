using Managers;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GeneralManager.Instance.GameManager != null)
            GeneralManager.Instance.GameManager.CancelkillCountdown();
        else
            Debug.LogError("GameManager no encontrado en la escena");

        var inv = other.GetComponent<MaskInventory>();
        if (inv != null && GameSession.Instance != null)
        {
            GameSession.Instance.CaptureFrom(inv);
        }
    }
}
