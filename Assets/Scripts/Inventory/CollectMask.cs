using System;
using Managers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CollectMask : MonoBehaviour
{

    [SerializeField] private float secondsToDie = 500f;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject messageUIPrefab;
    [TextArea(3, 10)]
    [SerializeField] private string messageText;

    [SerializeField] private bool timerToDie = true;

    public static event Action<string> OnMaskCollected;
    private void Start()
    {
        gameManager = GeneralManager.Instance.GameManager;
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
                        
                        OnMaskCollected?.Invoke(messageText);

                        if (gameManager != null && timerToDie)
                            gameManager.StartKillCountdown(secondsToDie);

                        Destroy(gameObject);
                    } 
                }
            }
        }
    }
}
