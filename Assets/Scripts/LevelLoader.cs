using Managers;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private int levelBuildIndex = -1;
    [SerializeField] private int spawnPointIndex = -1;

    private bool _loaded = false;
    
    private void OnTriggerEnter(Collider other)
    {
        if (_loaded) return;
        if (!other.CompareTag("Player")) return;
        if (levelBuildIndex > 0)
        {
            GeneralManager.Instance.SceneController.LoadLevel(levelBuildIndex);
            GameSession.Instance.SpawnPointIndex = spawnPointIndex;
            _loaded = true;
        }
    }
}
