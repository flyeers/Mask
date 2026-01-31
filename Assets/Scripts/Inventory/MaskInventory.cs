using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskInventory : MonoBehaviour
{
    [SerializeField] private List<MaskSO> inventory = new List<MaskSO>();

    private void Awake()
    {
        if (inventory.Count > 0) 
        {
            ActivateItem(inventory[0]);
        }
    }

    public void AddItem(MaskSO maskSO)
    {
        inventory.Add(maskSO);

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
                    if (mask == maskSO) ((Behaviour)comp).enabled = true;
                    else ((Behaviour)comp).enabled = false;
                }
            }
        }
    }

}
