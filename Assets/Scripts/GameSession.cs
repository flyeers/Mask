using System.Collections.Generic;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance {  get; private set; }

    //Datos mantenidos gracias al cum
    public List<MaskSO> CollectedMasks = new List<MaskSO>();
    public int ActiveMaskIndex = -1;

    public int SpawnPointIndex { get; set; } = -1;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CaptureFrom(MaskInventory inventory)
    {
        CollectedMasks = inventory.GetInventoryCopy();
        ActiveMaskIndex = inventory.GetCurrentIndex();
    }

    public void ApplyTo(MaskInventory inventory)
    {
        inventory.SetInventory(CollectedMasks, ActiveMaskIndex);
    }

    public void Clear()
    {
        SpawnPointIndex = -1;
        CollectedMasks.Clear();
    }
}
