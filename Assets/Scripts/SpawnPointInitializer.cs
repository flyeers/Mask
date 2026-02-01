using System.Collections.Generic;
using UnityEngine;

public class SpawnPointInitializer : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private List<Transform> _spawnPoints;
    
    private void Start()
    {
        var spawnPointIndex = GameSession.Instance.SpawnPointIndex;

        if (spawnPointIndex != -1)
        {
            _target.transform.position = _spawnPoints[spawnPointIndex].position;
        }
    }
}
