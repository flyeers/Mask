using UnityEngine;
using DG.Tweening;

public class HookAbility : MonoBehaviour
{
    [SerializeField] private HookProjectile _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private float _upDistance;
    [SerializeField] private float _upTime;

    private bool _lock;

    private HookProjectile _currentProjectile;

    private void Start()
    {
        Execute();
    }

    private void Execute()
    {
        if (_currentProjectile == null)
        {
            FireHook();
        }
        else if (!_lock)
        {
            ReleaseHook();
        }
    }

    private void FireHook()
    {
        if (_currentProjectile != null)
        {
            return;
        }
        
        _lock = true;
        _currentProjectile = Instantiate(_projectilePrefab, _projectileSpawnPoint);
        _currentProjectile.OnContact += OnContact;
        _currentProjectile.OnDestroyed += OnProjectileDestroyed;
        
        _currentProjectile.MoveUp();
    }

    private void OnProjectileDestroyed()
    {
        _lock = false;
    }

    private void ReleaseHook()
    {
        if (_currentProjectile == null)
        {
            return;
        }
        
        var currentProjectile = _currentProjectile;
        currentProjectile.OnContact -= OnContact;
        currentProjectile.OnDestroyed -= OnProjectileDestroyed;
        
        Destroy(currentProjectile);
        
        _currentProjectile = null;
    }

    private void OnContact()
    {
        var originalPosition = transform.position;
        
        DOVirtual.Float(0f, _upDistance, _upTime, distance =>
        {
            transform.position = originalPosition + Vector3.up * distance;
            _currentProjectile.UpdateOrigin(distance);
        }).SetEase(Ease.InSine).OnComplete(() =>
        {
            _lock = false;
        });
    }
}