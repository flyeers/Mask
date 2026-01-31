using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointLoadScene : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var inv = other.GetComponent<MaskInventory>();
        if (inv != null && GameSession.Instance != null)
        {
            GameSession.Instance.CaptureFrom(inv);
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
