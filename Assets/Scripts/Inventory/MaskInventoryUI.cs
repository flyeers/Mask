using System;
using UnityEngine;
using UnityEngine.UI;

public class MaskInventoryUI : MonoBehaviour
{
    [SerializeField] private Image[] masks;
    [SerializeField] float alphaUnselected = 0.5f;

    private int currentMascksCount = -1;

    public void AddItemUI(Sprite MaskSprite) 
    {
        currentMascksCount++;
        if (currentMascksCount >= masks.Length) return;
        masks[currentMascksCount].gameObject.SetActive(true);
        if(MaskSprite) masks[currentMascksCount].sprite = MaskSprite;
    }

    public void ActivateItemUI(int index)
    {
        if (index >= masks.Length || index > currentMascksCount) return;

        for (int i = 0; i <= currentMascksCount; i++)
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

    public void Clear()
    {
        // Resetea el contador: no hay ninguna máscara mostrada
        currentMascksCount = -1;

        // Apaga todas las casillas y las deja “limpias”
        for (int i = 0; i < masks.Length; i++)
        {
            if (masks[i] == null) continue;

            masks[i].gameObject.SetActive(false);
            masks[i].sprite = null;

            // opcional: deja el alpha a 1 por defecto
            Color c = masks[i].color;
            c.a = 1f;
            masks[i].color = c;
        }
    }

}
