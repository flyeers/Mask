using UnityEngine;
using DG.Tweening;

public class HookAbility : MonoBehaviour
{
    [SerializeField] private HookProjectile _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private float _upDistance;
    [SerializeField] private float _upTime;

    private HookProjectile _currentProjectile;

    private void Start()
    {
        FireHook();
    }

    private void FireHook()
    {
        if (_currentProjectile != null)
        {
            return;
        }
        
        _currentProjectile = Instantiate(_projectilePrefab, _projectileSpawnPoint);
        _currentProjectile.OnContact += OnContact;
        
        _currentProjectile.MoveUp();
    }

    private void OnContact()
    {
        var originalPosition = transform.position;
        
        DOVirtual.Float(0f, _upDistance, _upTime, distance =>
        {
            transform.position = originalPosition + Vector3.up * distance;
            _currentProjectile.UpdateOrigin(distance);

        }).SetEase(Ease.InSine);
    }
}