using UnityEngine;
using DG.Tweening;
using Input;

public class HookAbility : MonoBehaviour
{
    [SerializeField] private HookProjectile _projectilePrefab;
    [SerializeField] private Transform _projectileSpawnPoint;
    [SerializeField] private float _upDistance;
    [SerializeField] private float _upTime;

    private bool _lock;

    private HookProjectile _currentProjectile;
    private PlayerInputController _playerInputController;
    private ThirdPersonController _thirdPersonController;

    private void Awake()
    {
        _playerInputController = GetComponent<PlayerInputController>();
        _thirdPersonController = GetComponent<ThirdPersonController>();
    }

    private void OnEnable()
    {
        if (_playerInputController != null)
        {
            _playerInputController.UseAbility += Execute;
        }
    }

    private void OnDisable()
    {
        if (_playerInputController != null)
        {
            _playerInputController.UseAbility -= Execute;
        }
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
        
        _thirdPersonController.EnableAllMovement(false);
        
        _currentProjectile.MoveUp();
    }

    private void OnProjectileDestroyed()
    {
        _lock = false;
        
        _thirdPersonController.EnableAllMovement(true);
    }

    private void ReleaseHook()
    {
        if (_currentProjectile == null)
        {
            return;
        }
        
        var currentProjectile = _currentProjectile;
        currentProjectile.OnContact -= OnContact;
        
        Destroy(currentProjectile.gameObject);
        
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