using System;
using Managers;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private int levelBuildIndex = -1;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (levelBuildIndex > 0)
        {
            GeneralManager.Instance.SceneController.LoadLevel(levelBuildIndex);
        }
    }
}
