using Unity.VisualScripting;
using UnityEngine;

public class CollectMask : MonoBehaviour
{
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
                        
                        Destroy(gameObject);
                    } 
                }
            }
        }
    }
}
