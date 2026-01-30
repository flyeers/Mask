using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskInventory : MonoBehaviour
{
    [SerializeField] private List<MaskItem> inventory = new List<MaskItem>();

    private void Awake()
    {
        if (inventory.Count > 0) 
        {
            ActivateItem(inventory[0].maskSo);
        }
    }

    public void AddItem(MaskSO maskSO)
    {
        MaskItem maskItem = new MaskItem(maskSO);
        inventory.Add(maskItem);

        //ACTIVATE ITEM 
        ActivateItem(maskSO);
    }

    public void ActivateItem(MaskSO maskSO) 
    {
        foreach (MaskItem maskItem in inventory) 
        {
            Type tipo = Type.GetType(maskItem.maskSo.AbilityScriptName);
            if (tipo != null)
            {
                Component comp = GetComponent(tipo);

                if (comp != null)
                {
                    if (maskItem.maskSo == maskSO) ((Behaviour)comp).enabled = true;
                    else ((Behaviour)comp).enabled = false;
                }
            }
        }
    }

}
