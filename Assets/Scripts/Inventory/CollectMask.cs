using Managers;
using Unity.VisualScripting;
using UnityEngine;

public class CollectMask : MonoBehaviour
{

    [SerializeField] private float secondsToDie = 5f;

    [SerializeField] private GameManager gameManager;

    private void Awake()
    {
        
        gameManager = FindFirstObjectByType<GameManager>();
        if (gameManager == null)
            Debug.LogError("No hay GameManager en la escena.");
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<MaskInventory>(out MaskInventory maskInventory)) 
            {
                if (TryGetComponent<MaskItem>(out MaskItem maskItem)) 
                { 
                    if (maskItem.maskSo != null) 
                    { 
                        maskInventory.AddItem(maskItem.maskSo);

                        if (gameManager != null)
                            gameManager.StartKillCountdown(secondsToDie);
                        else
                            Debug.LogError("GameManager no encontrado en la escena");

                        Destroy(gameObject);
                    } 
                }
            }
        }
    }
}
