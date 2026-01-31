using System;
using UnityEngine;
using UnityEngine.UI;

public class MaskInventoryUI : MonoBehaviour
{
    [SerializeField] private Image[] masks;
    [SerializeField] float alphaUnselected = 0.5f;

    private int currentMascksCount = 0;

    public void AddItemUI(Sprite MaskSprite) 
    {
        currentMascksCount++;
        masks[currentMascksCount].enabled = true;
        if(MaskSprite) masks[currentMascksCount].sprite = MaskSprite;
    }

    public void ActivateItemUI(int index)
    {
        if (index >= masks.Length || index > currentMascksCount) return;

        for (int i = 0; i < currentMascksCount; i++)
        {
            Color c = masks[i].color;
            if (masks[i].enabled && i != index) // not active
            {
                c.a = alphaUnselected;
            }
            else 
            {
                c.a = 1;
            }

            masks[i].color = c;
        }
    }
}
