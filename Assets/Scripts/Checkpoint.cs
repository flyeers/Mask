using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{

    [SerializeField] GameManager gameManager;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (gameManager != null)
            gameManager.CancelkillCountdown();
        else
            Debug.LogError("GameManager no encontrado en la escena");


        var inv = other.GetComponent<MaskInventory>();
        if (inv != null && GameSession.Instance != null)
        {
            GameSession.Instance.CaptureFrom(inv);
        }
    }
}
