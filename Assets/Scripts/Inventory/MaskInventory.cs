using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskInventory : MonoBehaviour
{
    [SerializeField] private List<MaskSO> inventory = new List<MaskSO>();
    [SerializeField] MaskInventoryUI maskInventoryUI;

    private int CurrentItemIndex = -1; 

    private void Awake()
    {
        if (inventory.Count > 0) 
        {
            ActivateItem(inventory[0]);
            CurrentItemIndex = 0;
        }
    }

    public void AddItem(MaskSO maskSO)
    {
        inventory.Add(maskSO);
        if(maskInventoryUI) maskInventoryUI.AddItemUI(maskSO.MaskSprite);
        //ACTIVATE ITEM 
        ActivateItem(maskSO);
    }

    public void ActivateItem(MaskSO maskSO) 
    {
        foreach (MaskSO mask in inventory) 
        {
            Type tipo = Type.GetType(mask.AbilityScriptName);
            if (tipo != null)
            {
                Component comp = GetComponent(tipo);

                if (comp != null)
                {
                    if (mask == maskSO) 
                    { 
                        ((Behaviour)comp).enabled = true;
                        CurrentItemIndex = inventory.IndexOf(maskSO);
                    } 
                    else ((Behaviour)comp).enabled = false;
                }
            }
        }

        if (maskInventoryUI) maskInventoryUI.ActivateItemUI(inventory.IndexOf(maskSO));
    }
    public void NextItem() 
    {
        if (inventory.Count == 0 || inventory.Count == 1) return;
        if (CurrentItemIndex + 1 >= inventory.Count)
        {
            ActivateItem(inventory[0]);
        }
        else 
        {
            ActivateItem(inventory[CurrentItemIndex + 1]);
        }
    }

    public void PrevItem()
    {
        if (inventory.Count == 0 || inventory.Count == 1) return;
        if (CurrentItemIndex - 1 < 0)
        {
            ActivateItem(inventory[inventory.Count - 1]);
        }
        else
        {
            ActivateItem(inventory[CurrentItemIndex - 1]);
        }
    }

}
